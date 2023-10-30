using Remouse.Core.Components;
using Remouse.Core.World;

namespace Remouse.Core.ECS.Utils
{
    public static class WorldUtils
    {
        public static int? GetPlayerEntity(this IReadOnlyWorld world, long playerId)
        {
            var filter = world.Query().Inc<PlayerTag>().Inc<ReTransform>().Exc<MoveInDirection>().End();

            foreach (var playerEntityId in filter)
            {
                var playerTag = world.GetComponent<PlayerTag>(playerEntityId);
                if (playerTag.Value.playerId == playerId)
                {
                    return playerEntityId;
                }
            }

            return null;
        }
    }
}