using Remouse.Core.Components;
using Remouse.Core.World;
using Remouse.MathLib;

namespace Remouse.Core.ECS.Systems
{
    public class WaypointSystem : IRunSystem
    {
        public void Run(IWorld world)
        {
            var query = world.Query().Inc<WaypointPath>().Inc<MovementSpeed>().Inc<ReTransform>().End();
        
            foreach (var entity in query)
            {
                ref WaypointPath waypointPath = ref world.GetComponentRef<WaypointPath>(entity);
                ref ReTransform transform = ref world.GetComponentRef<ReTransform>(entity);
                ref MovementSpeed speed = ref world.GetComponentRef<MovementSpeed>(entity);
            
                if (waypointPath.waypoints.Count > 0)
                {
                    Vec2 targetPosition = waypointPath.waypoints[0];
                    Vec2 direction = targetPosition - transform.position;
                    float distance = direction.magnitude;

                    if (distance > 0.01f)
                    {
                        direction /= distance;
                        transform.position += direction * speed.speed;
                    }
                    else
                    {
                        waypointPath.waypoints.RemoveAt(0);
                    }
                }
            }
        }
    }
}