using System;
using System.Net;
using Remouse.GameClient.Transport;
using Remouse.Serialization;

namespace Remouse.Transport
{
    public interface IClientSocket : IClientSocketEvents, IDisposable
    {
        public void Connect(IPEndPoint host);
        public void Disconnect();
        public void Send(ReadOnlySpan<byte> data, DeliveryMethod deliveryMethod);
        public void Update();
    }
}

namespace Remouse.GameClient.Transport
{
    public interface IClientSocketEvents
    {
        public event Action Connected;
        public event Action ConnectionFailed;
        public event Action Disconnected;
        public event Action<INetworkReader> DataReceived;
    }
}