using System;
using System.Collections.Generic;
using ReDI;
using Remouse.Network.Models;
using Remouse.Serialization;
using Remouse.Utils;
using Remouse.Network.Sockets;

namespace Remouse.Network.Server
{
    internal class ServerTransport : IServerTransport, IConnectedPlayers
    {
        [Inject] private IServerSocket _socket;
        [Inject] private IAuthorizer _authorizer;
        [Inject] private ServerTransportEvents _transportEvents;
        
        private Dictionary<Connection, Player> _playersByConnection = new Dictionary<Connection, Player>();
        
        IEnumerable<IPlayer> IConnectedPlayers.Players { get => _playersByConnection.Values; }

        [Inject]
        private void SubscribeOnEvents()
        {
            _socket.DataReceived += HandleData;
            _socket.Connected += HandleConnected;
            _socket.Disconnected += HandleDisconnected;
            _socket.GotConnectionRequest += HandleConnectionRequest;
        }
        
        public void Start(ushort port, int maxPlayers)
        {
            LLogger.Current.LogInfo(this, $"Starting server on port {port}");
            
            _socket.Start(port, maxPlayers);
                
            LLogger.Current.LogInfo(this, "Server started");
        }

        public void Stop()
        {
            LLogger.Current.LogInfo(this, "Server stop requested");
            
            _socket.Stop();

            LLogger.Current.LogInfo(this, "Server stopped");
        }

        public void Update()
        {
            _socket.PollEvents();
        }

        private async void HandleConnectionRequest(ConnectionRequest request)
        {
            LLogger.Current.LogInfo(this, $"Connection request [Ip:{request.EndPoint}]");

            var authRequest = new AuthenticationCredentials();
            var reader = new BytesReader(request.Data);
            
            try
            {
                authRequest.Deserialize(reader);
            }
            catch (Exception exception)
            {
                LLogger.Current.LogInfo(this, $"Exception on deserializing received authorization request [Ip:{request.EndPoint} connection request reject]. Message: {exception.Message}");
                request.Reject();
                return;
            }

            AuthorizeResult result;
            try
            {
                result = await _authorizer.Authorize(authRequest);
            }
            catch (Exception exception)
            {
                LLogger.Current.LogException(this, exception, $"Got exception on authorization process [Ip:{request.EndPoint}]");
                request.Reject();
                return;
            }

            if (result.Status != Status.Successful)
            {
                request.Reject();
                LLogger.Current.LogTrace(this, $"Unsuccessful authorization [Ip:{request.EndPoint}]");
                return;
            }
            
            var connection = request.Accept();

            var player = new Player(result.Identity, connection, (int)result.Identity.id % 100);

            LLogger.Current.LogPlayerInfo(this, player, $"Authorization successful");

            _playersByConnection[connection] = player;
        }

        private void HandleConnected(Connection connection)
        {
            var player = _playersByConnection[connection];
            
            LLogger.Current.LogInfo(this, $"Connected: [Ip:{connection.EndPoint}] [PlayerId:{player.Id}]");
            
            _transportEvents.RaiseConnected(player);
        }

        private void HandleData(Connection connection, IBytesReader data)
        {
            var player = _playersByConnection[connection];

            NetworkMessage message;
            try
            {
                message = NetworkSerializeExtensions.ReadNetworkMessage(data);
            }
            catch (Exception exception)
            {
                LLogger.Current.LogPlayerWarning(this, player, $"Got exception on deserialization. ExceptionMessage {exception.Message}");
                return;
            }

            if (message == null)
            {
                LLogger.Current.LogPlayerWarning(this, player, "Received invalid network message");
                return;
            }
            
            LLogger.Current.LogPlayerTrace(this, player, $"Deserialized message [NetworkMessage:{message}]");
            
            _transportEvents.RaiseMessage(player, message);
        }

        private void HandleDisconnected(Connection connection)
        {

            if (_playersByConnection.TryGetValue(connection, out var player))
            {
                LLogger.Current.LogPlayerInfo(this, player, $"Disconnected [Ip:{connection.EndPoint}]");
                _transportEvents.RaiseDisconnected(player);
            }
            else
            {
                LLogger.Current.LogInfo(this, $"Disconnected [Ip:{connection.EndPoint}]");
            }

            _playersByConnection.Remove(connection);
        }

    }
}