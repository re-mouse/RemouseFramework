using System.Net;
using Remouse.Shared.Serialization;

namespace Remouse.GameServer.ServerTransport
{
    internal abstract class Connection
    {
        public IPEndPoint EndPoint { get; }

        public Connection(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }
        
        public abstract void Send(INetworkWriter data, DeliveryMethod deliveryMethod);
        public abstract void Disconnect();
    }
}