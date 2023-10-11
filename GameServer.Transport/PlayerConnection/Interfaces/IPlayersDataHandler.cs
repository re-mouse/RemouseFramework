using Shared.Online.Commands;

namespace GameServer.ServerTransport
{
    internal interface IPlayersDataHandler
    {
        public void HandleMessage(PlayerConnection player, NetworkMessage message);
    }
}