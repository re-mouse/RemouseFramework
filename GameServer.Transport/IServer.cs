namespace Remouse.GameServer.ServerTransport
{
    public interface IServer
    {
        public void Start(ushort port);
        public void Stop();
        public void Update();
    }
}