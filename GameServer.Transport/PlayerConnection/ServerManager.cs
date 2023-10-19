using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Remouse.GameServer.Authorization;
using Remouse.GameServer.Players;
using Remouse.Shared.DIContainer;
using Remouse.Shared.Models;
using Remouse.Shared.Models.Messages;
using Remouse.Shared.Serialization;
using Remouse.Shared.Utils.Log;

namespace Remouse.GameServer.ServerTransport
{
    internal class ServerManager : IServerSocketEventsHandler, IServer
    {
        private IServerSocket _server;
        
        private IAuthorizer _authorizer;
        
        private IPlayersConnectionHandler _playersConnectionHandler;
        private IPlayersDataHandler _playersDataHandler;
        
        private readonly INetworkWriter _writer = new NetworkBytesWriter();

        private Dictionary<Connection, PlayerConnection> _playersByConnection = new Dictionary<Connection, PlayerConnection>();

        public void Construct(Container mediator)
        {
            _server = mediator.Get<IServerSocket>();
            _authorizer = mediator.Get<IAuthorizer>();
            _playersConnectionHandler = mediator.Get<IPlayersConnectionHandler>();
            _playersDataHandler = mediator.Get<IPlayersDataHandler>();
            
            _server.SetHandler(this);
        }
        
        public void Start(ushort port)
        {
            Logger.Current.LogInfo(this, $"Starting server on port {port}");
            
            _server.Start(port);
                
            Logger.Current.LogInfo(this, "Server started");
        }

        public void Stop()
        {
            Logger.Current.LogInfo(this, "Server stop requested");
            
            _server.Stop();

            Logger.Current.LogInfo(this, "Server stopped");
        }

        public void Update()
        {
            _server.Update();
        }

        void IServerSocketEventsHandler.HandleConnectionRequest(ConnectionRequest request)
        {
            Logger.Current.LogInfo(this, $"Received connection request from {request.EndPoint}");

            var authRequest = new AuthorizationRequest();
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

                _playersByConnection[connection] = new PlayerConnection(player, connection);
            }
            else
            {
                Logger.Current.LogInfo(this, "Authorization unsuccessful");

                request.Reject();

                Logger.Current.LogInfo(this, $"{request.EndPoint} connection request reject");
            }
        }

        void IServerSocketEventsHandler.HandleConnected(Connection connection)
        {
            Logger.Current.LogInfo(this, $"Connected: {connection.EndPoint}");
            
            var player = _playersByConnection[connection];
            _playersConnectionHandler.HandleConnected(player);
        }

        void IServerSocketEventsHandler.HandleData(Connection connection, INetworkReader data)
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
                
                _playersDataHandler.HandleMessage(player, message);
            }
            catch (Exception exception)
            {
                Logger.Current.LogException(this, exception, $"Received exception on deserialization: {connection.EndPoint}");
            }
        }

        void IServerSocketEventsHandler.HandleDisconnected(Connection connection)
        {
            Logger.Current.LogInfo(this, $"Disconnected: {connection.EndPoint}");

            if (_playersByConnection.TryGetValue(connection, out var player))
            {
                _playersConnectionHandler.HandleDisconnected(player);
            }

            _playersByConnection.Remove(connection);
        }
    }
}