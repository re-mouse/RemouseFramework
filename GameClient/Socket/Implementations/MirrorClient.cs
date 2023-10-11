#if MIRROR && LITENETLIB

using System;
using System.Net;
using UnityEngine;
using Mirror;
using Shared.Shared.Utils;

namespace GameClient.Transport
{
    public class MirrorClientSocket : ClientSocket
    {
        public MirrorClientSocket()
        {
            NetworkClient.RegisterHandler<ByteMessage>(OnByteReceived);
        }

        private void OnByteReceived(ByteMessage obj)
        {
            _eventsHandler?.HandleData(new NetworkBytesReader(obj.data));
        }

        public override void Connect(IPEndPoint host)
        {
            if (NetworkClient.isConnected)
                return;
            NetworkClient.Connect(host.ToString());
        }

        public override void Disconnect()
        {
            if(!NetworkClient.isConnected)
                return;
            NetworkClient.Disconnect();
        }

        public override void Send(ReadOnlySpan<byte> data, DeliveryMethod deliveryMethod)
        {
            var message = new ByteMessage();
            message.data = data.ToArray();
            NetworkClient.Send(message);
        }

        public override void Update() {  }
    }
    
    public struct ByteMessage : NetworkMessage
    {
        public byte[] data;
    }
}

#endif