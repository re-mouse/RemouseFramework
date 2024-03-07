using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Remouse.Serialization;

namespace Remouse.Network.Sockets
{
    public interface IClientSocket : IClientSocketEvents, IDisposable
    {
        bool IsConnected { get; }
        public int PingInMilliseconds { get; }
        Task<SocketConnectResult> ConnectAsync(IPEndPoint host, IBytesWriter connectData = null, CancellationToken cancellationToken = default);
        void Disconnect();
        void Send(IBytesWriter data, DeliveryMethod deliveryMethod);
        void PollEvents();
    }

    public interface IClientSocketEvents
    {
        event Action Connected;
        event Action Disconnected;
        event Action<IBytesReader> DataReceived;
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