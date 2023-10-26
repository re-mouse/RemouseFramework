using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remouse.MathLib;

namespace Remouse.Core.Configs
{
    public class Vec4Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vec4);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vec = (Vec4)value;
            var obj = new JObject
            {
                ["x"] = vec.x,
                ["y"] = vec.y,
                ["z"] = vec.z,
                ["w"] = vec.w
            };
            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            return new Vec4
            {
                x = obj["x"].Value<float>(),
                y = obj["y"].Value<float>(),
                z = obj["z"].Value<float>(),
                w = obj["w"].Value<float>()
            };
        }
    }
}