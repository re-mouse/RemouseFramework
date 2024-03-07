using Infrastructure;
using ReDI;
using Remouse.Network.Client;
using Remouse.Simulation;
using Remouse.Utils;

namespace Remouse.GameClient
{
    public class GameClientSimulationLoop
    {
        [Inject] private ICommandRunner _commandRunner;
        [Inject] private ReceivedWorldCommandsBuffer _receivedWorldCommandsBuffer;
        [Inject] private ISimulationHost _simulationHost;
        [Inject] private IClientTransport _clientTransport;
        
        private TickClock _tickClock;

        internal void Start()
        {
            _tickClock = new TickClock(Project.TicksInSecond); 
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

            _tickClock.SetMillisecondsDelay(_clientTransport.PingInMilliseconds + 100);
            
            for (int i = 0; i < _tickClock.CalculateTicksElapsed(); i++)
            {
                var commands = _receivedWorldCommandsBuffer.GetCommands(_simulationHost.Simulation.CurrentTick);
                _commandRunner.RunCommands(commands);
                
                _simulationHost.Simulation.Tick();
            }
        }
    }
}