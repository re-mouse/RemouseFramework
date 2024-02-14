using Infrastructure;
using Remouse.DI;
using Remouse.Network.Client;
using Remouse.Simulation;
using Remouse.Utils;

namespace Remouse.GameClient
{
    public class GameClientSimulationLoop
    {
        private ICommandRunner _commandRunner;
        private ReceivedWorldCommandsBuffer _receivedWorldCommandsBuffer;
        private ISimulationHost _simulationHost;
        private IClientTransport _clientTransport;
        private TickClock _tickClock;

        public void Construct(Container container)
        {
            _tickClock = new TickClock(Project.TicksInSecond);
            _receivedWorldCommandsBuffer = container.Resolve<ReceivedWorldCommandsBuffer>();
            _simulationHost = container.Resolve<ISimulationHost>();
            _commandRunner = container.Resolve<ICommandRunner>();
            _clientTransport = container.Resolve<IClientTransport>();
        }

        internal void Start()
        {
            _tickClock.Start();
        }

        public void Update()
        {
            _clientTransport.Update();
            
            if (_simulationHost.Simulation == null)
            {
                LLogger.Current.LogWarning(this , "Simulation not loaded");
                return;
            }
            
            for (int i = 0; i < _tickClock.CalculateTicksElapsed(); i++)
            {
                var commands = _receivedWorldCommandsBuffer.GetCommands(_simulationHost.Simulation.CurrentTick);
                _commandRunner.RunCommands(commands);
                
                _simulationHost.Simulation.Tick();
            }
        }
    }
}