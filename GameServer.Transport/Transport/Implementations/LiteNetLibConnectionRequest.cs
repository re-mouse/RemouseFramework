namespace Remouse.GameServer.ServerTransport.Implementations
{
    internal class LiteNetLibConnectionRequest : ConnectionRequest
    {
        private readonly LiteNetLib.ConnectionRequest _request;
        private readonly LiteNetLibServer _server;

        public LiteNetLibConnectionRequest(LiteNetLib.ConnectionRequest request, LiteNetLibServer server) : base(request.Data.RawData, request.RemoteEndPoint)
        {
            _request = request;
            _server = server;
        }

        public override Connection Accept()
        {
            return _server.CreateConnection(_request.Accept());
        }

        public override void Reject()
        {
            _request.Reject();
        }
    }
}