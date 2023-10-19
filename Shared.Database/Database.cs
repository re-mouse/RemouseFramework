using System;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.Database
{
    public class Database
    {
        private readonly Dictionary<Type, Settings> _settingsByType;
        private readonly Dictionary<Type, object> _tablesByDataType;

        internal Database(List<Settings> settings, List<object> tables)
        {
            foreach (var setting in settings)
            {
                if (setting == null)
                    continue;

                _settingsByType[setting.GetType()] = setting;
            }
            
            foreach (var table in tables)
            {
                _tablesByDataType[table.GetType()] = table;
            }
        }
        
        public T GetSettings<T>() where T : Settings
        {
            var settingsType = typeof(T);

            if (_settingsByType.TryGetValue(settingsType, out var settings))
                return (T)settings;
            
            return null;
        }

        public T GetTableData<T>(string id) where T : TableData
        {
            var dataType = typeof(T);

            if (!_tablesByDataType.TryGetValue(dataType, out var tableData))
                return null;

            var table = (Table<T>)tableData;

            return table.GetData(id);
        }

        internal Settings[] GetSetting()
        {
            return _settingsByType.Values.ToArray();
        }
        
        internal object[] GetTables() => _tablesByDataType.Values.ToArray();

    }
}