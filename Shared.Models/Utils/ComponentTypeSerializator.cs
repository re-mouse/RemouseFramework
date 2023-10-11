using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Online.Commands
{
    public static class ComponentTypeSerializator
    {
        private static Dictionary<Type, ushort> _IdsByType = new Dictionary<Type, ushort>();
        private static Dictionary<ushort, Type> _TypesById = new Dictionary<ushort, Type>();
        
        static ComponentTypeSerializator()
        {
            var components = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IBaseComponent).IsAssignableFrom(t))
                .OrderBy(t => t.FullName, StringComparer.Ordinal).ToList();

            for (ushort i = 0; i < components.Count; i++)
            {
                _IdsByType[components[i]] = i;
                _TypesById[i] = components[i];
            }
        }

        public static ushort GetComponentId(IBaseComponent component)
        {
            return GetComponentId(component.GetType());
        }
        
        public static ushort GetComponentId(Type type)
        {
            if (_IdsByType.TryGetValue(type, out ushort id))
                return id;

            throw new KeyNotFoundException("Type is not component");
        }

        public static Type GetComponentType(ushort typeId)
        {
            if (_TypesById.TryGetValue(typeId, out Type type))
                return type;

            return null;
        }

        public static IBaseComponent GetEmptyComponent(ushort typeId)
        {
            if(_TypesById.TryGetValue(typeId, out var type))
            {
                if(typeof(IBaseComponent).IsAssignableFrom(type))
                {
                    return (IBaseComponent)Activator.CreateInstance(type);
                }

                return null;
            }
            else
            {
                return null;
            }
        }
    }
}