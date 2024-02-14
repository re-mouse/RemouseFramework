using Remouse.Utils;

namespace Remouse.Network.Server
{
    public static class ILoggerExtensions
    {
        public static void LogPlayerError(this ILogger logger, object sender, IPlayer player, string error)
        {
            logger.LogError(sender, $"[PlayerId:{player.Identity.id}]: {error}");
        }
        
        public static void LogPlayerTrace(this ILogger logger, object sender, IPlayer player, string message)
        {
            logger.LogTrace(sender, $"[PlayerId:{player.Identity.id}]: {message}");
        }
        
        public static void LogPlayerInfo(this ILogger logger, object sender, IPlayer player, string error)
        {
            logger.LogInfo(sender, $"[PlayerId:{player.Identity.id}]: {error}");
        }
        
        public static void LogPlayerWarning(this ILogger logger, object sender, IPlayer player, string error)
        {
            logger.LogWarning(sender, $"[PlayerId:{player.Identity.id}]: {error}");
        }
    }
}