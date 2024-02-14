using System;
using System.Linq;
using Remouse.Database;
using Remouse.UnityEngine.Assets;
using Infrastructure;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public static class UnityDatabaseHost
    {
        private static readonly UnityResourceProvider _resources  = new UnityResourceProvider();
        private static IDatabase database;
        private static DatabaseJsonSerializer _serializer = new DatabaseJsonSerializer();
        private static DatabaseBuilder builder = new DatabaseBuilder();

        public static Action Saved;

        static UnityDatabaseHost()
        {
            try
            {
                var bytes = _resources.GetResource(ResourceType.DatabaseJson);
                if (bytes.Length == 0)
                {
                    database = builder.Build();
                    Debug.LogError("Database not found, created new");
                    return;
                }

                database = _serializer.Deserialize(bytes);
                
                builder.Fetch(database);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void CreateNew()
        {
            database = builder.Build();
            Save();
        }

        public static void Save()
        {
            var bytes = _serializer.Serialize(database);
            _resources.SaveAs(ResourceType.DatabaseJson, bytes);
            
            Saved?.Invoke();
        }
        
        public static IDatabase Get()
        {
            return database;
        }
        
        public static Table GetTable(Type dataType)
        {
            return database.GetTables().FirstOrDefault(t => t.GetDataType() == dataType);
        }
        
        public static Table<T> GetOrCreateTable<T>() where T : TableData
        {
            var table = database.GetTable<T>();
            if (table != null)
            {
                return table;
            }

            table = new Table<T>();
            builder.BindTable(table);
            database = builder.Build();
            
            return table;
        }

        public static void AddOrReplaceTable<T>(Table<T> table) where T : TableData
        {
            builder.BindTable(table, true);
            database = builder.Build();
        }
        
        public static void AddTable(Table table)
        {
            builder.BindTable(table, false);
            database = builder.Build();
        }
        
        public static void AddSettings<T>(T settings) where T : Settings
        {
            builder.BindSettings(settings);
            database = builder.Build();
        }
    }
}