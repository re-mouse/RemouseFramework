using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remouse.Utils;
using UnityEngine;

namespace Remouse.Core.Configs
{
    public class ComponentConfigJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(ComponentConfig);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var component = (ComponentConfig)value;
            if (component == null)
            {
                Debug.LogError($"Error on deserializing, component not found {value}, type - {value.GetType()}");
                return;
            }
            
            var obj = new JObject
            {
                ["componentType"] = component.componentType
            };

            if (component.fieldValues == null || component.fieldValues.Count == 0)
            {
                obj.WriteTo(writer);
                return;
            }

            foreach (var prop in component.fieldValues)
            {
                obj[prop.fieldName] = JToken.FromObject(prop.value, serializer);
            }

            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            
            List<ComponentFieldValue> fieldValues = obj
                .Properties()
                .Where(p => p.Name != "componentType")
                .Select(p => new ComponentFieldValue(p.Name, p.Value.ToString()))
                .ToList();
            
            var fieldName = obj["componentType"]?.ToString();
            if (fieldName.IsNullOrEmpty())
                return default;
            
            var component = new ComponentConfig
            {
                componentType = fieldName,
                fieldValues = fieldValues
            };
            
            return component;
        }
    }
}