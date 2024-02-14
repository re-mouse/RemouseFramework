using Remouse.World;
using Shared.Test.WorldTests;

namespace Shared.Test
{
    public class TestSystem : IRunSystem
    {
        public void Run(IWorld world)
        {
            var filter = world.Query().Inc<TestComponent>().End();

            foreach (var entity in filter)
            {
                ref var testComponent = ref world.GetComponentRef<TestComponent>(entity);
                testComponent.counter++;
            }
        }
    }
}