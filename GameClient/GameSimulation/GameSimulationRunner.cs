using System.Collections.Generic;
using Remouse.Shared.Core;
using Remouse.Shared.DIContainer;

namespace GameClient
{
    public class GameSimulationRunner
    {
        private const long TickUpdatesLatency = 10;
        
        private IInputCommandsProvider _inputProvider;
        private GameSimulationUpdatesBuffer _gameSimulationUpdatesBuffer;
        private IClientCommandsSender _commandsSender;

        private GameSimulationUpdatesApplier _updatesApplier = new GameSimulationUpdatesApplier();
        
        private Simulation _simulation;

        private List<PlayerNetworkMessage> _clientCommandsCached = new List<PlayerNetworkMessage>();

        public void Construct(Container container)
        {
            container.Get(out _inputProvider);
            container.Get(out _gameSimulationUpdatesBuffer);
            container.Get(out _commandsSender);
        }

        public void SetSimulation(Simulation simulation)
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
            
            var updateMessage = _gameSimulationUpdatesBuffer.GetAt(_simulation.CurrentTick - TickUpdatesLatency);

            if (updateMessage != null)
            {
                _updatesApplier.ApplyUpdate(_simulation, updateMessage.WorldState);
            }
            
            _simulation.Tick();
            
            _gameSimulationUpdatesBuffer.ClearUntil(_simulation.CurrentTick - TickUpdatesLatency);

            foreach (var inputCommand in _clientCommandsCached)
            {
                _commandsSender.Send(inputCommand);
            }
        }
    }
}