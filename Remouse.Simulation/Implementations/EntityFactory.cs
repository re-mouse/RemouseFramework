using System.Collections.Generic;
using Remouse.World;
using Remouse.Database;
using ReDI;
using Models.Database;
using Remouse.Utils;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    internal class EntityFactory : IEntityFactory
    {
        [Inject] private IDatabase _database;
        [Inject] private SimulationHost _simulationHost;
        
        private EcsWorld _world;
        private EcsPool<TypeInfo> _typeInfoPool;
        private EcsPool<CreatedEntityEvent> _createdEntityEventsPool;

        [Inject]
        private void SubscribeToEvents(SimulationHost host)
        {
            host.SimulationChanged += HandleSimulationChange;
        }

        private void HandleSimulationChange(ISimulation simulation)
        {
            _world = simulation?.World;
            _typeInfoPool = _world?.GetPoolOrCreate<TypeInfo>();
            _createdEntityEventsPool = _world?.GetPoolOrCreate<CreatedEntityEvent>();
        }
        
        public int Create()
        {
            if (_world == null)
            {
                LLogger.Current.LogError(this, "Load simulation first");
                return -1;
            }
            
            var entityId = _world.NewEntity();
            _createdEntityEventsPool.Add(entityId);
            return entityId;
        }
        
        public int Create(string typeId)
        {
            if (_world == null)
            {
                LLogger.Current.LogError(this, "Load simulation first");
                return -1;
            }
            
            var entityId = _world.NewEntity();
            
            var typeConfig = _database.GetTableData<EntityTypeConfig>(typeId);
            ApplyComponentConfigs(typeConfig.components, entityId);
            ApplyTypeIdInfo(entityId, typeId);
            _createdEntityEventsPool.Add(entityId);
            return entityId;
        }

        private void ApplyComponentConfigs(List<ComponentConfig> componentConfigs, int entityId)
        {
            foreach (var componentConfig in componentConfigs)
            {
                if (componentConfig == null || componentConfig.componentType.IsNullOrEmpty())
                {
                    LLogger.Current.LogError(this, "Empty config in map");
                    continue;
                }

                IComponent component = ComponentFactory.CreateFromConfig(componentConfig);
                if (component == null)
                {
                    LLogger.Current.LogError(this,
                        $"Component type not found for config with name {componentConfig.componentType}");
                    continue;
                }

                var pool = _world.GetPoolOrCreateByType(component.GetType());
                pool?.AddOrSetRaw(entityId, component);
            }
        }
        
        private void ApplyTypeIdInfo(int entityId, string typeId)
        {
            ref var typeInfo = ref _typeInfoPool.AddOrGet(entityId);
            typeInfo.typeId = typeId;
        }
    }
}