using System.Net;
using LiteNetLib;
using System;
using System.Net.Sockets;
using Remouse.Serialization;

namespace Remouse.Transport
{

    public class LiteNetLibClientSocket : IClientSocket
    {
        private EventBasedNetListener _listener;
        private NetManager _netManager;

        private NetPeer? _connectionToServer;
        
        public event Action Connected;
        public event Action ConnectionFailed;
        public event Action Disconnected;
        public event Action<INetworkReader> DataReceived;

        public LiteNetLibClientSocket()
        {
            _listener = new EventBasedNetListener();
            _netManager = new NetManager(_listener);
            _netManager.DisconnectTimeout = 10000000;
        
            _listener.PeerConnectedEvent += HandleConnect;
            _listener.PeerDisconnectedEvent += HandleDisconnect;
            _listener.NetworkReceiveEvent += HandleReceive;
            _listener.NetworkErrorEvent += HandleNetworkError;
        }

        private void HandleDisconnect(NetPeer peer, DisconnectInfo disconnectinfo)
        {
            _connectionToServer = null;

            Disconnected?.Invoke();
        }

        private void HandleReceive(NetPeer peer, NetPacketReader reader, byte channel, LiteNetLib.DeliveryMethod deliverymethod)
        {
            DataReceived?.Invoke(new LiteNetLibReader(reader));
        }

        private void HandleConnect(NetPeer peer)
        {
            _connectionToServer = peer;
            
            Connected?.Invoke();
        }

        private void HandleNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
        }

        public void Connect(IPEndPoint host)
        {
            if (_netManager.IsRunning)
            {
                return;
            }
            
            _netManager.Start();
            
            _netManager.Connect(host, "SomeConnectionKey");
        }

        public void Disconnect()
        {
            _netManager.DisconnectAll();
        }

        public void Send(ReadOnlySpan<byte> data, DeliveryMethod deliveryMethod)
        {
            _connectionToServer?.Send(data, (LiteNetLib.DeliveryMethod)deliveryMethod);
        }
        
        public void Update()
        {
            _netManager.PollEvents();
        }

        public void Dispose()
        {
            _netManager.Stop();
        }
    }
}