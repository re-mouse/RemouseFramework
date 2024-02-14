using System.Net;
using Remouse.Serialization;

namespace Remouse.Network.Sockets
{
    public abstract class Connection
    {
        public IPEndPoint EndPoint { get; }

        public Connection(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }
        
        public abstract void Send(IBytesWriter writer, DeliveryMethod deliveryMethod);
        public abstract void Disconnect();
    }
}