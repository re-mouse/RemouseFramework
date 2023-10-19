using Remouse.Models;
using Remouse.Models.Messages;
using Remouse.Serialization;

namespace Remouse.Core.Input
{
    public abstract partial class PlayerInputMessage : NetworkMessage
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