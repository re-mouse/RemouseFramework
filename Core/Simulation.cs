using System;
using Remouse.Core.World;
using Remouse.DatabaseLib;
using Remouse.Models;
using Remouse.Shared.Utils.Log;

namespace Remouse.Core
{
    public class Simulation
    {
        private readonly IWorld _world;
        
        public static float TicksInSecond { get => 60f; }

        public string MapId { get; }
        public IReadOnlyWorld World { get; }
        public long CurrentTick { get; private set; }

        public event Action<WorldCommand> WorldCommandRunned;

        private bool _initialized = false;
        private Database _database;

        public Simulation(IWorld world)
        {
            _world = world;
        }

        public void Initialize(Database database)
        {
            if (_initialized)
            {
                Logger.Current.LogError(this, "Double initialize on simulation");
                return;
            }
            
            _database = database;
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
            
            command.Run(_world, _database);
            WorldCommandRunned?.Invoke(command);
        }

        public PackedWorld PackWorld()
        {
            return PackedWorld.Pack(World);
        }

        public void UnpackWorld(PackedWorld packedWorld)
        {
            PackedWorld.Unpack(packedWorld, _world);
        }
    }
}