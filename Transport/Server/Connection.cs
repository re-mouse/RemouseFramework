using System.Net;
using Remouse.Serialization;

namespace Remouse.Transport
{
    public abstract class Connection
    {
        public IPEndPoint EndPoint { get; }

        public Connection(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }
        
        public abstract void Send(INetworkWriter writer, DeliveryMethod deliveryMethod);
        public abstract void Disconnect();
    }
}