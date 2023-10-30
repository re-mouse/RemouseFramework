using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Remouse.Core.Configs
{
    public class ComponentConfigConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(ComponentConfig);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var component = (ComponentConfig)value;
            var obj = new JObject
            {
                ["name"] = component.name
            };
            foreach (var prop in component.componentValues)
            {
                obj[prop.Key] = JToken.FromObject(prop.Value, serializer);
            }

            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var component = new ComponentConfig
            {
                name = obj["name"].Value<string>(),
                componentValues = obj.Properties().Where(p => p.Name != "name")
                    .ToDictionary(p => p.Name, p => p.Value.ToObject<string>(serializer))
            };
            return component;
        }
    }
}