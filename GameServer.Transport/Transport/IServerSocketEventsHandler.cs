using Shared.Serialization;

namespace GameServer.ServerTransport
{
    internal interface IServerSocketEventsHandler
    {
        public void HandleConnectionRequest(ConnectionRequest request);
        public void HandleConnected(Connection connection);
        public void HandleDisconnected(Connection connection);
        public void HandleData(Connection connection, INetworkReader reader);
    }
}