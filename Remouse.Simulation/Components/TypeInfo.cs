using Models;
using Remouse.Serialization;

namespace Remouse.Simulation
{
    public struct TypeInfo : IBytesSerializableComponent
    {
        public string typeId;

        public void Serialize(IBytesWriter writer)
        {
            writer.WriteString(typeId);
        }

        public void Deserialize(IBytesReader reader)
        {
            typeId = reader.ReadString();
        }
    }
}