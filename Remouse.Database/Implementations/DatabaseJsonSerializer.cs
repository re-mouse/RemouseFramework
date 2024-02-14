using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Remouse.Utils;

namespace Remouse.Database
{
    public class DatabaseJsonSerializer : IDatabaseSerializer
    {
        private static Type[] _settingTypes = TypeUtils<Settings>.DerivedInstanceTypes;
        private static Type[] _tableDataTypes = TypeUtils<TableData>.DerivedInstanceTypes;

        private readonly JsonSerializerSettings _settings;
        
        private Encoding DefaultEncoding { get => Encoding.UTF8; }

        public DatabaseJsonSerializer()
        {
            var defaultConverters = new List<JsonConverter>()
            {
                new TableDataLinkJsonConverter(),
                new Vec2JsonConverter(),
                new Vec3JsonConverter(),
                new Vec2IntJsonConverter(),
                new Vec3IntJsonConverter(),
                new Vec4JsonConverter()
            };
            
            _settings = new JsonSerializerSettings()
            {
                Converters = defaultConverters
            };
        }

        public void AddConverter(JsonConverter converter)
        {
            _settings.Converters.Add(converter);
        }
        
        public byte[] Serialize(IDatabase database)
        {
            return DefaultEncoding.GetBytes(SerializeToJson(database));
        }
        
        private string SerializeToJson(IDatabase database)
        {
            var jsonKeyValue = new Dictionary<string, object>();

            foreach (var table in database.GetTables())
            {
                var genericArg = table.GetType().GenericTypeArguments[0];
                jsonKeyValue[genericArg.Name ?? string.Empty] = table;
            }

            foreach (var setting in database.GetSettings())
            {
                jsonKeyValue[setting.GetType().Name] = setting;
            }

            return JsonConvert.SerializeObject(jsonKeyValue, _settings);
        }
        
        public IDatabase Deserialize(byte[] json)
        {
            string jsonString = DefaultEncoding.GetString(json);
            return DeserializeFromJson(jsonString);
        }
        
        private IDatabase DeserializeFromJson(string jsonString)
        {
            var serializedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString, _settings);
            if (serializedData == null)
            {
                throw new InvalidOperationException("Failed to deserialize the provided JSON string.");
            }

            var tableList = new List<Table>();
            var settingsList = new List<Settings>();

            foreach (var item in serializedData)
            {
                DeserializeItem(item, tableList, settingsList);
            }

            return new Database(settingsList, tableList);
        }

        private void DeserializeItem(KeyValuePair<string, object> item, List<Table> tableList, List<Settings> settingsList)
        {
            var type = _settingTypes.FirstOrDefault(s => s.Name == item.Key) ?? _tableDataTypes.FirstOrDefault(t => t.Name == item.Key);
            
            if (type == null)
            {
                LLogger.Current.LogError(this, $"Error while deserialized database, unknown type: {item.Key}");
                return;
            }

            if (type.IsSubclassOf(typeof(TableData)))
            {
                DeserializeAndAddTable(item, tableList, type);
            }
            else
            {
                DeserializeAndAddSetting(item, settingsList, type);
            }
        }

        private void DeserializeAndAddTable(KeyValuePair<string, object> item, List<Table> tableList, Type type)
        {
            var tableType = typeof(Table<>).MakeGenericType(type);
            try
            {
                var deserializedTable = JsonConvert.DeserializeObject(item.Value.ToString(), tableType, _settings);
                tableList.Add(deserializedTable as Table);
            }
            catch (Exception e)
            {
                LLogger.Current.LogError(this, $"Invalid value on table. Table {type.Name}, exception, {item} {e.Message}");
            }
        }

        private void DeserializeAndAddSetting(KeyValuePair<string, object> item, List<Settings> settingsList, Type type)
        {
            try
            {
                var settingValue = JsonConvert.DeserializeObject(item.Value.ToString(), type, _settings) as Settings;
                if (settingValue != null)
                    settingsList.Add(settingValue);
            }
            catch (Exception e)
            {
                LLogger.Current.LogError(this, $"Error on deserializing settings. {e.Message}");
            }
        }
    }
}
