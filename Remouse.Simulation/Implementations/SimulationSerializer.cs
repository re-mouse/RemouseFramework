using System.Collections.Generic;
using Remouse.World;
using Models;
using Remouse.DI;

namespace Remouse.Simulation
{
    public class SimulationSerializer : ISimulationSerializer
    {
        private ISimulationHost _simulationHost;

        public void Construct(Container container)
        {
            _simulationHost = container.Resolve<ISimulationHost>();
        }
        public WorldState Serialize()
        {
            var world = _simulationHost.Simulation.World;
            var entities = world.GetEntities();
            WorldState packedWorldState = new WorldState();

            foreach (var entityId in entities)
            {
                packedWorldState.entities.Add(SerializeEntity(world, entityId));
            }

            return packedWorldState;
        }
        
        public static EntityState SerializeEntity(IReadOnlyWorld world, int entityId)
        {
            var entityInfo = new EntityState();
            entityInfo.components = new List<SerializedComponent>();
            
            foreach (var componentType in world.ComponentTypes)
            {
                var component = world.GetComponent(componentType, entityId) as IBytesSerializableComponent;
                if (component == null)
                    continue;
                
                entityInfo.components.Add(new SerializedComponent(component));
            }
            
            return entityInfo;
        }
    }
}