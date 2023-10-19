using Remouse.Shared.Core;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Models.Messages
{
    public abstract class ServerWorldMessage : NetworkMessage
    {
        public long simulationTick;
        
        public override void Serialize(INetworkWriter writer)
        {
            writer.WriteLong(simulationTick);
        }

        public override void Deserialize(INetworkReader reader)
        {
            simulationTick = reader.ReadLong();
        }
        
        public abstract WorldCommand AsCommand();
    }
}