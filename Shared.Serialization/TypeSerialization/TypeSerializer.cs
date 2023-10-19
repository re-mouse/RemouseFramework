using System;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.Shared.Models
{
    public static class TypeSerializer<T> where T : ISerializableType
    {
        private static Dictionary<Type, ushort> idsByType = new Dictionary<Type, ushort>();
        private static Dictionary<ushort, Type> typesById = new Dictionary<ushort, Type>();
        
        static TypeSerializer()
        {
            var components = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t))
                .OrderBy(t => t.FullName, StringComparer.Ordinal).ToList();

            for (ushort i = 0; i < components.Count; i++)
            {
                idsByType[components[i]] = i;
                typesById[i] = components[i];
            }
        }

        public static ushort GetTypeId(T component)
        {
            return GetTypeId(component.GetType());
        }
        
        public static ushort GetTypeId(Type type)
        {
            if (idsByType.TryGetValue(type, out ushort id))
                return id;

            throw new KeyNotFoundException("Type is not component");
        }

        public static Type GetType(ushort typeId)
        {
            if (typesById.TryGetValue(typeId, out Type type))
                return type;

            return null;
        }
        
        public static bool TypeExist(ushort typeId)
        {
            return typesById.ContainsKey(typeId);
        }

        public static T GetNew(ushort typeId)
        {
            if(typesById.TryGetValue(typeId, out var type))
            {
                if(typeof(T).IsAssignableFrom(type))
                {
                    return (T)Activator.CreateInstance(type);
                }

                throw new AggregateException();
            }

            throw new KeyNotFoundException();
        }
    }
}