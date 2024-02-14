using System;
using System.Linq;

namespace Remouse.Utils
{
    public static class TypeUtils<T>
    {
        private static Type[] derivedInstanceTypes = GetAllDerivedTypes();

        public static Type[] DerivedInstanceTypes { get => derivedInstanceTypes; }

        public static Type FindInDerived(Func<Type, bool> predicate)
        {
            foreach (var derivedInstanceType in derivedInstanceTypes)
            {
                if (predicate(derivedInstanceType))
                    return derivedInstanceType;
            }

            return null;
        }
        
        public static Type FindInDerivedByName(string name)
        {
            foreach (var derivedInstanceType in derivedInstanceTypes)
            {
                if (name == derivedInstanceType.Name)
                    return derivedInstanceType;
            }

            return null;
        }
        
        internal static Type[] GetAllDerivedTypes()
        {
            
            var originalType = typeof(T);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => originalType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToArray();
        }
    }
}