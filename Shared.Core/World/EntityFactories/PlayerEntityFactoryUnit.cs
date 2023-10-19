using Remouse.Shared.Core.ECS.Components;
using Remouse.Shared.Core.World;
using Remouse.Shared.Math;

namespace Remouse.Shared.Core.Factories
{
    public class PlayerEntityFactory
    {
        public static int Create(IWorld world)
        {
            int entity = world.NewEntity();

            ref var transform = ref world.AddComponent<Transform>(entity);
            transform.position = Vec2.zero;
            
            ref var moveSpeed = ref world.AddComponent<MovementSpeed>(entity);
            moveSpeed.speed = 5f;
            
            ref var moveDirection = ref world.AddComponent<MovementDirection>(entity);
            moveDirection.movementDirection = Vec2.one;

            ref var networkIdentity = ref world.AddComponent<NetworkIdentity>(entity);
            networkIdentity.visibility = NetworkVisibility.Default;
            
            world.AddComponent<PlayerTag>(entity);
            
            return entity;
        }
    }
}