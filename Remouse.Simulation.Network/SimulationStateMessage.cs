using Remouse.Network.Models;
using Remouse.Serialization;

namespace Remouse.Simulation.Network
{
    public class SimulationStateMessage : NetworkMessage
    {
        public WorldState WorldState { get; private set; }
        public long Tick { get; private set; }
        public int LastNetworkId { get; private set; }

        public SimulationStateMessage()
        {
        }

        public SimulationStateMessage(WorldState worldState, long tick, int lastNetworkId)
        {
            WorldState = worldState;
            Tick = tick;
            LastNetworkId = lastNetworkId;
        }

        public override void Serialize(IBytesWriter writer)
        {
            WorldState.Serialize(writer);
            writer.WriteLong(Tick);
            writer.WriteInt(LastNetworkId);
        }

        public override void Deserialize(IBytesReader reader)
        {
            WorldState = new WorldState();
            WorldState.Deserialize(reader);
            Tick = reader.ReadLong();
            LastNetworkId = reader.ReadInt();
        }
    }
}