using Remouse.DIContainer;
using Remouse.Infrastructure;
using UnityEngine;
using Logger = Remouse.Shared.Utils.Log.Logger;

namespace Remouse.DatabaseLib
{
    public class DatabaseHost
    {
        private Database _database;
        private IResources _resources;
        private DatabaseJsonSerializer _serializer;

        public void Construct(Container container)
        {
            _resources = container.Resolve<IResources>();
            _serializer = container.Resolve<DatabaseJsonSerializer>();
        }

        public Database Get()
        {
            return _database;
        }

        public void LoadFromResources()
        {
            if (_resources == null)
            {
                Logger.Current.LogError(this, "Add resources in container, cannot load database");
                return;
            }

            var bytes = _resources.GetResource(ResourceType.DatabaseJson);
            if (bytes == null || bytes.Length == 0)
            {
                Logger.Current.LogError(this, "Given resource empty for database");
                return;
            }

            _database = _serializer.DeserializeFromBytes(bytes);
        }

        public void Set(Database database)
        {
            _database = database;
        }
    }
}