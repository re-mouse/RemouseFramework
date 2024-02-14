using Remouse.Utils;

namespace Remouse.Network.Sockets.Implementations
{
    public class LiteNetLibConnectionRequest : ConnectionRequest
    {
        private readonly LiteNetLib.ConnectionRequest _request;
        private readonly LiteNetLibServer _server;

        public LiteNetLibConnectionRequest(byte[] data, LiteNetLib.ConnectionRequest request, LiteNetLibServer server) : base(data, request.RemoteEndPoint)
        {
            _request = request;
            _server = server;
        }

        public override Connection Accept()
        {
            LLogger.Current.LogTrace(this, $"[Ip:{_request.RemoteEndPoint}] connection request accepted");
            return _server.CreateConnection(_request.Accept());
        }

        public override void Reject()
        {
            LLogger.Current.LogTrace(this, $"[Ip:{_request.RemoteEndPoint}] connection request rejected");
            _request.Reject();
        }
    }
}