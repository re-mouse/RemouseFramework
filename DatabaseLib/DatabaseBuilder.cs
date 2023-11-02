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
        
        public void BindTable(Table table, bool overrideIfExist)
        {
            if (table == null)
                throw new NullReferenceException("Table is null");

            if (table.GetType().GetGenericTypeDefinition() != typeof(Table<>))
                throw new ArgumentException("Invalid table type");
            
            var genericArguments = table.GetType()?.GetGenericArguments();
            var dataType = genericArguments?.FirstOrDefault();
            
            if (_tablesByDataType.ContainsKey(dataType) && !overrideIfExist)
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

        public void Fetch(Database database)
        {
            var settings = database.GetSettings();
            foreach (var setting in settings)
            {
                _settingsByType[setting.GetType()] = setting;
            }
            
            var tables = database.GetTables();
            foreach (var table in tables)
            {
                _tablesByDataType[table.GetType()] = table;
            }
        }

        public Database Build()
        {
            Database database = new Database(_settingsByType.Values?.ToList(), _tablesByDataType.Values?.ToList());
            return database;
        }
    }
}