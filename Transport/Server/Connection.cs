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
        
        public abstract void Send(byte[] data, DeliveryMethod deliveryMethod);
        public abstract void Disconnect();
    }
}