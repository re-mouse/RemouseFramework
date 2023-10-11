using System.Collections.Generic;
using LiteNetLib;

namespace GameServer.ServerTransport.Implementations
{
    internal class LiteNetLibServer : IServerSocket
    {
        private IServerSocketEventsHandler _eventsHandler;
        private NetManager _netManager;
        private ServerArgs _args;

        private Dictionary<NetPeer, Connection> _connections = new Dictionary<NetPeer, Connection>();

        public LiteNetLibServer()
        {
            var listener = new EventBasedNetListener();
            _netManager = new NetManager(listener);
            _netManager.DisconnectTimeout = 10000000;

            listener.ConnectionRequestEvent += HandleConnectionRequest;

            listener.PeerConnectedEvent += HandlePeerConnected;
            
            listener.PeerDisconnectedEvent += HandleDisconnected;
            
            listener.NetworkReceiveEvent += HandleNetworkReceive;
        }
        
        void IServerSocket.Start(ServerArgs args)
        {
            _args = args;
            
            _netManager.Start(args.port);
        }

        void IServerSocket.SetHandler(IServerSocketEventsHandler handler)
        {
            _eventsHandler = handler;
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
            _eventsHandler?.HandleConnectionRequest(new LiteNetLibConnectionRequest(request, this));
        }
        
        private void HandlePeerConnected(NetPeer peer)
        {
            if (!_connections.ContainsKey(peer))
                return;

            var connection = _connections[peer];
            
            _eventsHandler?.HandleConnected(connection);
        }
        
        private void HandleDisconnected(NetPeer peer, DisconnectInfo disconnectinfo)
        {
            if (!_connections.ContainsKey(peer))
                return;

            var connection = _connections[peer];
            
            _eventsHandler?.HandleDisconnected(connection);
            
            _connections.Remove(peer);
        }
        
        private void HandleNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, LiteNetLib.DeliveryMethod deliverymethod)
        {
            var connection = _connections[peer];

            _eventsHandler?.HandleData(connection, new LiteNetLibReader(reader));
        }
    }
}