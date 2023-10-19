using Remouse.Shared.Core.ECS.Components;
using Remouse.Shared.Core.World;

namespace Remouse.Shared.Core.ECS.Utils
{
    public static class WorldUtils
    {
        public static int? GetPlayerEntity(this IReadOnlyWorld world, long playerId)
        {
            var filter = world.Query().Inc<PlayerTag>().Inc<Transform>().Exc<MovementDirection>().End();

            foreach (var playerEntityId in filter)
            {
                var playerTag = world.GetComponent<PlayerTag>(playerEntityId);
                if (playerTag.playerId == playerId)
                {
                    return playerEntityId;
                }
            }

            return null;
        }
    }
}