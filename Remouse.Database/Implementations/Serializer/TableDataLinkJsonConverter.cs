using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Remouse.Database
{
    public class TableDataLinkJsonConverter : JsonConverter
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
            string idValue = reader.TokenType == JsonToken.Null ? "" : 
                reader.TokenType == JsonToken.String ? 
                reader.Value.ToString() : 
                JObject.Load(reader)["Id"].Value<string>();
            
            var linkIdTypeObj = objectType.GetGenericTypeDefinition();
            var genericTypeObj = objectType.GetGenericArguments()[0];
            var constructedTypeObj = linkIdTypeObj.MakeGenericType(genericTypeObj);
            var linkIdInstanceObj = Activator.CreateInstance(constructedTypeObj);

            var idPropertyObj = constructedTypeObj.GetField("Id");
            idPropertyObj.SetValue(linkIdInstanceObj, idValue);

            return linkIdInstanceObj;
        }
    }
}