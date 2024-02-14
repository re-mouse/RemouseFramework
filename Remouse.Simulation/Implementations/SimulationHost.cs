using System;

namespace Remouse.Simulation
{
    internal class SimulationHost : ISimulationHost
    {
        public ISimulation Simulation { get => _simulation; }
        private Simulation _simulation;
        public event Action<ISimulation> SimulationChanged;
        public event Action<BaseWorldCommand> WorldCommandRunned;
        public event Action TickRunned;

        public void ClearSimulation()
        {
            SetSimulation(null);
        }

        internal void SetSimulation(Simulation simulation)
        {
            if (simulation == _simulation)
                return;
            
            if (_simulation != null)
            {
                _simulation.TickRunned -= HandleTick;
                _simulation.WorldCommandRunned -= HandleWorldCommandRun;
            }
            
            _simulation = simulation;
            
            if (_simulation != null)
            {
                _simulation.TickRunned += HandleTick;
                _simulation.WorldCommandRunned += HandleWorldCommandRun;
            }

            SimulationChanged?.Invoke(simulation);
        }

        internal Simulation GetSimulationInternal()
        {
            return _simulation;
        }

        private void HandleWorldCommandRun(BaseWorldCommand command)
        {
            WorldCommandRunned?.Invoke(command);
        }

        private void HandleTick()
        {
            TickRunned?.Invoke();
        }
    }
}