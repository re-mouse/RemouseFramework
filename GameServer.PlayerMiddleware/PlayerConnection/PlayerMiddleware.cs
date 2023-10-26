using System;
using System.Collections.Generic;
using Remouse.GameServer.Authorization;
using Remouse.DIContainer;
using Remouse.GameServer.Players;
using Remouse.Models;
using Remouse.Models.Messages;
using Remouse.Serialization;
using Remouse.Shared.Utils.Log;
using Remouse.Transport;

namespace Remouse.GameServer.ServerTransport
{
    internal class PlayerMiddleware : IPlayerServer
    {
        private IServerSocket _socket;
        private IAuthorizer _authorizer;
        private PlayerEventsBus _eventsBus;
        
        private Dictionary<Connection, Player> _playersByConnection = new Dictionary<Connection, Player>();
        
        IEnumerable<IPlayer> IConnectedPlayers.Players { get => _playersByConnection.Values; }

        public void Construct(Container mediator)
        {
            _socket = mediator.Resolve<IServerSocket>();
            _authorizer = mediator.Resolve<IAuthorizer>();
            _eventsBus = mediator.Resolve<PlayerEventsBus>();

            _socket.DataReceived += HandleData;
            _socket.Connected += HandleConnected;
            _socket.Disconnected += HandleDisconnected;
            _socket.GotConnectionRequest += HandleConnectionRequest;
        }
        
        public void Start(ushort port, int maxPlayers)
        {
            Logger.Current.LogInfo(this, $"Starting server on port {port}");
            
            _socket.Start(port, maxPlayers);
                
            Logger.Current.LogInfo(this, "Server started");
        }

        public void Stop()
        {
            Logger.Current.LogInfo(this, "Server stop requested");
            
            _socket.Stop();

            Logger.Current.LogInfo(this, "Server stopped");
        }

        public void Update()
        {
            _socket.Update();
        }

        void HandleConnectionRequest(ConnectionRequest request)
        {
            Logger.Current.LogInfo(this, $"Received connection request from {request.EndPoint}");

            var authRequest = new AuthenticationCredentials();
            var reader = new NetworkBytesReader(request.Data);
            
            try
            {
                authRequest.Deserialize(reader);
            }
            catch (Exception _)
            {
                request.Reject();
                return;
            }
            
            var credentials = _authorizer.Authorize(authRequest, out var result);

            if (result == AuthorizeResult.Successful)
            {
                Logger.Current.LogInfo(this, $"Authorization successful: Name: {credentials.name}, Id: {credentials.playerId}");

                var connection = request.Accept();
                
                var player = new PlayerData
                {
                    sessionData = new PlayerSessionData(),
                    cloudData = new PlayerCloudData
                    {
                        id = credentials.playerId, 
                        name = credentials.name, 
                    }
                };
                
                Logger.Current.LogInfo(this, $"{request.EndPoint} connection request accept");

                _playersByConnection[connection] = new Player(player, connection);
            }
            else
            {
                Logger.Current.LogInfo(this, "Authorization unsuccessful");

                request.Reject();

                Logger.Current.LogInfo(this, $"{request.EndPoint} connection request reject");
            }
        }

        void HandleConnected(Connection connection)
        {
            Logger.Current.LogInfo(this, $"Connected: {connection.EndPoint}");
            
            var player = _playersByConnection[connection];
            
            _eventsBus.RaiseConnected(player);
        }

        void HandleData(Connection connection, INetworkReader data)
        {
            try
            {
                var player = _playersByConnection[connection];
                
                var message = NetworkMessageSerializer.Deserialize(data);

                if (message == null)
                {
                    Logger.Current.LogWarning(this, $"Received invalid network message from {connection.EndPoint}");
                    
                    return;
                }
                
                Logger.Current.LogInfo(this, $"Succesfuly deserialized from {connection.EndPoint}, message - {message}");
                
                _eventsBus.RaiseMessage(player, message);
            }
            catch (Exception exception)
            {
                Logger.Current.LogException(this, exception, $"Received exception on deserialization: {connection.EndPoint}");
            }
        }

        void HandleDisconnected(Connection connection)
        {
            Logger.Current.LogInfo(this, $"Disconnected: {connection.EndPoint}");

            if (_playersByConnection.TryGetValue(connection, out var player))
            {
                _eventsBus.RaiseDisconnected(player);
            }

            _playersByConnection.Remove(connection);
        }

    }
}