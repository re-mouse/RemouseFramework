using Remouse.Shared.Core.ECS.Components;
using Remouse.Shared.Core.World;
using Remouse.Shared.EcsLib.LeoEcsLite;

namespace Remouse.Shared.Core.ECS.Systems
{
    public class PositionTeleportSystem : IRunSystem
    {
        public void Run(IWorld world)
        {
            var query = world.Query().Inc<PositionTeleport>().Inc<Transform>().End();
        
            foreach (var entity in query)
            {
                ref PositionTeleport teleport = ref world.GetComponentRef<PositionTeleport>(entity);
                ref Transform transform = ref world.GetComponentRef<Transform>(entity);

                transform.position = teleport.destinationPosition;
                world.DelComponent<Transform>(entity);
            }
        }
    }
}