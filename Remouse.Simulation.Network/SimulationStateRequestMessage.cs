using Remouse.Network.Models;
using Remouse.Serialization;

namespace Remouse.Simulation.Network
{
    public class SimulationStateRequestMessage : NetworkMessage
    {
        public override void Serialize(IBytesWriter writer)
        {
        }

        public override void Deserialize(IBytesReader reader)
        {
            
        }
    }
}