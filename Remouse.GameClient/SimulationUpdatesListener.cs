using Remouse.Simulation;
using Remouse.DI;
using Remouse.Network.Client;
using Remouse.Simulation.Network;
using Remouse.Utils;

namespace Remouse.GameClient
{
    public class SimulationUpdatesListener
    {
        private IClientMessageBus _clientMessageBus;
        private SimulationStateRequester _simulationStateRequester;
        private ReceivedWorldCommandsBuffer _receivedWorldCommandsBuffer;
        private ISimulationHost _simulationHost;

        private bool _isListening;
        
        public void Construct(Container container)
        {
            _simulationHost = container.Resolve<ISimulationHost>();
            _clientMessageBus = container.Resolve<IClientMessageBus>();
            _simulationStateRequester = container.Resolve<SimulationStateRequester>();
            _receivedWorldCommandsBuffer = container.Resolve<ReceivedWorldCommandsBuffer>();
            _clientMessageBus.SubscribeToMessage<SimulationUpdateMessage>(HandleSimulationUpdate);
        }

        public void StartListening()
        {
            _isListening = true;
            
            LLogger.Current.LogInfo(this, "Started listening simulation updates");
        }

        public void StopListening()
        {
            _isListening = false;
            
            LLogger.Current.LogInfo(this, "Stoped listening simulation updates");
        }

        private void HandleSimulationUpdate(SimulationUpdateMessage message)
        {
            if (!_isListening)
            {
                return;
            }
            
            if (_simulationHost.Simulation == null)
            {
                LLogger.Current.LogError(this, "Received simulation update message, while simulation is not loaded");
                return;
            }
            
            var currentTick = _simulationHost.Simulation.CurrentTick;
            if (message.Tick < currentTick)
            {
                LLogger.Current.LogInfo(this, $"Detected outdated update message. Requesting new packed simulation. [Tick:{currentTick}] [MessageTick:{message.Tick}]");
                _simulationStateRequester.RequestPackedSimulation();
                return;
            }

            foreach (var command in message.Commands)
            {
                _receivedWorldCommandsBuffer.Enqueue(message.Tick, command);
            }
            
            LLogger.Current.LogTrace(this, $"Enqueued world commands [WorldCommandsCount:{message.Commands.Count}] [CommandsTick:{message.Tick}] [Tick:{currentTick}]");
        }
    }
}