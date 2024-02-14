using Remouse.World;
using Remouse.DI;

namespace Remouse.Simulation
{
    internal class SimulationLoader : ISimulationLoader
    {
        private IWorldBuilder _worldBuilder;
        private WorldMapLoader _worldMapLoader;
        private SimulationHost _simulationHost;
        private Container _container;

        public void Construct(Container container)
        {
            _container = container;
            _worldBuilder = container.Resolve<IWorldBuilder>();
            _worldMapLoader = container.Resolve<WorldMapLoader>();
            _simulationHost = container.Resolve<SimulationHost>();
        }

        public void LoadEmpty(long startTick = 0)
        {
            var world = _worldBuilder.Build();
            var simulation = new Simulation(world, _container, startTick);
            simulation.Initialize();
            _simulationHost.SetSimulation(simulation);
        }

        public void LoadMap(string mapId, long startTick = 0)
        {
            var world  = _worldMapLoader.Load(mapId);
            
            var simulation = new Simulation(world, _container, startTick);
            simulation.Initialize();
            _simulationHost.SetSimulation(simulation);
        }

        public void LoadSerialized(WorldState packedWorldState, long startTick = 0)
        {
            var world = _worldBuilder.Build();
            Deserialize(world, packedWorldState);

            var simulation = new Simulation(world, _container, startTick);
            simulation.Initialize();
            _simulationHost.SetSimulation(simulation);
        }
        
        private void Deserialize(IWorld world, WorldState serializedWorldState)
        {
            foreach (var entityInfo in serializedWorldState.entities)
            {
                DeserializeEntity(world, entityInfo);
            }
        }

        private void DeserializeEntity(IWorld world, EntityState serializedEntityState)
        {
            int entity = world.NewEntity();
            foreach (var componentContainer in serializedEntityState.components)
            {
                world.SetComponent(componentContainer.component.GetType(), entity, componentContainer.component);
            }
        }
    }
}