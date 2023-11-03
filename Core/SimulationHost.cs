using System;

namespace Remouse.Core
{
    public class SimulationHost
    {
        public Simulation? Simulation { get; private set; }
        public event Action<Simulation> SimulationChanged; 

        public void Set(Simulation simulation)
        {
            Simulation = simulation;
            SimulationChanged?.Invoke(simulation);
        }
    }
}