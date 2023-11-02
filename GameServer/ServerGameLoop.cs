using System.Collections.Generic;
using System.Threading.Tasks;
using Remouse.GameServer.Players;
using Remouse.Core;
using Remouse.DIContainer;
using Remouse.Models;
using Remouse.Models.Messages;

namespace Remouse.GameServer
{
    public class ServerGameLoop
    {
        private SimulationHost _simulationHost;
        private WorldCommandBuffer _commandBuffer;
        private IConnectedPlayers _connectedPlayers;
        
        public void Construct(Container container)
        {
            _commandBuffer = container.Resolve<WorldCommandBuffer>();
            _simulationHost = container.Resolve<SimulationHost>();
            _connectedPlayers = container.Resolve<IConnectedPlayers>();
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
            foreach (var player in _connectedPlayers.Players)
            {
                if (player.Data.sessionData.state != PlayerState.InGame)
                    continue;
                
                player.Send(new SimulationUpdateMessage(commands, _simulationHost.Simulation.CurrentTick));
            }
        }
    }
}