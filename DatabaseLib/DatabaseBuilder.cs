using System;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.DatabaseLib
{
    public class DatabaseBuilder
    {
        private Dictionary<Type, Settings> _settingsByType = new Dictionary<Type, Settings>();
        private Dictionary<Type, Table> _tablesByDataType = new Dictionary<Type, Table>();
        
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
            Database database = new Database(_settingsByType.Values?.ToList(), _tablesByDataType.Values?.ToList());
            return database;
        }
    }
}