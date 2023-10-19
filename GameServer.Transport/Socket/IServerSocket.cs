namespace Remouse.GameServer.ServerTransport
{
    internal interface IServerSocket
    {
        public void Start(ushort port);
        public void SetHandler(IServerSocketEventsHandler handler);
        public void Stop();
        public void Update();
    }
}