using Shared.EcsLib;
using Shared.GameSimulation.ECS.Components;

namespace Shared.GameSimulation.ECS.Systems
{
    public class PositionTeleportSystem : IEcsSystem, IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<PositionTeleport>().Inc<Transform>().End();

            var moveToPositionTagPool = world.GetPool<PositionTeleport>();
            var positionPool = world.GetPool<Transform>();

            foreach (var entity in filter)
            {
                ref var moveToPositionTagRef = ref moveToPositionTagPool.Get(entity);
                ref var transform = ref positionPool.Get(entity);

                transform.position = moveToPositionTagRef.DestinationPosition;
                positionPool.Del(entity);
            }
        }
    }
}