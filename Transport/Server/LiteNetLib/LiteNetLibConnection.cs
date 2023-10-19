using LiteNetLib;
using Remouse.Serialization;

namespace Remouse.Transport.Implementations
{
    public class LiteNetLibConnection : Connection
    {
        private readonly NetPeer _peer;

        public LiteNetLibConnection(NetPeer peer) : base(peer.EndPoint)
        {
            _peer = peer;
        }
        
        public override void Send(byte[] data, DeliveryMethod deliveryMethod)
        {
            _peer.Send(data, (LiteNetLib.DeliveryMethod)deliveryMethod);
        }

        public override void Disconnect()
        {
            _peer.Disconnect();
        }
    }
}