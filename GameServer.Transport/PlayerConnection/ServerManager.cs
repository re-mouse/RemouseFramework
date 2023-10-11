using System;
using System.Collections.Generic;
using GameServer.Authorization;
using GameServer.Players;
using Shared.DIContainer;
using Shared.Online.Commands;
using Shared.Online.Models;
using Shared.Serialization;
using Shared.Utils.Log;

namespace GameServer.ServerTransport
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
        
        public void Start(int port)
        {
            int maxPlayers = 10;
            
            Logger.Current.LogInfo(this, $"Starting server on port {port}, maxPlayers = {maxPlayers}");
            
            var args = new ServerArgs()
            {
                port = port,
                maxPlayers = maxPlayers
            };
            
            _server.Start(args);
                
            Logger.Current.LogInfo(this, $"Server started");
        }

        public void Stop()
        {
            Logger.Current.LogInfo(this, $"Server stop requested");
            
            _server.Stop();

            Logger.Current.LogInfo(this, $"Server stopped");
        }

        public void Update()
        {
            _server.Update();
        }

        void IServerSocketEventsHandler.HandleConnectionRequest(ConnectionRequest request)
        {
            Logger.Current.LogInfo(this, $"Received connection request from {request.endPoint}");
            
            var credentials = _authorizer.Authorize(request.data, out var result);

            if (result == AuthorizeResult.Successful)
            {
                Logger.Current.LogInfo(this, $"Authorization successful: Name: {credentials.name}, Id: {credentials.playerId}");

                var connection = request.Accept();
                
                var player = new PlayerData()
                {
                    sessionData = new PlayerSessionData(),
                    saveData = new PlayerSaveData()
                    {
                        id = credentials.playerId, 
                        name = credentials.name, 
                        worldName = "Top"
                    }
                };
                
                Logger.Current.LogInfo(this, $"{request.endPoint} connection request accept");

                _playersByConnection[connection] = new PlayerConnection(player, connection);
            }
            else
            {
                Logger.Current.LogInfo(this, "Authorization unsuccessful");

                request.Reject();

                Logger.Current.LogInfo(this, $"{request.endPoint} connection request reject");
            }
        }

        void IServerSocketEventsHandler.HandleConnected(Connection connection)
        {
            Logger.Current.LogInfo(this, $"Connected: {connection.endPoint}");
            
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
                    Logger.Current.LogWarning(this, $"Received invalid network message from {connection.endPoint}");
                    
                    return;
                }
                
                Logger.Current.LogInfo(this, $"Succesfuly deserialized from {connection.endPoint}, message - {message}");
                
                _playersDataHandler.HandleMessage(player, message);
            }
            catch (Exception exception)
            {
                Logger.Current.LogException(this, exception, $"Received exception on deserialization: {connection.endPoint}");
            }
        }

        void IServerSocketEventsHandler.HandleDisconnected(Connection connection)
        {
            Logger.Current.LogInfo(this, $"Disconnected: {connection.endPoint}");

            if (_playersByConnection.TryGetValue(connection, out var player))
            {
                _playersConnectionHandler.HandleDisconnected(player);
            }

            _playersByConnection.Remove(connection);
        }
    }
}