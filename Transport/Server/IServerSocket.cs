using System;
using Remouse.Serialization;

namespace Remouse.Transport
{
    public interface IServerSocket : IServerSocketEvents, IDisposable
    {
        public void Start(ushort port, int maxConnections);
        public void Stop();
        public void PollEvents();
    }
    
    public interface IServerSocketEvents
    {
        public event Action<ConnectionRequest> GotConnectionRequest;
        public event Action<Connection> Connected;
        public event Action<Connection> Disconnected;
        public event Action<Connection, INetworkReader> DataReceived;
    }
}