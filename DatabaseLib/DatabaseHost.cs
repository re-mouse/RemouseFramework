using Remouse.DIContainer;
using Remouse.Infrastructure;

namespace Remouse.DatabaseLib
{
    public class DatabaseHost
    {
        private Database _database;

        public Database Get()
        {
            return _database;
        }

        public void Set(Database database)
        {
            _database = database;
        }
    }
}