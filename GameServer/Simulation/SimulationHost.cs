using System;
using Remouse.Core;

namespace Remouse.GameServer.ServerShards
{
    public class SimulationHost
    {
        public Simulation? Simulation { get; private set; }
        public event Action<Simulation> SimulationChanged; 

        public void SetGameSimulation(Simulation simulation)
        {
            Simulation = simulation;
            SimulationChanged?.Invoke(simulation);
        }
    }
}