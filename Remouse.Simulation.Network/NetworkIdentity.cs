    using Models;
using Remouse.Serialization;

namespace Remouse.Simulation.Network
{
    public struct NetworkIdentity : IBytesSerializableComponent
    {
        public int id;

        public void Serialize(IBytesWriter writer)
        {
            writer.WriteInt(id);
        }

        public void Deserialize(IBytesReader reader)
        {
            id = reader.ReadInt();
        }
    }
}