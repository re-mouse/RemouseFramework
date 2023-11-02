using Remouse.Serialization;

namespace Remouse.Models.Messages
{
    public class SimulationInitialMessage : NetworkMessage
    {
        public PackedWorld PackedWorld { get; }

        public SimulationInitialMessage(PackedWorld packedWorld)
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