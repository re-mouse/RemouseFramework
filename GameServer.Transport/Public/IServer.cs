namespace Remouse.GameServer.ServerTransport
{
    public interface IServer
    {
        public void Start(int port, int maxConnections);
        public void Stop();
        public void Update();
    }
}