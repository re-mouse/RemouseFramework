using Remouse.Shared.Models.Messages;

namespace Remouse.GameServer.ServerTransport
{
    internal interface IPlayersDataHandler
    {
        public void HandleMessage(PlayerConnection player, NetworkMessage message);
    }
}