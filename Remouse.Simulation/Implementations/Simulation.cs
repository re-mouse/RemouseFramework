using System;
using Remouse.World;
using Remouse.Database;
using ReDI;
using Remouse.Utils;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    public class Simulation : ISimulation
    {
        private readonly EcsWorld _world;
        private readonly EcsSystems _systems;
        
        public EcsWorld World { get => _world; }
        public long CurrentTick { get; private set; }

        internal event Action<BaseWorldCommand> WorldCommandRunned;
        internal event Action TickRunned;

        private bool _initialized;
        
        internal Simulation(EcsWorld world, EcsSystems systems, long startTick = 0)
        {
            _world = world;
            _systems = systems;
            CurrentTick = startTick;
        }

        internal void Initialize()
        {
            if (_initialized)
            {
                LLogger.Current.LogError(this, "Double initialize on simulation.");
                return;
            }
            
            _systems.Init();
            _initialized = true;
        }

        public void Tick()
        {
            if (!_initialized)
            {
                LLogger.Current.LogError(this, "Trying to tick, while not initialized.");
                return;
            }

            _systems.Run();
                
            CurrentTick++;
                
            TickRunned?.Invoke();
        }

        internal void RunCommand(BaseWorldCommand command)
        {
            if (!_initialized)
            {
                LLogger.Current.LogError(this, "Trying to run command, while not initialized.");
                return;
            }
            
            command.Run(_world);
            WorldCommandRunned?.Invoke(command);
        }
    }
}