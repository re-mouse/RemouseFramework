using Remouse.Shared.Core.World;
using Remouse.Shared.Utils.Log;
using Remouse.Shared.Models;

namespace Remouse.Shared.Core
{
    public class Simulation
    {
        private readonly IWorld _world;
        
        public static float TicksInSecond { get => 60f; }

        public string MapId { get; }
        public IReadOnlyWorld World { get; }
        public long CurrentTick { get; private set; }

        private bool _initialized = false;
        
        public Simulation(IWorld world)
        {
            _world = world;
        }

        public void Initialize()
        {
            if (_initialized)
            {
                Logger.Current.LogError(this, "Double initialize on simulation");
                return;
            }
            
            _world.Initialize();
            _initialized = true;
        }

        public void Tick()
        {
            if (!_initialized)
            {
                Logger.Current.LogError(this, "Trying to tick, while not initialized");
                return;
            }
            
            _world.Tick();
            
            CurrentTick++;
        }

        public void RunCommand(WorldCommand command)
        {
            if (!_initialized)
            {
                Logger.Current.LogError(this, "Trying to run command, while not initialized");
                return;
            }
            
            command.Apply(_world);
        }
    }
}