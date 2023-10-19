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
        
        public void Construct(Container container)
        {
            container.Get(out _commandBuffer);
            container.Get(out _simulationHost);
            container.Get(out _playersProvider);
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
                
                player.Send(new TickWorldCommandsMessage(commands));
            }
        }
    }
}