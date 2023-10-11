using System;
using Shared.EcsLib;

namespace Shared.GameSimulation
{
    public class GameSimulation : IDisposable
    {
        public static float ticksInSecond { get => 60f; }
        
        private readonly EcsSystems _systems;

        public string MapId { get; }
        public EcsWorld World { get; }
        public long simulationTick { get; private set; }
        
        public GameSimulation(string mapId = "")
        {
            EcsWorld.Config config = new EcsWorld.Config() { };
            World = new EcsWorld(config);
            _systems = new EcsSystems(World);
            
            MapId = mapId;
        }

        public void SetTick(long tick)
        {
            simulationTick = tick;
        }

        public void Initialize()
        {
            _systems.Init(); 
        }

        public void Tick()
        {
            _systems.Run();
            
            simulationTick++;
        }

        public void AddSystem(IEcsSystem system)
        {
            _systems.Add(system);
        }

        public void Dispose()
        {
            World?.Destroy();
        }
    }
}