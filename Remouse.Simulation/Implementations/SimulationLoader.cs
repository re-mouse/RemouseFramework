using Remouse.World;
using ReDI;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    internal class SimulationLoader : ISimulationLoader
    {
        [Inject] private EcsSystemsBuilder _systemsBuilder;
        [Inject] private WorldMapLoader _worldMapLoader;
        [Inject] private SimulationHost _simulationHost;

        public void LoadEmpty(long startTick = 0)
        {
            var world = new EcsWorld();
            var systems = _systemsBuilder.Build(world);
            var simulation = new Simulation(world, systems, startTick);
            _simulationHost.SetSimulation(simulation);
            simulation.Initialize();
        }

        public void LoadMap(string mapId, long startTick = 0)
        {
            var world = new EcsWorld();
            var systems = _systemsBuilder.Build(world);
            var simulation = new Simulation(world, systems, startTick);
            _simulationHost.SetSimulation(simulation);
            _worldMapLoader.Load(world, mapId);
            simulation.Initialize();
        }

        public void LoadSerialized(WorldState packedWorldState, long startTick = 0)
        {
            var world = new EcsWorld();
            var systems = _systemsBuilder.Build(world);
            var simulation = new Simulation(world, systems, startTick);
            _simulationHost.SetSimulation(simulation);
            Deserialize(packedWorldState);
            simulation.Initialize();
        }
        
        private void Deserialize(WorldState packedWorldState)
        {
            foreach (var entityInfo in packedWorldState.entities)
            {
                DeserializeEntity(entityInfo);
            }
        }

        private void DeserializeEntity(EntityState serializedEntityState)
        {
            int entity = _simulationHost.Simulation.World.NewEntity();
            foreach (var componentContainer in serializedEntityState.components)
            {
                var pool = _simulationHost.Simulation.World.GetPoolOrCreateByType(componentContainer.component.GetType());
                pool.AddOrSetRaw(entity, componentContainer.component);
            }
        }
    }
}