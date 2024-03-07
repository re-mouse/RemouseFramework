using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    public class EntityCreationEventsClearSystem : IEcsPostRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<CreatedEntityEvent> _createdPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<CreatedEntityEvent>().End();
            _createdPool = _world.GetPoolOrCreate<CreatedEntityEvent>();
        }
        
        public void PostRun(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _createdPool.Del(entity);
            }
        }
    }
}