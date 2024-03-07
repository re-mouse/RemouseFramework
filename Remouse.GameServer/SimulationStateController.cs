using Remouse.Simulation;
using ReDI;
using Remouse.Network.Server;
using Remouse.Simulation.Network;
using Remouse.Utils;

namespace Remouse.GameServer
{
    internal class SimulationStateController
    {
        [Inject] private ISimulationHost _simulationHost;
        [Inject] private NetworkEntityRegistry _networkEntityRegistry;
        [Inject] private ISimulationSerializer _simulationSerializer;

        [Inject]
        private void SubscribeOnEvents(IServerMessageBus messageBus)
        {
            messageBus.SubscribeToMessage<SimulationStateRequestMessage>(HandlePackedSimulationRequest);
        }
        
        private void HandlePackedSimulationRequest(IPlayer player, SimulationStateRequestMessage message)
        {
            if (_simulationHost.Simulation == null)
            {
                LLogger.Current.LogError(this, "Simulation not found");
                return;
            }
            
            SendPackedSimulation(player);
            
            LLogger.Current.LogPlayerTrace(this, player, "Sended packed simulation");
        }
        
        public void SendPackedSimulation(IPlayer player)
        {
            var worldState = _simulationSerializer.Serialize();
            player.Send(new SimulationStateMessage(worldState, _simulationHost.Simulation.CurrentTick, _networkEntityRegistry.LastUsedNetworkId));
        }
    }
}