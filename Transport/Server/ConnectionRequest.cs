using System.Net;

namespace Remouse.Transport
{
    public abstract class ConnectionRequest
    {
        public byte[] Data { get; }
        public IPEndPoint EndPoint { get; }

        internal ConnectionRequest(byte[] data, IPEndPoint endPoint)
        {
            Data = data;
            EndPoint = endPoint;
        }
        
        public abstract Connection Accept();
        public abstract void Reject();
    }
}