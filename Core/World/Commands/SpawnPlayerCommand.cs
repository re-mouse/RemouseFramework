using Remouse.Core.Components;
using Remouse.Core.Configs;
using Remouse.Core.World;
using Remouse.DatabaseLib;
using Remouse.Models.Database;
using Remouse.Serialization;

namespace Remouse.Core
{
    public class SpawnPlayerCommand : WorldCommand
    {
        public long playerId;
        
        public override void Run(IWorld ecsWorld, Database database)
        {
            var playerSettings = database.GetSetting<PlayerSettings>();
            var entity = playerSettings.playerEntityId.GetTableData(database).CreateEntity(ecsWorld);
            var playerTag = ecsWorld.GetComponentRef<PlayerTag>(entity);
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