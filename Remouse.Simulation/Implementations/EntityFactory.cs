using System.Collections.Generic;
using Remouse.World;
using Remouse.Database;
using Remouse.DI;
using Models.Database;
using Remouse.Utils;

namespace Remouse.Simulation
{
    internal class EntityFactory : IEntityFactory
    {
        private IDatabase _database;
        
        public void Construct(Container container)
        {
            _database = container.Resolve<IDatabase>();
        }
        
        public int Create(IWorld world)
        {
            var entityId = world.NewEntity();
            world.GetOrAddComponent<CreatedEntityEvent>(entityId);
            return entityId;
        }
        
        public int Create(IWorld world, string typeId)
        {
            var entityId = world.NewEntity();
            
            var typeConfig = _database.GetTableData<EntityTypeConfig>(typeId);
            ApplyComponentConfigs(typeConfig.components, world, entityId);
            ApplyTypeIdInfo(world, entityId, typeId);
            world.GetOrAddComponent<CreatedEntityEvent>(entityId);
            return entityId;
        }

        private void ApplyComponentConfigs(List<ComponentConfig> componentConfigs, IWorld world, int entityId)
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

                world.SetComponent(component.GetType(), entityId, component);
            }
        }
        
        private void ApplyTypeIdInfo(IWorld world, int entityId, string typeId)
        {
            ref var typeInfo = ref world.GetOrAddComponent<TypeInfo>(entityId);
            typeInfo.typeId = typeId;
        }
    }
}