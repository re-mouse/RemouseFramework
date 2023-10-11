using System.Net;
using Shared.Serialization;

namespace GameServer.ServerTransport
{
    internal abstract class Connection
    {
        public IPEndPoint endPoint { get; }

        public Connection(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
        }
        
        public abstract void Send(INetworkWriter data, DeliveryMethod deliveryMethod);
        public abstract void Disconnect();
    }
}