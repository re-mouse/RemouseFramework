using Remouse.Serialization;

namespace Remouse.Models.Messages
{
    public class CurrentWorldStateMessage : NetworkMessage
    {
        public PackedWorld PackedWorld { get; }

        public CurrentWorldStateMessage(PackedWorld packedWorld)
        {
            this.PackedWorld = packedWorld;
        }

        public override void Serialize(INetworkWriter writer)
        {
            PackedWorld.Serialize(writer);
        }

        public override void Deserialize(INetworkReader reader)
        {
            PackedWorld.Deserialize(reader);
        }
    }
}