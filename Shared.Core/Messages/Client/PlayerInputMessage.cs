using Remouse.Shared.Models;
using Remouse.Shared.Models.Messages;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core.Input
{
    public abstract class PlayerInputMessage : NetworkMessage
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
        
        public abstract WorldCommand AsCommand(PlayerData playerData);
    }
}