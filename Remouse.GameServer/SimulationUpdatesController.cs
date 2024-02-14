using System.Collections.Generic;
using System.Linq;
using Remouse.Simulation;
using Remouse.DI;
using Remouse.Network.Server;
using Remouse.Simulation.Network;
using Remouse.Utils;

namespace Remouse.GameServer
{
    internal class SimulationUpdatesController
    {
        private IConnectedPlayers _players;
        private ICommandRunner _commandRunner;
        private ISimulationHost _simulationHost;

        public void Construct(Container container)
        {
            _simulationHost = container.Resolve<ISimulationHost>();
        }
        
        public void SendEnqueuedCommands()
        {
            long tick = _simulationHost.Simulation.CurrentTick;
            var updateMessage = new SimulationUpdateMessage(new HashSet<BaseWorldCommand>(_commandRunner.EnqueuedCommands), tick);
            
            foreach (var player in _players.Players)
            {
                player.Send(updateMessage);
            }
            
            LLogger.Current.LogTrace(this, $"Sent simulation update message [WorldCommandsCount:{updateMessage.Commands.Count}] [PlayersCount:{_players.Players.Count()}] [Tick:{updateMessage.Tick}]");
        }
    }
}