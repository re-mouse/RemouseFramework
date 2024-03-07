using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    public class EntityDestroySystem : IEcsPostRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<EntityDestroyRequest>().End();
        }
        
        public void PostRun(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _world.DelEntity(entity);
            }
        }

    }
}