using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Remouse.Core.Configs;

namespace Remouse.DatabaseLib
{
    public static class DatabaseSerializer
    {
        private static readonly HashSet<Type> converterTypes = new HashSet<Type>();
        private static readonly JsonSerializerSettings settings;

        static DatabaseSerializer()
        {
            settings = new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>
                {
                    new Vec2IntConverter(),
                    new Vec3Converter(),
                    new Vec4Converter(),
                    new TableDataLinkConverter(),
                    new ComponentConfigConverter()
                }
            };
        }
        public static string SerializeDatabase(Database database)
        {
            var result = new Dictionary<string, object>();

            foreach (var table in database.GetTables())
            {
                var genericArg = table.GetType().GenericTypeArguments[0];
                result[genericArg.AssemblyQualifiedName] = table;
            }

            foreach (var setting in database.GetSetting())
            {
                result[setting.GetType().AssemblyQualifiedName] = setting;
            }

            return JsonConvert.SerializeObject(result, settings);
        }

        public static void AddConverter<T>(T converter) where T : JsonConverter
        {
            if (converterTypes.Add(typeof(T)))
            {
                settings.Converters.Add(converter);
            }
        }
        
        public static void RemoveConverter<T>(T converter) where T : JsonConverter
        {
            var type = typeof(T);

            if (!converterTypes.Remove(type))
                return;
            
            for (var i = 0; i < settings.Converters.Count; i++)
            {
                if (settings.Converters[i].GetType() == type)
                {
                    settings.Converters.RemoveAt(i);
                    break;
                }
            }
        }

        public static Database DeserializeDatabase(string jsonString)
        {
            var serializedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString, settings);
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

        private static void DeserializeItem(KeyValuePair<string, object> item, List<Table> tableList, List<Settings> settingsList)
        {
            var type = Type.GetType(item.Key);
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

        private static void DeserializeAndAddTable(KeyValuePair<string, object> item, List<Table> tableList, Type type)
        {
            var tableType = typeof(Table<>).MakeGenericType(type);
            var deserializedTable = JsonConvert.DeserializeObject(item.Value.ToString(), tableType, settings);
            tableList.Add(deserializedTable as Table);
        }

        private static void DeserializeAndAddSetting(KeyValuePair<string, object> item, List<Settings> settingsList, Type type)
        {
            var settingValue = JsonConvert.DeserializeObject(item.Value.ToString(), type, settings) as Settings;
            if (settingValue == null)
            {
                throw new InvalidOperationException($"Failed to deserialize setting for type: {type.FullName}");
            }
            settingsList.Add(settingValue);
        }
    }
}
