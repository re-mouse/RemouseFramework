using System.Net;

namespace Remouse.Network.Sockets
{
    public abstract class ConnectionRequest
    {
        public string Authorization { get; }
        public IPEndPoint EndPoint { get; }

        internal ConnectionRequest(string authorization, IPEndPoint endPoint)
        {
            Authorization = authorization;
            EndPoint = endPoint;
        }
        
        public abstract Connection Accept();
        public abstract void Reject();
    }
}