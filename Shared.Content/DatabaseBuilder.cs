using System;
using System.Collections.Generic;

namespace Shared.Content
{
    public class DatabaseBuilder
    {
        private Dictionary<Type, Settings> _settingsByType = new Dictionary<Type, Settings>();
        private Dictionary<Type, object> _tablesByDataType = new Dictionary<Type, object>();
        
        public void BindTable<T>(Table<T> table) where T : TableData
        {
            if (table == null)
                throw new NullReferenceException("Table is null");
            
            var dataType = typeof(T);
            
            if (_tablesByDataType.ContainsKey(dataType))
                throw new ArgumentException("Table of this type of data already added");

            _tablesByDataType[dataType] = table;
        }

        public void BindSettings<T>(T settings) where T : Settings
        {
            if (settings == null)
                throw new NullReferenceException("Settings is null");
            
            var settingsType = typeof(T);
            
            if (_settingsByType.ContainsKey(settingsType))
                throw new ArgumentException("Settings of this type of data already added");

            _settingsByType[settingsType] = settings;
        }

        public Database Build()
        {
            Database database = new Database(_settingsByType, _tablesByDataType);
            return database;
        }
    }
}