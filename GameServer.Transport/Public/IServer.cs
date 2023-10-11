namespace GameServer.ServerTransport
{
    public interface IServer
    {
        public void Start(int port);
        public void Stop();
        public void Update();
    }
}