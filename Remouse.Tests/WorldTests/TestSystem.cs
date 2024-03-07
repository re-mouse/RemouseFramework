using Remouse.World;
using Shared.EcsLib.LeoEcsLite;
using Shared.Test.WorldTests;

namespace Shared.Test
{
    public class TestSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            var filter = world.Filter<TestComponent>().End();
            var pool = world.GetPoolOrCreate<TestComponent>();

            foreach (var entity in filter)
            {
                ref var testComponent = ref pool.Get(entity);
                testComponent.counter++;
            }
        }
    }
}