using Remouse.Serialization;

namespace Remouse.Simulation
{
    public struct CreatedEntityEvent : IBytesSerializableComponent
    {
        public void Serialize(IBytesWriter writer) {  }

        public void Deserialize(IBytesReader reader) { }
    }
}