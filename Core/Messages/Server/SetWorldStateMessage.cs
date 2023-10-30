using Remouse.Core;
using Remouse.Serialization;
using Remouse.Shared.World.Commands;

namespace Remouse.Models.Messages
{
    public class SetWorldStateMessage : ServerWorldMessage
    {
        public PackedWorld PackedWorld { get; }

        public SetWorldStateMessage(PackedWorld packedWorld)
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

        public override WorldCommand AsCommand()
        {
            return new ApplyWorldStateCommand(PackedWorld);
        }
    }
}