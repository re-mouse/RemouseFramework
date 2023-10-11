using System;
using System.Collections.Generic;

namespace Shared.Content
{
    public class Database
    {
        private readonly Dictionary<Type, Settings> _settingsByType;
        private readonly Dictionary<Type, object> _tablesByDataType;

        internal Database(Dictionary<Type, Settings> settingsByType, Dictionary<Type, object> tablesByDataType)
        {
            _settingsByType = settingsByType;
            _tablesByDataType = tablesByDataType;
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
    }
}