using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Remouse.Core.Configs;
using Utils;

namespace Remouse.DatabaseLib
{
    public class DatabaseSerializer
    {
        private static Type[] _settingTypes = TypeUtils<Settings>.DerivedInstanceTypes;
        private static Type[] _tableDataTypes = TypeUtils<TableData>.DerivedInstanceTypes;
        
        private readonly HashSet<Type> _converterTypes = new HashSet<Type>();
        private readonly JsonSerializerSettings _settings;
        
        private Encoding DefaultEncoding => Encoding.UTF8;

        public DatabaseSerializer()
        {
            var defaultConverters = new List<JsonConverter>()
            {
                new Vec2IntJsonConverter(),
                new Vec3JsonConverter(),
                new Vec4JsonConverter(),
                new TableDataLinkJsonConverter(),
                new ComponentConfigJsonConverter()
            };
            
            _settings = new JsonSerializerSettings()
            {
                Converters = defaultConverters
            };
        }
        
        public byte[] SerializeToJsonBytes(Database database)
        {
            return DefaultEncoding.GetBytes(SerializeToJson(database));
        }
        
        public string SerializeToJson(Database database)
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
        
        public Database DeserializeFromBytes(byte[] json)
        {
            string jsonString = DefaultEncoding.GetString(json);
            return DeserializeFromJson(jsonString);
        }
        
        public Database DeserializeFromJson(string jsonString)
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
                throw new InvalidOperationException($"Unable to resolve type for key: {item.Key}");
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
            var deserializedTable = JsonConvert.DeserializeObject(item.Value.ToString(), tableType, _settings);
            tableList.Add(deserializedTable as Table);
        }

        private void DeserializeAndAddSetting(KeyValuePair<string, object> item, List<Settings> settingsList, Type type)
        {
            var settingValue = JsonConvert.DeserializeObject(item.Value.ToString(), type, _settings) as Settings;
            if (settingValue == null)
            {
                throw new InvalidOperationException($"Failed to deserialize setting for type: {type.FullName}");
            }
            settingsList.Add(settingValue);
        }
        
        public void AddJsonConverter<T>(T converter) where T : JsonConverter
        {
            if (_converterTypes.Add(typeof(T)))
            {
                _settings.Converters.Add(converter);
            }
        }
        
        public void RemoveJsonConverter<T>(T converter) where T : JsonConverter
        {
            var type = typeof(T);

            if (!_converterTypes.Remove(type))
                return;
            
            for (var i = 0; i < _settings.Converters.Count; i++)
            {
                if (_settings.Converters[i].GetType() == type)
                {
                    _settings.Converters.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
