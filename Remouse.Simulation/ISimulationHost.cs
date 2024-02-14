using System;

namespace Remouse.Simulation
{
    public interface ISimulationHost
    {
        public ISimulation Simulation { get; }
        public event Action<ISimulation> SimulationChanged;
        public event Action<BaseWorldCommand> WorldCommandRunned;
        public event Action TickRunned;
        public void ClearSimulation();
    }
}