#if LITENETLIB

using System.Net;
using LiteNetLib;
using System;
using System.Net.Sockets;
using Shared.Online;

namespace GameClient
{

    public class LiteNetLibClientSocket : ClientSocket
    {
        private EventBasedNetListener _listener;
        private NetManager _netManager;

        private ILogger _logger;

        private NetPeer? _connectionToServer;

        public LiteNetLibClientSocket()
        {
            Singleton.ProjectContextMediator.Get(out _logger);
            
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
            Logger.Current.LogTrace(this, $"Socket disconnected - {disconnectinfo.Reason} {disconnectinfo.SocketErrorCode}");
            _connectionToServer = null;
            
            _eventsHandler?.HandleDisconnected();
        }

        private void HandleReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliverymethod)
        {
            _eventsHandler?.HandleData(new LiteNetLibReader(reader));
        }

        private void HandleConnect(NetPeer peer)
        {
            _connectionToServer = peer;
            
            _eventsHandler?.HandleConnected();
        }

        private void HandleNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Logger.Current.LogError(this, $"Network error: {socketError}");
        }

        public override void Connect(IPEndPoint host)
        {
            if (_netManager.IsRunning)
            {
                Logger.Current.LogError(this, "Unable to connect: already started");
                return;
            }
            
            _netManager.Start();
            
            _netManager.Connect(host, "SomeConnectionKey");
        }

        public override void Disconnect()
        {
            _netManager.Stop();
        }

        public override void Send(ReadOnlySpan<byte> data, Transport.DeliveryMethod deliveryMethod)
        {
            _connectionToServer?.Send(data, (DeliveryMethod)deliveryMethod);
        }
        
        public override void Update()
        {
            _netManager.PollEvents();
        }
    }
}

#endif