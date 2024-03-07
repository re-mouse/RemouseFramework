using System.Net;
using System.Threading;
using Cysharp.Threading.Tasks;
using Remouse.Network.Models;
using Remouse.Network.Sockets;

namespace Remouse.Network.Client
{
    public interface IClientTransport
    {
        public bool IsConnected { get; }
        public int PingInMilliseconds { get; }
        public UniTask<ClientConnectResult> ConnectAsync(IPEndPoint endPoint, CancellationToken cancellationToken);
        public void Disconnect();
        public void Update();
        public void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.Sequenced);
    }
    
    public enum ClientConnectResult
    {
        Timeout,
        InvalidCredentials,
        Cancelled,
        UnknownError,
        Successful,
    }
}