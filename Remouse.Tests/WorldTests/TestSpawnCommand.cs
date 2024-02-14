using Remouse.Simulation;
using Remouse.World;
using Remouse.Database;
using Remouse.DI;
using Remouse.Serialization;

namespace Shared.Test.WorldTests
{
    public class TestSpawnCommand : BaseWorldCommand
    {
        public long startingSpawnCounter;

        public override void Run(IWorld world)
        {
            var entity = world.NewEntity();
            ref var component = ref world.AddComponent<TestComponent>(entity);
            component.counter = startingSpawnCounter;
        }

        public override void Serialize(IBytesWriter writer)
        {
            writer.WriteLong(startingSpawnCounter);
        }

        public override void Deserialize(IBytesReader reader)
        {
            startingSpawnCounter = reader.ReadLong();
        }
    }
}