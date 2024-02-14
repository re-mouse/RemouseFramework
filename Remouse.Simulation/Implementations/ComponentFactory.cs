using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Remouse.World;
using Remouse.Database;
using Remouse.Math;
using Models.Database;
using Remouse.Utils;

namespace Remouse.Simulation
{
    public class ComponentFactory
    {
        private static Type[] componentTypes = TypeUtils<IComponent>.DerivedInstanceTypes;
        
        public static IComponent CreateFromConfig(ComponentConfig config)
        { 
            var componentType = componentTypes?.FirstOrDefault(c => c?.Name == config?.componentType);
            
            if (componentType == null)
                return null;
            
            var component = Activator.CreateInstance(componentType);

            FieldInfo[] fields = componentType.GetFields();
            foreach (var field in fields)
            {
                var componentFieldValue = config.fieldValues.FirstOrDefault(f => f.fieldName == field.Name);

                if (componentFieldValue != null)
                {
                    var fieldType = field.FieldType;
                    var fieldValue = componentFieldValue.value;
                    field.SetValue(component, ParseField(fieldType, fieldValue));
                }
            }

            return component as IComponent;
        }

        private static object ParseField(Type fieldType, string fieldValue)
        {
            if (fieldType == typeof(int))
            {
                return int.Parse(fieldValue);
            }
            else if (fieldType == typeof(long))
            {
                return long.Parse(fieldValue);
            }
            else if (fieldType == typeof(short))
            {
                return short.Parse(fieldValue);
            }
            else if (fieldType == typeof(uint))
            {
                return uint.Parse(fieldValue);
            }
            else if (fieldType == typeof(ushort))
            {
                return ushort.Parse(fieldValue);
            }
            else if (fieldType == typeof(ulong))
            {
                return ulong.Parse(fieldValue);
            }
            else if (fieldType == typeof(byte))
            {
                return byte.Parse(fieldValue);
            }
            else if (fieldType == typeof(sbyte))
            {
                return sbyte.Parse(fieldValue);
            }
            else if (fieldType == typeof(decimal))
            {
                return decimal.Parse(fieldValue);
            }
            else if (fieldType == typeof(float))
            {
                return float.Parse(fieldValue);
            }
            else if (fieldType == typeof(double))
            {
                return double.Parse(fieldValue);
            }
            else if (fieldType == typeof(bool))
            {
                return bool.Parse(fieldValue);
            }
            else if (fieldType == typeof(string))
            {
                return fieldValue;
            }
            else if (fieldType.IsEnum)
            {
                return Enum.Parse(fieldType, fieldValue);
            }
            else if (fieldType == typeof(Vec2))
            {
                return VecParser.ParseVec2(fieldValue);
            }
            else if (fieldType == typeof(Vec2Int))
            {
                return VecParser.ParseVec2Int(fieldValue);
            }
            else if (fieldType == typeof(Vec3))
            {
                return VecParser.ParseVec3(fieldValue);
            }
            else if (fieldType == typeof(Vec3Int))
            {
                return VecParser.ParseVec3Int(fieldValue);
            }
            else if (fieldType == typeof(Vec4))
            {
                return VecParser.ParseVec4(fieldValue);
            }
            else
            {
                return null;
            }
        }
    }
}