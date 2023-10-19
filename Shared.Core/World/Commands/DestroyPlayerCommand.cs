using Remouse.Shared.Core;
using Remouse.Shared.Core.World;
using Remouse.Shared.Serialization;
using Remouse.Shared.Core.ECS.Utils;

namespace Remouse.Shared.GameSimulation.Commands
{
    public class DestroyPlayerCommand : WorldCommand
    {
        public long playerId;

        public override void Apply(IWorld ecsWorld)
        {
            var playerEntityId = ecsWorld.GetPlayerEntity(playerId);
            
            if (playerEntityId != null)
                ecsWorld.DelEntity(playerEntityId.Value);
        }

        public override void Serialize(INetworkWriter writer)
        {
            writer.WriteLong(playerId);
        }

        public override void Deserialize(INetworkReader reader)
        {
            playerId = reader.ReadLong();
        }
    }
}