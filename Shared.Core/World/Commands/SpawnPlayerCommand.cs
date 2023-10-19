using Remouse.Shared.Core.ECS.Components;
using Remouse.Shared.Core.Factories;
using Remouse.Shared.Core.World;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Core
{
    public class SpawnPlayerCommand : WorldCommand
    {
        public long playerId;
        
        public override void Apply(IWorld ecsWorld)
        {
            int entityId = PlayerEntityFactory.Create(ecsWorld);
            ref var playerTag = ref ecsWorld.GetComponentRef<PlayerTag>(entityId);
            
            playerTag.playerId = playerId;
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