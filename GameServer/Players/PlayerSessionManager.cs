using Remouse.GameServer.ServerShards;
using Remouse.GameServer.ServerTransport;
using Remouse.Core;
using Remouse.Core.Input;
using Remouse.DIContainer;
using Remouse.Shared.GameSimulation.Commands;
using Remouse.Models;
using Remouse.Models.Messages;
using Remouse.Shared.Utils.Log;

namespace Remouse.GameServer.Players
{
    public class PlayersSessionManager
    {
        private SimulationHost _simulationHost;
        private WorldCommandBuffer _commandBuffer;
        private IPlayerEvents _playerEvents;

        public void Construct(Container container)
        {
            _simulationHost = container.Resolve<SimulationHost>();
            _playerEvents = container.Resolve<IPlayerEvents>();
            _commandBuffer = container.Resolve<WorldCommandBuffer>();
            
            _playerEvents.Connected += HandleConnected;
            _playerEvents.Disconnected += HandleDisconnected;
            _playerEvents.SubscribeToMessage<PlayerInputMessage>(HandleInputMessage);
            _playerEvents.SubscribeToMessage<SimulationLoadedMessage>(HandleSimulationLoadedMessage);
        }

        private void HandleSimulationLoadedMessage(IPlayer player, SimulationLoadedMessage message)
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

        private void SendCurrentWorldState(IPlayer player)
        {
            var worldState = WorldStatePacker.Pack(_simulationHost.Simulation.World);
            player.Send(new CurrentWorldStateMessage(worldState));
        }

        private void EnqueueSpawnPlayer(IPlayer player)
        {
            var command = new SpawnPlayerCommand();
            command.playerId = player.Data.cloudData.id;
            _commandBuffer.Enqueue(command);
        }

        private void HandleInputMessage(IPlayer player, PlayerInputMessage playerInput)
        {
            if (player.Data.sessionData.state != PlayerState.InGame)
            {
                Logger.Current.LogPlayerWarning(this, player, "Player sent input, while not in game");
            }

            var command = playerInput.AsCommand(player.Data);
            _commandBuffer.Enqueue(command);
        
            Logger.Current.LogPlayerTrace(this, player, $"Added message {playerInput} to buffer");
        }

        private void HandleConnected(IPlayer player)
        { 
            player.Send(new LoadMapMessage(_simulationHost.Simulation.MapId));
            
            player.Data.sessionData.state = PlayerState.LoadingMap;
            
            Logger.Current.LogPlayerInfo(this, player, "Connected");
        }

        private void HandleDisconnected(IPlayer player)
        {
            if (player.Data.sessionData.state == PlayerState.InGame)
            {
                _commandBuffer.Enqueue(new DestroyPlayerCommand() { playerId = player.Data.cloudData.id });
            }
            
            player.Data.sessionData.state = PlayerState.Disconnected;
            
            Logger.Current.LogPlayerInfo(this, player, $"Disconnected. State {player.Data.sessionData.state}");
        }
    }
}