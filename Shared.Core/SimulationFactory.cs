using Remouse.Database;
using Remouse.Shared.Core.ECS.Components;
using Remouse.Shared.Core.ECS.Systems;
using Remouse.Shared.Core.World;
using Remouse.DIContainer;

namespace Remouse.Shared.Core
{
    public class SimulationFactory
    {
        private Database _database;
        private IWorldBuilder _worldBuilder;

        public void Construct(Container container)
        {
            _database = container.Resolve<Database>();
            _worldBuilder = container.Resolve<IWorldBuilder>();
            
            RegisterTypesInBuilder();
        }
        
        public Simulation CreateGameSimulation(string worldName = "Start")
        {
            var world = _worldBuilder.Build();
            Simulation simulation = new Simulation(world);
            
            simulation.Initialize();

            return simulation;
        }

        private void RegisterTypesInBuilder()
        {
            _worldBuilder.RegisterSystem<WaypointSystem>();
            _worldBuilder.RegisterSystem<PositionTeleportSystem>();
            _worldBuilder.RegisterSystem<MoveToDirectionSystem>();
            
            _worldBuilder.RegisterComponentType<MovementDirection>();
            _worldBuilder.RegisterComponentType<MovementSpeed>();
            _worldBuilder.RegisterComponentType<NetworkIdentity>();
            _worldBuilder.RegisterComponentType<PlayerTag>();
            _worldBuilder.RegisterComponentType<PositionTeleport>();
            _worldBuilder.RegisterComponentType<Transform>();
            _worldBuilder.RegisterComponentType<WaypointPath>();
        }
    }
}