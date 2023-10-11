using Shared.Content;
using Shared.DIContainer;
using Shared.GameSimulation.ECS.Systems;

namespace Shared.GameSimulation
{
    public class GameSimulationFactory
    {
        private Database _database;

        public void Construct(Container container)
        {
            _database = container.Get<Database>();
        }
        
        public GameSimulation CreateGameSimulation(string worldName = "Start")
        {
            GameSimulation simulation = new GameSimulation(worldName);
            
            AddSystems(simulation);
            
            simulation.Initialize();

            return simulation;
        }

        private void AddSystems(GameSimulation simulation)
        {
            simulation.AddSystem(new WaypointSystem());
            simulation.AddSystem(new PositionTeleportSystem());
            simulation.AddSystem(new MoveToDirectionSystem());
        }
    }
}