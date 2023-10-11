using System;
using System.Collections.Generic;
using Shared;
using Shared.DIContainer;
using Shared.EcsLib;
using Shared.GameSimulation;
using Shared.GameSimulation.ECS.Components;
using Shared.Online.Commands;
using Shared.Online.Models;
using Shared.Utils.ObjectPool;

namespace GameServer.ServerShards
{
    public class EntityUpdatesTracker
    {
        private GameSimulation _simulation;
        private EcsPool<NetworkIdentity> _networkIdentitysPool;

        private TrackedUpdates _trackedUpdates = new TrackedUpdates();
        private ObjectPool<HashSet<Type>> _updatesPool = new ObjectPool<HashSet<Type>>();
        
        private SimulationChanges _cachedChangeses = new SimulationChanges();
        
        public void Construct(Container container)
        {
            _simulation = container.Get<GameSimulation>();

            _simulation.entityCreated += HandleEntityCreated;
            _simulation.entityDestroyed += HandleEntityDeleted;
            _simulation.entityChanged += HandleComponentUpdated;
            
            _networkIdentitysPool = _simulation.World.GetPool<NetworkIdentity>();
        }

        public void Clear()
        {
            foreach (var dirtyComponents in _trackedUpdates.dirtyComponents.Values)
            {
                dirtyComponents.Clear();
                _updatesPool.Return(dirtyComponents);
            }
            
            _trackedUpdates.dirtyComponents.Clear();
            _trackedUpdates.createdEntities.Clear();
            _trackedUpdates.deletedEntities.Clear();
        }

        public SimulationChanges GetUpdates()
        {
            var updates = new SimulationChanges();

            updates.createdEntities = _trackedUpdates.createdEntities;
            updates.deletedEntities = _trackedUpdates.deletedEntities;

            foreach (var dirtyComponent in _trackedUpdates.dirtyComponents)
            {
                int entityId = dirtyComponent.Key;
                HashSet<Type> componentTypes = dirtyComponent.Value;

                foreach (var type in componentTypes)
                {
                    var pool = _simulation.World.GetPoolByType(type);
                    
                    if (pool == null)
                        continue;

                    ushort? componentId = ComponentTypeSerializator.GetComponentId(type);
                    
                    if (componentId == null)
                        continue;

                    if (!pool.Has(entityId))
                    {
                        updates.deletedComponents.Add(new EntityComponentDeleted(entityId, componentId.Value));
                    }
                    else
                    {
                        IBaseComponent component = pool.GetRaw(entityId) as IBaseComponent;
                        
                        if (component == null)
                            continue;
                        
                        updates.updatedComponents.Add(new EntityComponentUpdate(entityId, component));
                    }
                }
            }
            
            
            return updates;
        }
        
        public void HandleEntityCreated(int entityId, string factoryId)
        {
            if (!IsNetworkEntity(entityId))
                return;
            
            _trackedUpdates.createdEntities.Add(new CreatedEntity(entityId, factoryId));
        }

        public void HandleEntityDeleted(int entityId)
        {
            _trackedUpdates.deletedEntities.Add(entityId);
        }

        public void HandleComponentUpdated(int entityId, Type componentType)
        {
            if (!IsNetworkEntity(entityId))
                return;
            
            MarkDirtyComponent(entityId, componentType);
        }

        private bool IsNetworkEntity(int entityId)
        {
            return _networkIdentitysPool.Has(entityId);
        }

        private void MarkDirtyComponent(int entityId, Type type)
        {
            if (_trackedUpdates.dirtyComponents.TryGetValue(entityId, out var dirtyComponents))
            {
                dirtyComponents.Add(type);
            }
            else
            {
                var pooledDirtyComponentsContainer = _updatesPool.Get();
                
                pooledDirtyComponentsContainer.Add(type);

                _trackedUpdates.dirtyComponents[entityId] = pooledDirtyComponentsContainer;
            }
        }
        
        private class TrackedUpdates
        {
            public List<CreatedEntity> createdEntities = new List<CreatedEntity>();
            public List<int> deletedEntities = new List<int>();
            public Dictionary<int, HashSet<Type>> dirtyComponents = new Dictionary<int, HashSet<Type>>();
        }
    }
}