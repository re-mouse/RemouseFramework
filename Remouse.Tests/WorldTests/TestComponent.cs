using Remouse.Serialization;
using Remouse.Simulation;

namespace Shared.Test.WorldTests
{
    public struct TestComponent : IBytesSerializableComponent
    {
        public long counter;
        
        public void Serialize(IBytesWriter writer)
        {
            writer.WriteLong(counter);
        }

        public void Deserialize(IBytesReader reader)
        {
            counter = reader.ReadLong();
        }
    }
}