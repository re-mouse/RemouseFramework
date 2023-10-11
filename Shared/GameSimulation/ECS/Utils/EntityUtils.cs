using Shared.EcsLib;

namespace Shared.GameSimulation.ECS.Utils
{
    public static class EntityUtils
    {
        public static ref T AddComponent<T>(EcsWorld world, int entity) where T : struct, IBaseComponent
        {
            var componentsPool = world.GetPool<T>();

            return ref componentsPool.Add(entity);
        }
        
        public static bool HasComponent<T>(EcsWorld world, int entity) where T : struct, IBaseComponent
        {
            var componentsPool = world.GetPool<T>();
            
            return componentsPool.Has(entity);
        }
        
        public static ref T GetComponent<T>(EcsWorld world, int entity) where T : struct, IBaseComponent
        {
            var componentsPool = world.GetPool<T>();
            
            return ref componentsPool.Get(entity);
        }
    }
}