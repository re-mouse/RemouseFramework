using Infrastructure;
using Remouse.DI;
using Remouse.Network.Server;
using Remouse.Simulation;
using Remouse.Utils;

namespace Remouse.GameServer
{
    public class GameServerLoop
    {
        private ICommandRunner _commandRunner;
        private ISimulationHost _simulationHost;
        private IServerTransport _serverTransport;
        private TickClock _tickClock;
        private SimulationUpdatesController _simulationUpdatesController;
        
        public void Construct(Container container)
        {
            _tickClock = new TickClock(Project.TicksInSecond);
            _simulationHost = container.Resolve<ISimulationHost>();
            _simulationUpdatesController = container.Resolve<SimulationUpdatesController>();
            _commandRunner = container.Resolve<ICommandRunner>();
        }

        internal void Start()
        {
            _tickClock.Start();
        }

        public void Update()
        {
            _serverTransport.Update();
            
            if (_simulationHost.Simulation == null)
            {
                LLogger.Current.LogWarning(this, "Simulation not loaded");
                return;
            }
            
            for (int i = 0; i < _tickClock.CalculateTicksElapsed(); i++)
            {
                _simulationUpdatesController.SendEnqueuedCommands();
                _commandRunner.RunEnqueuedCommands();
                _simulationHost.Simulation.Tick();
            }
        }
    }
}