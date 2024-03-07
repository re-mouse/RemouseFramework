using System;
using Remouse.World;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    public interface ISimulation
    {
        public EcsWorld World { get; }
        public long CurrentTick { get; }
        public void Tick();
    }
}