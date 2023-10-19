using Remouse.GameServer.ServerTransport;
using Remouse.Core.ECS.Utils;
using Remouse.Core.World;

namespace Remouse.GameServer.Utils
{
    public static class PlayerConnectionExtensions
    {
        public static long? GetPlayerId(this IPlayer player)
        {
            return player?.Data?.cloudData?.id;
        }
        
        public static int? GetPlayerEntityId(this IPlayer player, IReadOnlyWorld world)
        {
            var playerId = player.GetPlayerId();
            return playerId != null ? world.GetPlayerEntity(playerId.Value) : null;
        }
    }
}