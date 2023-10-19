using LiteNetLib;
using Remouse.Shared.Serialization;

namespace Remouse.GameServer.ServerTransport.Implementations
{
    internal class LiteNetLibConnection : Connection
    {
        private readonly NetPeer _peer;

        public LiteNetLibConnection(NetPeer peer) : base(peer.EndPoint)
        {
            _peer = peer;
        }
        
        public override void Send(INetworkWriter data, DeliveryMethod deliveryMethod)
        {
            _peer.Send(data.GetBytes().ToArray(), (LiteNetLib.DeliveryMethod)deliveryMethod);
        }

        public override void Disconnect()
        {
            _peer.Disconnect();
        }
    }
}