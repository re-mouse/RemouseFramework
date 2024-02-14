using System;
using Remouse.World;

namespace Remouse.Simulation
{
    public interface ISimulation
    {
        public IReadOnlyWorld World { get; }
        public long CurrentTick { get; }
        public void Tick();
    }
}