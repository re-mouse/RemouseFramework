using System.Collections.Generic;
using Remouse.World;
using Models;
using ReDI;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    public class SimulationSerializer : ISimulationSerializer
    {
        [Inject] private ISimulationHost _simulationHost;

        public WorldState Serialize()
        {
            var world = _simulationHost.Simulation.World;
            int[] entities = null;
            world.GetAllEntities(ref entities);
            WorldState packedWorldState = new WorldState();

            foreach (var entityId in entities)
            {
                packedWorldState.entities.Add(SerializeEntity(world, entityId));
            }

            return packedWorldState;
        }
        
        public static EntityState SerializeEntity(EcsWorld world, int entityId)
        {
            var entityInfo = new EntityState();
            entityInfo.components = new List<SerializedComponent>();

            IEcsPool[] pools = null;
            world.GetAllPools(ref pools);
            foreach (var pool in pools)
            {
                if (!pool.Has(entityId))
                    continue;
                
                var component = pool.GetRaw(entityId) as IBytesSerializableComponent;
                if (component == null)
                    continue;
                
                entityInfo.components.Add(new SerializedComponent(component));
            }
            
            return entityInfo;
        }
    }
}