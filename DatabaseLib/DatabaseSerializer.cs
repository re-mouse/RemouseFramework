using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Remouse.DatabaseLib
{
    public static class DatabaseSerializer
    {
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

            return JsonConvert.SerializeObject(result);
        }

        public static Database DeserializeDatabase(string jsonString)
        {
            var serializedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
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
            var deserializedTable = JsonConvert.DeserializeObject(item.Value.ToString(), tableType);
            tableList.Add(deserializedTable as Table);
        }

        private static void DeserializeAndAddSetting(KeyValuePair<string, object> item, List<Settings> settingsList, Type type)
        {
            var settingValue = JsonConvert.DeserializeObject(item.Value.ToString(), type) as Settings;
            if (settingValue == null)
            {
                throw new InvalidOperationException($"Failed to deserialize setting for type: {type.FullName}");
            }
            settingsList.Add(settingValue);
        }
    }
}
