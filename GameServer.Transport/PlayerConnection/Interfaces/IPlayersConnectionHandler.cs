using GameServer.ServerTransport;

namespace GameServer.Players
{
    internal interface IPlayersConnectionHandler
    {
        void HandleConnected(PlayerConnection player);
        void HandleDisconnected(PlayerConnection player);
    }
}