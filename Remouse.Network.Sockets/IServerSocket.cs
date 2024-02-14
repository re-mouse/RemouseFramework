using System;
using Remouse.Serialization;

namespace Remouse.Network.Sockets
{
    public interface IServerSocket : IServerSocketEvents, IDisposable
    {
        void Start(ushort port, int maxConnections);
        void Stop();
        void PollEvents();
    }
    
    public interface IServerSocketEvents
    {
        event Action<ConnectionRequest> GotConnectionRequest;
        event Action<Connection> Connected;
        event Action<Connection> Disconnected;
        event Action<Connection, IBytesReader> DataReceived;
    }
}