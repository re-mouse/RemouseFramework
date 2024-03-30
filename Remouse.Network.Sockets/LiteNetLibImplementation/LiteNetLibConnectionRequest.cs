using System.Text;
using System.Text.Unicode;
using Remouse.Utils;

namespace Remouse.Network.Sockets.Implementations
{
    public class LiteNetLibConnectionRequest : ConnectionRequest
    {
        private readonly LiteNetLib.ConnectionRequest _request;
        private readonly LiteNetLibServerSocket _serverSocket;

        public LiteNetLibConnectionRequest(string authorizationString, LiteNetLib.ConnectionRequest request, LiteNetLibServerSocket serverSocket) : base(authorizationString, request.RemoteEndPoint)
        {
            _request = request;
            _serverSocket = serverSocket;
        }

        public override Connection Accept()
        {
            LLogger.Current.LogTrace(this, $"[Ip:{_request.RemoteEndPoint}] connection request accepted");
            return _serverSocket.CreateConnection(_request.Accept());
        }

        public override void Reject()
        {
            LLogger.Current.LogTrace(this, $"[Ip:{_request.RemoteEndPoint}] connection request rejected");
            _request.Reject();
        }
    }
}