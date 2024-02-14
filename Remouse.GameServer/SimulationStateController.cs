using Remouse.Simulation;
using Remouse.DI;
using Remouse.Network.Server;
using Remouse.Simulation.Network;
using Remouse.Utils;

namespace Remouse.GameServer
{
    internal class SimulationStateController
    {
        private ISimulationHost _simulationHost;
        private NetworkEntityRegistry _networkEntityRegistry;
        private IServerMessageBus _serverMessageBus;
        private ISimulationSerializer _simulationSerializer;

        public void Construct(Container container)
        {
            _simulationSerializer = container.Resolve<ISimulationSerializer>();
            _networkEntityRegistry = container.Resolve<NetworkEntityRegistry>();
            _simulationHost = container.Resolve<ISimulationHost>();
            _serverMessageBus = container.Resolve<IServerMessageBus>();
            
            _serverMessageBus.SubscribeToMessage<SimulationStateRequestMessage>(HandlePackedSimulationRequest);
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