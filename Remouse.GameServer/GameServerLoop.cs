using Infrastructure;
using ReDI;
using Remouse.Network.Server;
using Remouse.Simulation;
using Remouse.Utils;

namespace Remouse.GameServer
{
    public class GameServerLoop
    {
        [Inject] private ICommandRunner _commandRunner;
        [Inject] private ISimulationHost _simulationHost;
        [Inject] private SimulationUpdatesController _simulationUpdatesController;
        
        private TickClock _tickClock;

        public void Start()
        {
            _tickClock = new TickClock(Project.TicksInSecond);
            _tickClock.Start();
        }

        public void Update()
        {
            if (_simulationHost.Simulation == null)
            {
                LLogger.Current.LogWarning(this, "Simulation not loaded");
                return;
            }
            
            for (int i = 0; i < _tickClock.CalculateTicksElapsed(); i++)
            {
                if (!_commandRunner.IsEmpty())
                {
                    _simulationUpdatesController.SendEnqueuedCommands();
                    _commandRunner.RunEnqueuedCommands();
                }

                _simulationHost.Simulation.Tick();
            }
        }
    }
}