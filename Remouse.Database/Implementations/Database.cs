using System;
using System.Collections.Generic;
using System.Linq;
using Remouse.Utils;

namespace Remouse.Database
{
    internal sealed class Database : IDatabase
    {
        private readonly Dictionary<Type, Settings> _settingsByType = new Dictionary<Type, Settings>();
        private readonly Dictionary<Type, Table> _tablesByDataType = new Dictionary<Type, Table>();
        
        public Database(List<Settings> settings, List<Table> tables)
        {
            foreach (var setting in settings)
            {
                if (setting == null)
                    continue;

                _settingsByType[setting.GetType()] = setting;
            }
            
            foreach (var table in tables)
            {
                Type tableDataType = table.GetType().GenericTypeArguments[0];
                _tablesByDataType[tableDataType] = table;
            }
        }
        
        public T GetSetting<T>() where T : Settings
        {
            var settingsType = typeof(T);

            if (_settingsByType.TryGetValue(settingsType, out var settings))
                return (T)settings;
            
            return null;
        }

        public Table<T> GetTable<T>() where T : TableData
        {
            var dataType = typeof(T);

            if (!_tablesByDataType.TryGetValue(dataType, out var tableData))
            {
                LLogger.Current.LogError(this, $"Table not found [TableType:{dataType}]");
                return null;
            }

            return (Table<T>)tableData;
        }

        public T GetTableData<T>(string id) where T : TableData
        {
            var table = GetTable<T>();

            return table?.GetData(id);
        }

        public Settings[] GetSettings()
        {
            return _settingsByType.Values.ToArray();
        }

        public Table[] GetTables()
        {
            return _tablesByDataType.Values.ToArray();
        }
    }
}