using System;
using System.Linq;
using System.Reflection;
using Remouse.Core.Configs;
using Remouse.Core.World;
using Remouse.DatabaseLib.Tables;

namespace Remouse.Core
{
    public static class ConfigExtensions
    {
        public static IComponent Create(this ComponentConfig config, IReadOnlyWorld world)
        {
            var componentType = world.GetComponentTypes().FirstOrDefault(c => c.Name == config.name);
            
            if (componentType == null)
                return null;
            
            var component = Activator.CreateInstance(componentType);

            FieldInfo[] fields = componentType.GetFields();
            foreach (var field in fields)
            {
                if (config.componentValues.TryGetValue(field.Name, out var value) && field.FieldType == value.GetType())
                {
                    field.SetValue(component, value);
                }
            }

            return component as IComponent;
        }
        
        public static T Create<T>(this ComponentConfig config, IReadOnlyWorld world) where T : IComponent
        {
            var componentType = world.GetComponentTypes().FirstOrDefault(c => c.Name == config.name);
            
            if (componentType == null)
                return default;
            
            var component = Activator.CreateInstance(componentType);

            FieldInfo[] fields = componentType.GetFields();
            foreach (var field in fields)
            {
                if (config.componentValues.TryGetValue(field.Name, out var value) && field.FieldType == value.GetType())
                {
                    field.SetValue(component, value);
                }
            }

            return component is T ? (T)component : default;
        }
        
        public static bool ContainComponent<T>(this EntityConfig entityConfig, IReadOnlyWorld world) where T : IComponent
        {
            var componentName = typeof(T).Name;
            return entityConfig.overrideComponents.FirstOrDefault(c => c.name == componentName) != default;
        }
        
        public static bool ContainComponent<T>(this EntityTypeConfig entityTypeConfig, IReadOnlyWorld world) where T : IComponent
        {
            var componentName = typeof(T).Name;
            return entityTypeConfig.components.FirstOrDefault(c => c.name == componentName) != default;
        }

        public static T CreateComponent<T>(this EntityTypeConfig entityTypeConfig, IReadOnlyWorld world) where T : IComponent
        {
            var componentConfig = entityTypeConfig.components.FirstOrDefault(c => c.name == typeof(T).Name);

            return componentConfig.Create<T>(world);
        }
    }
}