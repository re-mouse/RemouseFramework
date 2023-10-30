using Remouse.Core;
using Remouse.Core.World;
using Remouse.Serialization;
using Remouse.Core.ECS.Utils;

namespace Remouse.Shared.GameSimulation.Commands
{
    public class DestroyPlayerCommand : WorldCommand
    {
        public long playerId;

        public override void Run(IWorld ecsWorld)
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