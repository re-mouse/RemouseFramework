using Remouse.DI;
using Remouse.World;

namespace Remouse.Simulation.Network
{
    public class NetworkIdentityAssignerSystem : IInitSystem, IRunSystem
    {
        private NetworkEntityRegistry _entityRegistry;

        public void Construct(Container container)
        {
            _entityRegistry = container.Resolve<NetworkEntityRegistry>();
        }
        
        public void Run(IWorld world)
        {
            var filter = world.Query().Inc<NetworkIdentity>().Inc<CreatedEntityEvent>().End();
            
            foreach (var entityId in filter)
            {
                ref var networkIdentity = ref world.GetComponentRef<NetworkIdentity>(entityId);

                networkIdentity.id = _entityRegistry.AssignNetworkId(entityId);
            }
        }

        public void Init(IWorld world)
        {
            var filter = world.Query().Inc<NetworkIdentity>().End();

            foreach (var entity in filter)
            {
                var identity = world.GetComponent<NetworkIdentity>(entity);
                _entityRegistry.SetEntity(entity, identity.id);
            }
        }
    }
}