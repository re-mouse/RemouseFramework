using Remouse.DIContainer;

namespace Remouse.Database
{
    public class DatabaseProvider
    {
        private Database _database;
        
        public Database Get()
        {
            if (_database == null)
            {
                _database = DatabaseSerializer.DeserializeDatabase(DatabaseJson.Path);
            }

            return _database;
        }
        
        public Database Get(string jsonPath)
        {
            if (_database == null)
            {
                _database = DatabaseSerializer.DeserializeDatabase(jsonPath);
            }

            return _database;
        }
    }
}