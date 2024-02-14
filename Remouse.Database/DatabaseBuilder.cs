using System;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.Database
{
    public sealed class DatabaseBuilder : IDatabaseBuilder
    {
        private Dictionary<Type, Settings> _settingsByType = new Dictionary<Type, Settings>();
        private Dictionary<Type, Table> _tablesByDataType = new Dictionary<Type, Table>();
        
        public void BindTable<T>(Table<T> table, bool overrideIfExist = false) where T : TableData
        {
            BindTable((Table)table, overrideIfExist);
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

        public void BindSettings(Settings settings)
        {
            if (settings == null)
                throw new NullReferenceException("Settings is null");
            
            var settingsType = settings.GetType();
            
            if (_settingsByType.ContainsKey(settingsType))
                throw new ArgumentException($"Settings of {settingsType} type of data already added");

            _settingsByType[settingsType] = settings;
        }

        public void Fetch(IDatabase database)
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

        public IDatabase Build()
        {
            Database database = new Database(_settingsByType.Values?.ToList(), _tablesByDataType.Values?.ToList());
            return database;
        }
    }
}