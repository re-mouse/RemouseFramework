using System;
using System.Collections.Generic;
using Remouse.Utils;

namespace Remouse.Serialization
{
    public static class TypeSerializer<T>
    {
        private static Dictionary<Type, ushort> idsByType = new Dictionary<Type, ushort>();
        private static Dictionary<ushort, Type> typesById = new Dictionary<ushort, Type>();
        
        static TypeSerializer()
        {
            var components = TypeUtils<T>.DerivedInstanceTypes;

            for (ushort i = 0; i < components.Length; i++)
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

            throw new KeyNotFoundException();
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
                return (T)Activator.CreateInstance(type);
            }

            throw new KeyNotFoundException();
        }
    }
}