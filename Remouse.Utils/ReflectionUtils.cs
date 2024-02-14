using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Remouse.Utils
{
    public static class ReflectionUtils
    {
        internal static Type[] GetAllDerivedTypes(Type originalType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => originalType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToArray();
        }
    }
}