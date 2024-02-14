using System;
using Remouse.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Remouse.Database
{
    public class Vec2JsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vec2);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vec = (Vec2)value;
            var obj = new JObject
            {
                ["x"] = vec.x,
                ["y"] = vec.y
            };
            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            return new Vec2
            {
                x = obj["x"].Value<float>(),
                y = obj["y"].Value<float>()
            };
        }
    }
}