namespace Remouse.GameServer.ServerTransport
{
    internal interface IServerSocket
    {
        public void Start(ServerArgs args);
        public void SetHandler(IServerSocketEventsHandler handler);
        public void Stop();
        public void Update();
    }
    
    internal class ServerArgs
    {
        public int port;
        public int maxPlayers;
    }
}