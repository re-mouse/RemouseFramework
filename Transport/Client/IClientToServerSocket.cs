using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Remouse.Serialization;

namespace Remouse.Transport
{
    public interface IClientToServerSocket : IClientSocketEvents, IDisposable
    {
        public bool IsConnected { get; }
        public Task<SocketConnectResult> ConnectAsync(IPEndPoint host, INetworkWriter connectData = null, int timeOutMilliseconds = 2000, CancellationToken cancellationToken = default);
        public void Disconnect();
        public void Send(INetworkWriter data, DeliveryMethod deliveryMethod);
        public void Update();
    }

    public interface IClientSocketEvents
    {
        public event Action Connected;
        public event Action Disconnected;
        public event Action<INetworkReader> DataReceived;
    }

    public enum SocketConnectResult
    {
        Error,
        Successful,
        Timeout,
        Rejected,
        Cancelled,
    }
}