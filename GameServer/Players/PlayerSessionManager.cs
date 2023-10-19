using System.Collections.Generic;
using Remouse.GameServer.ServerShards;
using Remouse.GameServer.ServerTransport;
using Remouse.Shared.Core;
using Remouse.Shared.Core.ECS.Utils;
using Remouse.Shared.Core.Input;
using Remouse.Shared.DIContainer;
using Remouse.Shared.GameSimulation.Commands;
using Remouse.Shared.Models;
using Remouse.Shared.Models.Messages;
using Remouse.Shared.Utils.Log;

namespace Remouse.GameServer.Players
{
    public class PlayersSessionManager : IPlayersProvider
    {
        private SimulationHost _simulationHost;
        private WorldCommandBuffer _commandBuffer;
        private IServerEvents _serverEvents;

        private HashSet<IPlayerConnection> _players = new();

        public IEnumerable<IPlayerConnection> Players { get => _players; }

        public void Construct(Container container)
        {
            container.Get(out _simulationHost);
            container.Get(out _serverEvents);
            container.Get(out _commandBuffer);
            
            _serverEvents.Connected += HandleConnected;
            _serverEvents.Disconnected += HandleDisconnected;
            _serverEvents.SubscribeToMessage<PlayerInputMessage>(HandleInputMessage);
            _serverEvents.SubscribeToMessage<SimulationLoadedMessage>(HandleSimulationLoadedMessage);
        }

        private void HandleSimulationLoadedMessage(IPlayerConnection player, SimulationLoadedMessage message)
        {
            if (player.Data.sessionData.state != PlayerState.LoadingMap)
            {
                Logger.Current.LogPlayerError(this, player, $"Received loaded simulation message, incorrect player state. Current state - {player.Data.sessionData.state}");
                return;
            }
            
            SendCurrentWorldState(player);
            
            EnqueueSpawnPlayer(player);

            player.Data.sessionData.state = PlayerState.InGame;
            
            Logger.Current.LogPlayerInfo(this, player, "Spawned player");
        }

        private void SendCurrentWorldState(IPlayerConnection player)
        {
            var worldState = WorldStatePacker.Pack(_simulationHost.Simulation.World);
            player.Send(new SetWorldStateMessage(worldState));
        }

        private void EnqueueSpawnPlayer(IPlayerConnection player)
        {
            var command = new SpawnPlayerCommand();
            command.playerId = player.Data.cloudData.id;
            _commandBuffer.Enqueue(command);
        }

        private void HandleInputMessage(IPlayerConnection player, PlayerInputMessage playerInput)
        {
            if (player.Data.sessionData.state != PlayerState.InGame)
            {
                Logger.Current.LogPlayerWarning(this, player, "Player sent input, while not in game");
            }

            var command = playerInput.AsCommand(player.Data);
            _commandBuffer.Enqueue(command);
        
            Logger.Current.LogPlayerTrace(this, player, $"Added message {playerInput} to buffer");
        }

        private void HandleConnected(IPlayerConnection player)
        { 
            player.Send(new LoadMapMessage(_simulationHost.Simulation.MapId));
            
            player.Data.sessionData.state = PlayerState.LoadingMap;

            _players.Add(player);
            
            Logger.Current.LogPlayerInfo(this, player, "Connected");
        }

        private void HandleDisconnected(IPlayerConnection player)
        {
            if (player.Data.sessionData.state == PlayerState.InGame)
            {
                _commandBuffer.Enqueue(new DestroyPlayerCommand() { playerId = player.Data.cloudData.id });
            }
            
            player.Data.sessionData.state = PlayerState.Disconnected;

            _players.Remove(player);
            
            Logger.Current.LogPlayerInfo(this, player, $"Disconnected. State {player.Data.sessionData.state}");
        }
    }
}