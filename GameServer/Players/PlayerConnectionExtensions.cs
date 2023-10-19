using Remouse.GameServer.ServerTransport;
using Remouse.Shared.Core.ECS.Utils;
using Remouse.Shared.Core.World;

namespace Remouse.GameServer.Utils
{
    public static class PlayerConnectionExtensions
    {
        public static long? GetPlayerId(this IPlayerConnection playerConnection)
        {
            return playerConnection?.Data?.cloudData?.id;
        }
        
        public static int? GetPlayerEntityId(this IPlayerConnection playerConnection, IReadOnlyWorld world)
        {
            var playerId = playerConnection.GetPlayerId();
            return playerId != null ? world.GetPlayerEntity(playerId.Value) : null;
        }
    }
}