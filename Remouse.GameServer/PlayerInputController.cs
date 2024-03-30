using Remouse.Simulation;
using ReDI;
using Remouse.Network.Models;
using Remouse.Network.Server;
using Remouse.Simulation.Network;
using Remouse.Utils;

namespace Remouse.GameServer
{
    internal class PlayerInputController
    {
        [Inject] private ICommandRunner _commandRunner;

        [Inject]
        private void SubscribeOnEvents(IServerTransportEvents events)
        {
            events.MessageReceived += HandleMessage;
        }

        private void HandleMessage(IPlayer player, NetworkMessage message)
        {
            if (message is not BasePlayerInputMessage playerInputMessage)
                return;
            
            HandleInputMessage(player, playerInputMessage);
        }

        private void HandleInputMessage(IPlayer player, BasePlayerInputMessage playerInput)
        {
            var command = playerInput.AsCommand();
            command.playerGameId = player.GameId;
            _commandRunner.EnqueueCommand(command);
            
            LLogger.Current.LogPlayerTrace(this, player, $"Enqueued command [WorldCommand:{command}]");
        }
    }
}