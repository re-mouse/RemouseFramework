using Remouse.Core.Components;
using Remouse.Core.Factories;
using Remouse.Core.World;
using Remouse.Serialization;

namespace Remouse.Core
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