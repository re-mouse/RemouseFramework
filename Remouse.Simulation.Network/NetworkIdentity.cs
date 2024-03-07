    using Models;
using Remouse.Serialization;

namespace Remouse.Simulation.Network
{
    public struct NetworkIdentity : IBytesSerializableComponent
    {
        public long playerOwnerId;
        public bool isPlayerOwned;
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