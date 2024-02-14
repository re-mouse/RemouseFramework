using Remouse.Simulation;
using Remouse.DI;
using Remouse.Network.Client;
using Remouse.Simulation.Network;
using Remouse.Utils;

namespace Remouse.GameClient
{
    public class PackedSimulationListener
    {
        private IClientMessageBus _clientMessageBus;
        private ISimulationLoader _simulationLoader;
        private NetworkEntityRegistry _networkEntityRegistry;
        private GameClientSimulationLoop _simulationLoop;

        public void Construct(Container container)
        {
            _clientMessageBus = container.Resolve<IClientMessageBus>();
            _simulationLoop = container.Resolve<GameClientSimulationLoop>();
            _simulationLoader = container.Resolve<ISimulationLoader>();
            _networkEntityRegistry = container.Resolve<NetworkEntityRegistry>();
            _clientMessageBus.SubscribeToMessage<SimulationStateMessage>(HandlePackedSimulationMessage);
        }

        private void HandlePackedSimulationMessage(SimulationStateMessage message)
        {
            _simulationLoader.LoadSerialized(message.WorldState, message.Tick);
            _networkEntityRegistry.SetLastUsedNetworkId(message.LastNetworkId);
            _simulationLoop.Start();
            
            LLogger.Current.LogTrace(this, $"Setted and initialized simulation on host from packed simulation [StartTick:{message.Tick}]");
        }
    }
}