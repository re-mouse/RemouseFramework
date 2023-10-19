using System.Collections.Generic;
using System.Threading.Tasks;
using Remouse.GameServer.Players;
using Remouse.GameServer.ServerShards;
using Remouse.Shared.Core;
using Remouse.Shared.DIContainer;
using Remouse.Shared.Models;
using Remouse.Shared.Models.Messages;

namespace Remouse.GameServer
{
    public class ServerGameLoop
    {
        private SimulationHost _simulationHost;
        private WorldCommandBuffer _commandBuffer;
        private IPlayersProvider _playersProvider;
        private SimulationFactory _simulationFactory;
        
        public void Construct(Container container)
        {
            container.Get(out _commandBuffer);
            container.Get(out _simulationHost);
            container.Get(out _playersProvider);
            container.Get(out _simulationFactory);
        }

        public async Task Initialize()
        {
            var simulation = _simulationFactory.CreateGameSimulation();
            
            _simulationHost.SetGameSimulation(simulation);
        }

        public void Update()
        {
            var commands = _commandBuffer.GetCurrentCommands();
            foreach (var command in commands)
            {
                _simulationHost.Simulation.RunCommand(command);
            }

            SendCommandsToPlayers(commands);
            
            _simulationHost.Simulation.Tick();
            
            _commandBuffer.SyncTickWithSimulation();
        }

        private void SendCommandsToPlayers(HashSet<WorldCommand> commands)
        {
            foreach (var player in _playersProvider.Players)
            {
                if (player.Data.sessionData.state != PlayerState.InGame)
                    continue;
                
                player.Send(new WorldCommandsAtTickMessage(commands, _simulationHost.Simulation.CurrentTick));
            }
        }
    }
}