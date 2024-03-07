using System.Collections.Generic;
using System.Linq;
using Remouse.Simulation;
using ReDI;
using Remouse.Network.Server;
using Remouse.Simulation.Network;
using Remouse.Utils;

namespace Remouse.GameServer
{
    internal class SimulationUpdatesController
    {
        [Inject] private IConnectedPlayers _players;
        [Inject] private ICommandRunner _commandRunner;
        [Inject] private ISimulationHost _simulationHost;

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