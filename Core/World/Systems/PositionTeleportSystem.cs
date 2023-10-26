using Remouse.Core.Components;
using Remouse.Core.World;

namespace Remouse.Core.ECS.Systems
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