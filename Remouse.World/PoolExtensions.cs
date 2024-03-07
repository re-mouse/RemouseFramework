using System.Runtime.CompilerServices;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.World
{
    public static class PoolExtensions
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static ref TComponent AddOrGet<TComponent>(this EcsPool<TComponent> pool, int entityId) where TComponent : struct
        {
            if (pool.Has(entityId))
            {
                return ref pool.Get(entityId);
            }
            else
            {
                return ref pool.Add(entityId);
            }
        }
        
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void AddOrSetRaw(this IEcsPool pool, int entityId, object raw)
        {
            if (pool.Has(entityId))
            {
                pool.SetRaw(entityId, raw);
            }
            else
            {
                pool.AddRaw(entityId, raw);
            }
        }
    }
}