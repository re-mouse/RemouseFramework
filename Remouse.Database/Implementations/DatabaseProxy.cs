using ReDI;
using Infrastructure;

namespace Remouse.Database
{
    internal class DatabaseProxy : IDatabase
    {
        [Inject] private IResources _resources;
        [Inject] private IDatabaseSerializer _serializer;
        
        private IDatabase _database;

        private void EnsureLoaded()
        {
            if (_database != null)
                return;

            var databaseBytes = _resources.GetResource(ResourceType.DatabaseJson);
            _database = _serializer.Deserialize(databaseBytes);
        }

        public T GetSetting<T>() where T : Settings
        {
            EnsureLoaded();

            return _database.GetSetting<T>();
        }

        public Table<T> GetTable<T>() where T : TableData
        {
            EnsureLoaded();

            return _database.GetTable<T>();
        }

        public T GetTableData<T>(string id) where T : TableData
        {
            EnsureLoaded();

            return _database.GetTableData<T>(id);
        }

        public Settings[] GetSettings()
        {
            EnsureLoaded();

            return _database.GetSettings();
        }

        public Table[] GetTables()
        {
            EnsureLoaded();

            return _database.GetTables();
        }
    }
}