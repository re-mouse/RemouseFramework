using System.Net;

namespace Remouse.GameServer.ServerTransport
{
    internal abstract class ConnectionRequest
    {
        public byte[] Data { get; }
        public IPEndPoint EndPoint { get; }

        public ConnectionRequest(byte[] data, IPEndPoint endPoint)
        {
            Data = data;
            EndPoint = endPoint;
        }
        
        public abstract Connection Accept();
        public abstract void Reject();
    }
}