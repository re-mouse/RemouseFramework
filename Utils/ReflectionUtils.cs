using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utils
{
    public static class TypeUtils<T>
    {
        private static Type[] derivedInstanceTypes = ReflectionUtils.GetAllDerivedTypes(typeof(T));

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
    }
    
    public static class ReflectionUtils
    {
        internal static Type[] GetAllDerivedTypes(Type originalType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => originalType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToArray();
        }
        
        public static List<T> GetValuesFromFieldsOfType<T>(object obj)
        {
            if (obj == null)
                return new List<T>();
            
            List<T> values = new List<T>();
        
            FieldInfo[] fields = obj.GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(T) || field.FieldType.IsSubclassOf(typeof(T)))
                {
                    values.Add((T)field.GetValue(obj));
                }
            }
            return values;
        }
        
        public static List<T> GetValuesFromPropertiesOfType<T>(T obj, BindingFlags flags)
        {
            if (obj == null)
                return new List<T>();
            
            List<T> values = new List<T>();
        
            PropertyInfo[] properties = obj.GetType().GetProperties(flags);
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(T) || property.PropertyType.IsSubclassOf(typeof(T)))
                {
                    values.Add((T)property.GetValue(obj));
                }
            }
            return values;
        }
    }
}