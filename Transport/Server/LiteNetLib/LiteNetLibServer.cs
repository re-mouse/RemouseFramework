using System;
using System.Collections.Generic;
using LiteNetLib;
using Remouse.Serialization;

namespace Remouse.Transport.Implementations
{
    public class LiteNetLibServer : IServerSocket
    {
        private NetManager _netManager;

        private Dictionary<NetPeer, Connection> _connections = new Dictionary<NetPeer, Connection>();
        
        public event Action<ConnectionRequest> GotConnectionRequest;
        public event Action<Connection> Connected;
        public event Action<Connection> Disconnected;
        public event Action<Connection, INetworkReader> DataReceived;

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
            _netManager.Start(port);
        }

        void IServerSocket.Stop()
        {
            _netManager.Stop();
        }

        void IServerSocket.Update()
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
            GotConnectionRequest?.Invoke(new LiteNetLibConnectionRequest(request, this));
        }
        
        private void HandlePeerConnected(NetPeer peer)
        {
            if (!_connections.ContainsKey(peer))
                return;

            var connection = _connections[peer];
            
            Connected?.Invoke(connection);
        }
        
        private void HandleDisconnected(NetPeer peer, DisconnectInfo disconnectinfo)
        {
            if (!_connections.ContainsKey(peer))
                return;

            var connection = _connections[peer];
            
            Disconnected?.Invoke(connection);
            
            _connections.Remove(peer);
        }
        
        private void HandleNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, LiteNetLib.DeliveryMethod deliverymethod)
        {
            var connection = _connections[peer];

            DataReceived?.Invoke(connection, new LiteNetLibReader(reader));
        }

        public void Dispose()
        {
            _netManager.Stop();
        }
    }
}