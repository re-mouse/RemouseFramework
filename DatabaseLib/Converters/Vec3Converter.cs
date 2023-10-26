using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Remouse.MathLib;

namespace Remouse.Core.Configs
{
    public class Vec3Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vec3);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vec = (Vec3)value;
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
            return new Vec3
            {
                x = obj["x"].Value<float>(),
                y = obj["y"].Value<float>(),
                z = obj["z"].Value<float>()
            };
        }
    }
}