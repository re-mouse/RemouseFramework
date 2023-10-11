using Shared.EcsLib;

namespace Shared.GameSimulation.Factories
{
    public interface IEntityFactoryUnit
    {
        public int Create(EcsWorld world, string entityId);
        public string[] GetEntityIds();
    }
}