using Cysharp.Threading.Tasks;
using Remouse.Simulation;
using ReDI;
using Remouse.Network.Client;
using Remouse.Simulation.Network;
using Remouse.Utils;

namespace Remouse.GameClient
{
    public class PackedSimulationListener
    {
        [Inject] private ISimulationLoader _simulationLoader;
        [Inject] private NetworkEntityRegistry _networkEntityRegistry;
        [Inject] private GameClientSimulationLoop _simulationLoop;

        [Inject]
        private void SubscribeToEvents(IClientMessageBus messageBus)
        {
            messageBus.SubscribeToMessage<SimulationStateMessage>(HandlePackedSimulationMessage);
        }

        private async void HandlePackedSimulationMessage(SimulationStateMessage message)
        {
            _simulationLoader.LoadSerialized(message.WorldState, message.Tick);
            _networkEntityRegistry.SetLastUsedNetworkId(message.LastNetworkId);
            await UniTask.WaitForSeconds(0.1f);
            _simulationLoop.Start();
            
            LLogger.Current.LogTrace(this, $"Setted and initialized simulation on host from packed simulation [StartTick:{message.Tick}]");
        }
    }
}