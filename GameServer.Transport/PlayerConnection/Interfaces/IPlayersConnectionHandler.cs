using Remouse.GameServer.ServerTransport;

namespace Remouse.GameServer.Players
{
    internal interface IPlayersConnectionHandler
    {
        void HandleConnected(PlayerConnection player);
        void HandleDisconnected(PlayerConnection player);
    }
}