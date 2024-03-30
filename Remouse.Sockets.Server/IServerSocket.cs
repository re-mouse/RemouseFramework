namespace Remouse.Network.Sockets
{
    public interface IServerSocket : IServerSocketEvents, IDisposable
    {
        void Start(ushort port, int maxConnections);
        void Stop();
        event Action<ConnectionRequest> GotConnectionRequest;
        event Action<Connection> Connected;
        event Action<Connection> Disconnected;
        event Action<Connection, byte[]> DataReceived;
    }
    
    public interface IServerSocketEvents
    {
       
    }
}