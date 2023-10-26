using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Remouse.Core.Configs
{
    public class TableDataLinkConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(TableDataLink<>);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var idProperty = value.GetType().GetField("Id");
            var idValue = idProperty.GetValue(value);
            var obj = new JObject
            {
                ["Id"] = (string)idValue
            };
            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var idValue = reader.Value.ToString();

                var linkIdType = objectType.GetGenericTypeDefinition();
                var genericType = objectType.GetGenericArguments()[0];
                var constructedType = linkIdType.MakeGenericType(genericType);
                var linkIdInstance = Activator.CreateInstance(constructedType);

                var idProperty = constructedType.GetField("Id");
                idProperty.SetValue(linkIdInstance, idValue);

                return linkIdInstance;
            }

            var obj = JObject.Load(reader);
            var objIdValue = obj["Id"].Value<string>();

            var linkIdTypeObj = objectType.GetGenericTypeDefinition();
            var genericTypeObj = objectType.GetGenericArguments()[0];
            var constructedTypeObj = linkIdTypeObj.MakeGenericType(genericTypeObj);
            var linkIdInstanceObj = Activator.CreateInstance(constructedTypeObj);

            var idPropertyObj = constructedTypeObj.GetField("Id");
            idPropertyObj.SetValue(linkIdInstanceObj, objIdValue);

            return linkIdInstanceObj;
        }
    }
}