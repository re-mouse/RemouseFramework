using Shared.EcsLib.LeoEcsLite;

namespace Remouse.World
{
    public static class WorldExtensions
    {
        public static ref TComponent GetComponent<TComponent>(this EcsWorld world, int entityId) where TComponent : struct
        {
            var pool = world.GetPoolOrCreate<TComponent>();
            return ref pool.Get(entityId);
        }
    }
}