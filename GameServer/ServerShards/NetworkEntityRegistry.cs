using System;
using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.DIContainer;
using Shared.GameSimulation;
using Shared.Online.Commands;
using Shared.Online.Models;

namespace GameServer.ServerShards
{
    public class NetworkEntityRegistry
    {
        private static readonly Type[] ComponentTypes;
        static NetworkEntityRegistry()
        {
            ComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IBaseComponent).IsAssignableFrom(t) && t.IsValueType)
                .ToArray();
        }
        
        private readonly Dictionary<int, NetworkEntity> _entities = new Dictionary<int, NetworkEntity>();
        
        private GameSimulation _simulation;
        
        public IReadOnlyDictionary<int, NetworkEntity> entities => _entities;

        public void Construct(Container container)
        {
            _simulation = container.Get<GameSimulation>();
        }

        public void Update(SimulationChanges updates)
        {
            foreach (var deleted in updates.deletedEntities)
            {
                _entities.Remove(deleted);
            }
            
            foreach (var dirtyComponent in updates.updatedComponents)
            {
                if (!_entities.TryGetValue(dirtyComponent.entityId, out var entity))
                    continue;
                
                entity.components.Add(dirtyComponent.componentContainer.component.GetType());
            }
            
            foreach (var dirtyComponent in updates.deletedComponents)
            {
                if (!_entities.TryGetValue(dirtyComponent.entityId, out var entity))
                    continue;
                
                var type = ComponentTypeSerializator.GetComponentType(dirtyComponent.componentTypeId);
                
                entity.components.Remove(type);
            }
            
            foreach (var createdId in updates.createdEntities)
            {
                AddCreatedEntityInRegistry(createdId);
            }
        }

        private void AddCreatedEntityInRegistry(CreatedEntity createdId)
        {
            var newEntity = new NetworkEntity();
            newEntity.entityId = createdId.entityId;
            newEntity.name = createdId.entityName;
            
            foreach (var componentType in ComponentTypes)
            {
                var pool = _simulation.World.GetPoolByType(componentType);
                if (pool == null)
                    continue;
                
                if (pool.Has(createdId.entityId))
                {
                    newEntity.components.Add(pool.GetComponentType());
                }
            }
            
            _entities.Add(newEntity.entityId, newEntity);
        }
    }

    public partial class NetworkEntity
    {
        public int entityId;
        public string name;
        public HashSet<Type> components = new HashSet<Type>();
        
    }
}