using System.Net;

namespace GameServer.ServerTransport
{
    internal abstract class ConnectionRequest
    {
        public byte[] data { get; }
        public IPEndPoint endPoint { get; }

        public ConnectionRequest(byte[] data, IPEndPoint endPoint)
        {
            this.data = data;
            this.endPoint = endPoint;
        }
        
        public abstract Connection Accept();
        public abstract void Reject();
    }
}