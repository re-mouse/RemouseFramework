using Shared.DIContainer;
using Shared.GameSimulation;
using Shared.Utils.Log;

namespace GameServer.ServerShards
{
    public class GameSimulationRunner
    {
        private EntityUpdatesTracker _entityUpdatesTracker;
        private PlayerInputBuffer _playerInputBuffer;
        private EntityUpdatesSender _entityUpdatesSender;
        private ILogger _logger;
        private GameSimulation simulation;

        private Container _container;

        public void Construct(Container container)
        {
            container.Get(out _logger);
            container.Get(out _playerInputBuffer);
            container.Get(out _entityUpdatesSender);
            container.Get(out _entityUpdatesTracker);
            container.Get(out simulation);
            
            _container = container;
        }

        public void RunTick(GameSimulation simulation)
        {
            if (simulation == null)
            {
                Logger.Current.LogError(this, "Simulation is null");
                return;
            }
            
            RunTickedClientMessages();

            simulation.Tick();

            SendTrackedUpdatesToPlayers();
            
            _entityUpdatesTracker.Clear();
        }

        private void RunTickedClientMessages()
        {
            var clientMessages = _playerInputBuffer.GetInputMessages(simulation.simulationTick);

            if (clientMessages == null)
                return;
            
            foreach (var message in clientMessages)
            {
                var player = message.PlayerData;
                
                if (message.Validate())
                {
                    Logger.Current.LogPlayerTrace(this, player, $"Message validated. Type: {message.GetType().FullName}");

                    message.Execute();
                    
                    Logger.Current.LogPlayerTrace(this, player, $"Message executed. Type: {message.GetType().FullName}");
                }
                else
                {
                    Logger.Current.LogPlayerWarning(this, player, $"Message not validated. Type: {message.GetType().FullName}");
                }
            }
        }
        
        private void SendTrackedUpdatesToPlayers()
        {
            var updates = _entityUpdatesTracker.GetUpdates();
            
            _entityUpdatesSender.SendUpdatesToObservers(updates);
        }
    }
}