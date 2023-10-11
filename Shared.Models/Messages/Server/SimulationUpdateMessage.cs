using Shared.Online.Models;
using Shared.Serialization;

namespace Shared.Online.Commands
{
    public class SimulationUpdateMessage : NetworkMessage
    {
        public SimulationChanges simulationChanges { get; }
        
        public SimulationUpdateMessage(SimulationChanges changes)
        {
            simulationChanges = changes;
        }

        public override void Serialize(INetworkWriter writer)
        {
            simulationChanges.Serialize(writer);
        }

        public override void Deserialize(INetworkReader reader)
        {
            simulationChanges.Deserialize(reader);
        }
    }
}