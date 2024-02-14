using LiteNetLib;
using Remouse.Serialization;
using Remouse.Utils;

namespace Remouse.Network.Sockets.Implementations
{
    public class LiteNetLibConnection : Connection
    {
        private readonly NetPeer _peer;

        public LiteNetLibConnection(NetPeer peer) : base(peer.EndPoint)
        {
            _peer = peer;
        }
        
        public override void Send(IBytesWriter data, DeliveryMethod deliveryMethod)
        {
            var dataArray = data.GetBytes().ToArray();
            _peer.Send(dataArray, (LiteNetLib.DeliveryMethod)deliveryMethod);
            LLogger.Current.LogTrace(this, $"Sended data [Ip:{_peer.EndPoint}] [ByteSize:{dataArray.Length}]");
        }

        public override void Disconnect()
        {
            LLogger.Current.LogTrace(this, $"Disconnect requested [Ip:{_peer.EndPoint}]");
            _peer.Disconnect();
        }
    }
}