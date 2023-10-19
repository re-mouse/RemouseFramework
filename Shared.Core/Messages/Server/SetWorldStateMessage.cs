using Remouse.Shared.Core;
using Remouse.Shared.Serialization;
using Remouse.Shared.World.Commands;

namespace Remouse.Shared.Models.Messages
{
    public class SetWorldStateMessage : ServerWorldMessage
    {
        public WorldState worldState { get; }

        public SetWorldStateMessage(WorldState worldState)
        {
            this.worldState = worldState;
        }

        public override void Serialize(INetworkWriter writer)
        {
            worldState.Serialize(writer);
        }

        public override void Deserialize(INetworkReader reader)
        {
            worldState.Deserialize(reader);
        }

        public override WorldCommand AsCommand()
        {
            return new ApplyWorldStateCommand(worldState);
        }
    }
}