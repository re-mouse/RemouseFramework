using System;
using Remouse.World;
using Remouse.Database;
using Remouse.DI;
using Remouse.Serialization;
using Remouse.Utils;

namespace Remouse.Simulation
{
    public class Simulation : ISimulation
    {
        private readonly IWorld _world;
        private readonly Container _container;
        
        public IReadOnlyWorld World { get => _world; }
        public long CurrentTick { get; private set; }

        internal event Action<BaseWorldCommand> WorldCommandRunned;
        internal event Action TickRunned;

        private bool _initialized;
        
        internal Simulation(IWorld world, Container container, long startTick = 0)
        {
            _world = world;
            _container = container;
            CurrentTick = startTick;
        }

        internal void Initialize()
        {
            if (_initialized)
            {
                LLogger.Current.LogError(this, "Double initialize on simulation.");
                return;
            }
            
            _world.Initialize();
            _initialized = true;
        }

        public void Tick()
        {
            if (!_initialized)
            {
                LLogger.Current.LogError(this, "Trying to tick, while not initialized.");
                return;
            }

            _world.Tick();
                
            CurrentTick++;
                
            TickRunned?.Invoke();
        }

        public void RunCommand(BaseWorldCommand command)
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