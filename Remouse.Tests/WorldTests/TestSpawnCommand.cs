using Remouse.Simulation;
using Remouse.World;
using Remouse.Database;
using ReDI;
using Remouse.Serialization;
using Shared.EcsLib.LeoEcsLite;

namespace Shared.Test.WorldTests
{
    public class TestSpawnCommand : BaseWorldCommand
    {
        public long startingSpawnCounter;

        public override void Run(EcsWorld world)
        {
            var entity = world.NewEntity();
            var pool = world.GetPoolOrCreate<TestComponent>();
            ref var component = ref pool.Add(entity);
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