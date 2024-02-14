using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remouse.Math;

namespace Remouse.Database
{
    public class Vec2IntJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vec2Int);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vec = (Vec2Int)value;
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
            return new Vec2Int
            {
                x = obj["x"].Value<int>(),
                y = obj["y"].Value<int>()
            };
        }
    }
}