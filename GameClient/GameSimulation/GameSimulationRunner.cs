using System.Collections.Generic;
using Shared.GameSimulation;

namespace GameClient
{
    public class GameSimulationRunner
    {
        private const long TickUpdatesLatency = 10;
        
        private IInputCommandsProvider _inputProvider;
        private GameSimulationUpdatesBuffer _gameSimulationUpdatesBuffer;
        private IClientCommandsSender _commandsSender;

        private GameSimulationUpdatesApplier _updatesApplier = new GameSimulationUpdatesApplier();
        
        private GameSimulation _simulation;

        private List<PlayerNetworkMessage> _clientCommandsCached = new List<PlayerNetworkMessage>();

        public void Construct(Shared.DIContainer.Container container)
        {
            container.Get(out _inputProvider);
            container.Get(out _gameSimulationUpdatesBuffer);
            container.Get(out _commandsSender);
        }

        public void SetSimulation(GameSimulation simulation)
        {
            _simulation = simulation;
        }
        
        public void Run()
        {
            if (_simulation == null)
                return;
            
            _inputProvider.GetCommands(_clientCommandsCached);

            foreach (var input in _clientCommandsCached)
            {
                // input.simulationTick = _simulation.simulationTick;
                //
                // if (!input.Validate())
                //     continue;
                //
                // input.Execute();
            }
            
            var updateMessage = _gameSimulationUpdatesBuffer.GetAt(_simulation.simulationTick - TickUpdatesLatency);

            if (updateMessage != null)
            {
                _updatesApplier.ApplyUpdate(_simulation, updateMessage.simulationChanges);
            }
            
            _simulation.Tick();
            
            _gameSimulationUpdatesBuffer.ClearUntil(_simulation.simulationTick - TickUpdatesLatency);

            foreach (var inputCommand in _clientCommandsCached)
            {
                _commandsSender.Send(inputCommand);
            }
        }
    }
}