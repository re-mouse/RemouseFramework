using Remouse.World;

namespace Remouse.Simulation
{
    public static class WorldExtensions
    {
        public static ref T GetOrAddComponent<T>(this IWorld world, int entity) where T : struct, IComponent
        {
            if (world.HasComponent<T>(entity))
            {
                return ref world.GetComponentRef<T>(entity);
            }
            else
            {
                return ref world.AddComponent<T>(entity);
            }
        }
    }
}