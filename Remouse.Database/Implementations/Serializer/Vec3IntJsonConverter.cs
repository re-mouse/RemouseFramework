using System;
using Remouse.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Remouse.Database
{
    public class Vec3IntJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vec3Int);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vec = (Vec3Int)value;
            var obj = new JObject
            {
                ["x"] = vec.x,
                ["y"] = vec.y,
                ["z"] = vec.z
            };
            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            return new Vec3Int
            {
                x = obj["x"].Value<int>(),
                y = obj["y"].Value<int>(),
                z = obj["z"].Value<int>()
            };
        }
    }
}