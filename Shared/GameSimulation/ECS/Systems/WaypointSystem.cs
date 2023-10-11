using Shared.EcsLib;
using Shared.GameSimulation.ECS.Components;
using Shared.Math;

namespace Shared.GameSimulation.ECS.Systems
{
    public class WaypointSystem : IEcsSystem, IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld ();
            var filter = world.Filter<WaypointPath>().Inc<MovementSpeed>().Inc<Transform>().End();

            var waypointsPathPool = world.GetPool<WaypointPath>();
            var positionPool = world.GetPool<Transform>();
            var speedPool = world.GetPool<MovementSpeed>();
            
            foreach (var entity in filter)
            {
                ref WaypointPath waypointPath = ref waypointsPathPool.Get(entity);
                ref Transform transform = ref positionPool.Get(entity);
                ref MovementSpeed movementSpeed = ref speedPool.Get(entity);
                
                if (waypointPath.waypoints.Count > 0)
                {
                    Vec2 targetPosition = waypointPath.waypoints[0];
                    Vec2 direction = targetPosition - transform.position;
                    float distance = direction.magnitude;

                    if (distance > 0.01f)
                    {
                        direction /= distance;
                        transform.position += direction * movementSpeed.speed;
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