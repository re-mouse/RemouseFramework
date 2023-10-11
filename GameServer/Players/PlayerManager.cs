using System;
using System.Collections.Generic;
using GameServer.ServerShards;
using GameServer.ServerTransport;
using Shared.DIContainer;
using Shared.Online.Commands;
using Shared.Online.Commands.Client;
using Shared.Online.Models;
using Shared.Utils.Log;

namespace GameServer.Players
{
    public class PlayerManager
    {
        private GameSimulationHolder _simulationHolder;
        private PlayerInputBuffer _playersInputBuffer;

        private HashSet<IPlayerConnection> _players = new HashSet<IPlayerConnection>();

        public IEnumerable<IPlayerConnection> players => _players;
        
        public void Construct(Container container)
        {
            container.Get(out _simulationHolder);
            
            var serverEvents = container.Get<IServerEvents>();
            serverEvents.Connected += HandleConnected;
            serverEvents.Disconnected += HandleDisconnected;
            serverEvents.SubscribeToMessage<InputNetworkMessage>(HandleInputMessage);
        }

        private void HandleInputMessage(IPlayerConnection player, InputNetworkMessage inputNetworkMessage)
        {
            if (player == null)
            {
                Logger.Current.LogException(this, new NullReferenceException("Player sent command is null"));
                return;
            }

            if (player.data.sessionData.state != PlayerState.Spawned)
            {
                Logger.Current.LogPlayerWarning(this, player, "Player sended input, while not spawned in world");
            }

            if (inputNetworkMessage == null)
            {
                Logger.Current.LogPlayerError(this, player, "Handling command is null");
                return;
            }
            
            inputNetworkMessage.SetPlayer(player.data.sessionData);

            _playersInputBuffer.Enqueue(inputNetworkMessage);
        
            Logger.Current.LogPlayerTrace(this, player, $"Added message {inputNetworkMessage} to buffer");
        }

        private void HandleConnected(IPlayerConnection player)
        { 
            player.Send(new LoadMapMessage(_simulationHolder.simulation.MapId));

            _players.Add(player);
            
            Logger.Current.LogPlayerInfo(this, player, "Connected as Player");
        }

        private void HandleDisconnected(IPlayerConnection player)
        {
            if (player.data.sessionData.playerEntityId != null)
            {
                Logger.Current.LogPlayerInfo(this, player, $"Disconnected, removed player");

                _simulationRunner.DestroyEntity(player.sessionData.playerEntityId.Value);
            }

            _players.Remove(player);
            
            Logger.Current.LogPlayerInfo(this, player, $"Disconnected, player was not spawned");
        }
    }
}