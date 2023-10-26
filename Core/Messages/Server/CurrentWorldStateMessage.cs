using Remouse.Serialization;

namespace Remouse.Models.Messages
{
    public class CurrentWorldStateMessage : NetworkMessage
    {
        public WorldState worldState { get; }

        public CurrentWorldStateMessage(WorldState worldState)
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
    }
}