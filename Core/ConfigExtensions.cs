using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Remouse.Core.Configs;
using Remouse.Core.World;
using Remouse.DatabaseLib;
using Remouse.Models.Database;
using Remouse.Shared.Utils.Log;
using Utils;

namespace Remouse.Core
{
    public static class ConfigExtensions
    {
        private static Type[] componentTypes = TypeUtils<IComponent>.DerivedInstanceTypes;

        public static int CreateEntity(this EntityConfig config, IWorld world, Database database)
        {
            var typeConfig = config.typeId.GetTableData(database);
                
            var entity = world.NewEntity();

            typeConfig.components.ApplyOnEntity(entity, world);
            config.overrideComponents.ApplyOnEntity(entity, world);
            return entity;
        }
        
        public static int CreateEntity(this EntityTypeConfig config, IWorld world)
        {
            int entity = world.NewEntity();
            config.components.ApplyOnEntity(entity, world);

            return entity;
        }

        public static void ApplyOnEntity(this IEnumerable<ComponentConfig> configs, int entity, IWorld world)
        {
            foreach (var componentConfig in configs)
            {
                IComponent component = componentConfig.CreateComponentWithConfigValues();
                if (component == null)
                {
                    Logger.Current.LogError("ConfigExtension", $"Component type not found for config with name {componentConfig.componentType}" );
                    continue;
                }
                
                world.SetComponent(component.GetType(), entity, component);
            }
        }
        
        public static IComponent CreateComponentWithConfigValues(this ComponentConfig config)
        {
            var componentType = componentTypes.FirstOrDefault(c => c.Name == config.componentType);
            
            if (componentType == null)
                return null;
            
            var component = Activator.CreateInstance(componentType);

            //TODO: cache types and fields
            FieldInfo[] fields = componentType.GetFields();
            foreach (var field in fields)
            {
                var componentFieldValue = config.fieldValues.FirstOrDefault(f => f.fieldName == componentType.Name);

                if (componentFieldValue != null)
                {
                    field.SetValue(component, componentFieldValue.value);
                }
            }

            return component as IComponent;
        }
        
        public static T CreateComponentWithConfigValues<T>(this ComponentConfig config) where T : IComponent
        {
            Type componentType = typeof(T);
            var component = Activator.CreateInstance(componentType);

            FieldInfo[] fields = componentType.GetFields();
            foreach (var field in fields)
            {
                var componentFieldValue = config.fieldValues.FirstOrDefault(f => f.fieldName == field.FieldType.Name);
                
                if (componentFieldValue != null)
                {
                    field.SetValue(component, componentFieldValue.value);
                }
            }

            return component is T ? (T)component : default;
        }
        
        public static T CreateComponentWithConfigValues<T>(this EntityTypeConfig entityTypeConfig) where T : IComponent
        {
            var componentConfig = entityTypeConfig.components.FirstOrDefault(c => c.componentType == typeof(T).Name);

            return componentConfig.CreateComponentWithConfigValues<T>();
        }
        
        public static bool ContainComponent<T>(this EntityConfig entityConfig) where T : IComponent
        {
            var componentName = typeof(T).Name;
            return entityConfig.overrideComponents.FirstOrDefault(c => c.componentType == componentName) != default;
        }
        
        public static bool ContainComponent<T>(this EntityTypeConfig entityTypeConfig) where T : IComponent
        {
            var componentName = typeof(T).Name;
            return entityTypeConfig.components.FirstOrDefault(c => c.componentType == componentName) != default;
        }
    }
}