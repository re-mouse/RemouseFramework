using ReDI;
using Remouse.World;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation.Network
{
    public class NetworkIdentityAssignerSystem : IEcsInitSystem, IEcsRunSystem
    {
        [Inject] private NetworkEntityRegistry _entityRegistry;
        
        private EcsWorld _world;
        private EcsPool<NetworkIdentity> _pool;
        private EcsFilter _createdEntitysFilter;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _createdEntitysFilter = _world.Filter<NetworkIdentity>().Inc<CreatedEntityEvent>().End();
            _pool = _world.GetPoolOrCreate<NetworkIdentity>();

            foreach (var entity in _createdEntitysFilter)
            {
                var identity = _pool.Get(entity);
                _entityRegistry.SetEntity(entity, identity.id);
            }
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entityId in _createdEntitysFilter)
            {
                ref var networkIdentity = ref _pool.Get(entityId);

                networkIdentity.id = _entityRegistry.AssignNetworkId(entityId);
            }
        }
    }
}