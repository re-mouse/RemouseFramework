using System;
using System.Collections.Generic;
using LiteNetLib;
using Remouse.Serialization;
using Remouse.Utils;

namespace Remouse.Network.Sockets.Implementations
{
    public class LiteNetLibServer : IServerSocket
    {
        private readonly NetManager _netManager;
        private readonly Dictionary<NetPeer, Connection> _connections = new Dictionary<NetPeer, Connection>();
        private int _maxConnections;

        public event Action<ConnectionRequest> GotConnectionRequest;
        public event Action<Connection> Connected;
        public event Action<Connection> Disconnected;
        public event Action<Connection, IBytesReader> DataReceived;

        public LiteNetLibServer()
        {
            var listener = new EventBasedNetListener();
            _netManager = new NetManager(listener);
            _netManager.DisconnectTimeout = 10000;

            listener.ConnectionRequestEvent += HandleConnectionRequest;

            listener.PeerConnectedEvent += HandlePeerConnected;
            
            listener.PeerDisconnectedEvent += HandleDisconnected;
            
            listener.NetworkReceiveEvent += HandleNetworkReceive;
        }
        
        void IServerSocket.Start(ushort port, int maxConnections)
        {
            LLogger.Current.LogTrace(this, $"Start listening [Port:{port}] [MaxConnections:{maxConnections}]");
            _netManager.Start(port);
            _maxConnections = maxConnections;
        }

        void IServerSocket.Stop()
        {
            _netManager.Stop();
        }

        void IServerSocket.PollEvents()
        {
            _netManager.PollEvents();
        }

        public Connection CreateConnection(NetPeer netPeer)
        {
            var connection = new LiteNetLibConnection(netPeer);
            
            _connections[netPeer] = connection;

            return connection;
        }

        private void HandleConnectionRequest(LiteNetLib.ConnectionRequest request)
        {
            if (_connections.Count >= _maxConnections)
            {
                LLogger.Current.LogInfo(this, $"Autoreject - Maximum Connections Reached [Ip:{request.RemoteEndPoint}]");

                request.Reject();
                return;
            }

            var data = new byte[request.Data.AvailableBytes];
            Array.Copy(request.Data.RawData, request.Data.Position, data, 0, request.Data.AvailableBytes);
            GotConnectionRequest?.Invoke(new LiteNetLibConnectionRequest(data, request, this));
        }
        
        private void HandlePeerConnected(NetPeer peer)
        {
            if (!_connections.ContainsKey(peer))
                return;

            LLogger.Current.LogTrace(this, $"Connection connected [Ip:{peer.EndPoint}]");
            
            var connection = _connections[peer];
            
            Connected?.Invoke(connection);
        }
        
        private void HandleDisconnected(NetPeer peer, DisconnectInfo disconnectinfo)
        {
            if (!_connections.ContainsKey(peer))
                return;

            var connection = _connections[peer];
            
            LLogger.Current.LogTrace(this, $"Connection disconnected [Ip:{peer.EndPoint}] [Reason:{disconnectinfo.Reason}] [SocketErrorCode:{disconnectinfo.SocketErrorCode}]");
            
            Disconnected?.Invoke(connection);
            
            _connections.Remove(peer);
        }
        
        private void HandleNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, LiteNetLib.DeliveryMethod deliverymethod)
        {
            LLogger.Current.LogTrace(this, $"Received data [Ip:{peer.EndPoint}] [ByteSize:{reader.AvailableBytes}]");

            var connection = _connections[peer];

            DataReceived?.Invoke(connection, new LiteNetLibReader(reader));
        }

        public void Dispose()
        {
            _netManager.Stop();
        }
    }
}