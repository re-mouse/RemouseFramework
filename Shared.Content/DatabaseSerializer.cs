using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Remouse.Shared.Content
{
    public static class DatabaseSerializer
    {
        public static string SerializeDatabase(Database database)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (var table in database.GetTables())
            {
                var genericArg = table.GetType().GenericTypeArguments[0];
                
                result.Add(genericArg.AssemblyQualifiedName, table);
            }

            foreach (var setting in database.GetSetting())
            {
                var settingType = setting.GetType();
                result[settingType.AssemblyQualifiedName] = setting;
            }

            return JsonConvert.SerializeObject(result);
        }

        public static Database DeserializeDatabase(string jsonString)
        {
            var serializedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

            var tablesByDataType = new Dictionary<Type, object>();
            var settingsByType = new Dictionary<Type, Settings>();

            foreach (var item in serializedData)
            {
                var type = Type.GetType(item.Key);
                
                if (type == null)
                    throw new Exception();

                if (type.IsSubclassOf(typeof(TableData)))
                {
                    var tableType = typeof(Table<>).MakeGenericType(type);
                    var deserializedTable = JsonConvert.DeserializeObject(item.Value.ToString(), tableType);
                    tablesByDataType[type] = deserializedTable;
                }
                else
                {
                    var settingValue = JsonConvert.DeserializeObject(item.Value.ToString(), type) as Settings;
                    settingsByType[type] = settingValue;
                }
            }

            return new Database(settingsByType, tablesByDataType);
        }
    }
}
