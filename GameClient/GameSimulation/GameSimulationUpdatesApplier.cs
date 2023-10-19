using System.Collections.Generic;
using Remouse.Shared.Core;
using Remouse.Shared.Models;

namespace GameClient
{
    class GameSimulationUpdatesApplier
    {
        private Dictionary<int, int> _serverToLocalEntityIds = new Dictionary<int, int>();
        
        public void ApplyUpdate(Simulation simulation, WorldState changeses)
        {
            foreach (var createdEntity in changeses.entityInfos)
            {
                int localEntityId = simulation.World.CreateEntity(createdEntity.entityName);

                _serverToLocalEntityIds[createdEntity.entityId] = localEntityId;
            }
            
            foreach (var deletedEntity in changeses.deletedEntities)
            {
                int localId = _serverToLocalEntityIds[deletedEntity];
                
                _serverToLocalEntityIds.Remove(deletedEntity);
                
                simulation.World.DelEntity(localId);
            }
            
            foreach (var updatedComponent in changeses.updatedComponents)
            {
                int localId = _serverToLocalEntityIds[updatedComponent.entityId];

                var componentTypeId = updatedComponent.componentContainer.componentTypeId;
                var componentType = NetworkComponentTypeSerializer.GetComponentType(componentTypeId);
                var pool = simulation.World.GetPoolByType(componentType);

                if (!pool.Has(localId))
                {
                    pool.AddRaw(localId, updatedComponent.componentContainer.component);
                }
                else
                {
                    pool.SetRaw(localId, updatedComponent.componentContainer.component);
                }
            }
            
            foreach (var deletedComponent in changeses.deletedComponents)
            {
                int localId = _serverToLocalEntityIds[deletedComponent.entityId];

                var componentType = NetworkComponentTypeSerializer.GetComponentType(deletedComponent.componentTypeId);
                var pool = simulation.World.GetPoolByType(componentType);
                
                pool.Del(localId);
            }
        }
    }
}