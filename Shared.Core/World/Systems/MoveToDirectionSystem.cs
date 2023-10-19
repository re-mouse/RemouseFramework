using Remouse.Shared.Core.ECS.Components;
using Remouse.Shared.Core.World;
using Remouse.Shared.Math;
using Remouse.Shared.EcsLib.LeoEcsLite;

namespace Remouse.Shared.Core.ECS.Systems
{
    public class MoveToDirectionSystem : IRunSystem
    {
        public void Run(IWorld world)
        {
            var query = world.Query().Inc<MovementDirection>().Inc<MovementSpeed>().Inc<Transform>().End();
            
            foreach (var entity in query)
            {
                ref MovementDirection direction = ref world.GetComponentRef<MovementDirection>(entity);
                ref Transform transform = ref world.GetComponentRef<Transform>(entity);
                ref MovementSpeed speed = ref world.GetComponentRef<MovementSpeed>(entity);
                
                if (direction.movementDirection == Vec2.zero)
                    continue;

                transform.position += direction.movementDirection.Normalize() * speed.speed * (1 / Simulation.TicksInSecond);
            }
        }

    }
}

