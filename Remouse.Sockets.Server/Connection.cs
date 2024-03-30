using System.Net;

namespace Remouse.Network.Sockets
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