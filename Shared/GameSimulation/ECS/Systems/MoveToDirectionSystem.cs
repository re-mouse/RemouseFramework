using Shared.EcsLib;
using Shared.GameSimulation.ECS.Components;
using Shared.Math;

namespace Shared.GameSimulation.ECS.Systems
{
    public class MoveToDirectionSystem : IEcsSystem, IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<MovementDirection>().Inc<MovementSpeed>().Inc<Transform>().End();
            
            var directionsPool = world.GetPool<MovementDirection>();
            var positionPool = world.GetPool<Transform>();
            var speedPool = world.GetPool<MovementSpeed>();

            foreach (var entity in filter)
            {
                ref MovementDirection direction = ref directionsPool.Get(entity);
                ref Transform transform = ref positionPool.Get(entity);
                ref MovementSpeed speed = ref speedPool.Get(entity);
                
                if (direction.movementDirection == Vec2.zero)
                    continue;

                transform.position += direction.movementDirection.Normalize() * speed.speed * (1 / GameSimulation.ticksInSecond);
            }
        }

    }
}

