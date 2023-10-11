using Shared.EcsLib;
using Shared.GameSimulation.ECS.Components;
using Shared.Math;

namespace Shared.GameSimulation
{
    public static class GameSimulationExtensions
    {
        public static Transform GetEntityTransform(this GameSimulation gameSimulation, int entityId)
        {
            var transformPool = gameSimulation.World.GetPool<Transform>();
            return transformPool.Get(entityId);
        }
        
        public static Vec2 GetEntityPosition(this GameSimulation gameSimulation, int entityId)
        {
            var transformPool = gameSimulation.World.GetPool<Transform>();
            return transformPool.Get(entityId).position;
        }
        
        public static int[] GetSyncedEntities(this GameSimulation gameSimulation)
        {
            var filter = gameSimulation.World.Filter<NetworkIdentity>().Inc<Transform>().End();
            return filter.GetRawEntities();
        }
        
        public static ref T GetEntityComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();
            return ref pool.Get(entity);
        }
    }
}