using System.Collections.Generic;
using Shared.DIContainer;
using Shared.EcsLib;

namespace Shared.GameSimulation.Factories
{
    public class EntityFactory
    {
        private List<IEntityFactoryUnit> _factories;
        private Container _container;

        private Dictionary<string, IEntityFactoryUnit> _factoriesById = new Dictionary<string, IEntityFactoryUnit>();

        public void Construct(Container container)
        {
            _container = container;
            
            _factories = new List<IEntityFactoryUnit>();
            _container.GetAll(_factories);

            foreach (var factoryUnit in _factories)
            {
                FetchFactoryEntityIds(factoryUnit);
            }
        }

        private void FetchFactoryEntityIds(IEntityFactoryUnit factoryUnit)
        {
            foreach (var factoryEntityId in factoryUnit.GetEntityIds())
            {
                _factoriesById[factoryEntityId] = factoryUnit;
            }
        }

        public int Create(EcsWorld world, string entityId)
        {
            if (!_factoriesById.TryGetValue(entityId, out var factoryUnit))
                return 0;
            
            var entity = factoryUnit.Create(world, entityId);
            
            return entity;
        }
    }
}