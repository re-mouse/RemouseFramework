using Remouse.GameServer.ServerTransport;
using Remouse.Models;
using Remouse.Shared.Utils.Log;

namespace Remouse.GameServer
{
    public static class LoggerExtensions
    {
        public static void LogPlayerError(this ILogger logger, object sender, IPlayer player, string error)
        {
            logger.LogError(sender, $"[PlayerId:{player.Data.cloudData.id}]: {error}");
        }
        
        public static void LogPlayerTrace(this ILogger logger, object sender, IPlayer player, string message)
        {
            logger.LogTrace(sender, $"[PlayerId:{player.Data.cloudData.id}]: {message}");
        }
        
        public static void LogPlayerInfo(this ILogger logger, object sender, IPlayer player, string error)
        {
            logger.LogInfo(sender, $"[PlayerId:{player.Data.cloudData.id}]: {error}");
        }
        
        public static void LogPlayerWarning(this ILogger logger, object sender, IPlayer player, string error)
        {
            logger.LogWarning(sender, $"[PlayerId:{player.Data.cloudData.id}]: {error}");
        }
        
        public static void LogPlayerError(this ILogger logger, object sender, PlayerData playerData, string error)
        {
            logger.LogError(sender, $"[PlayerId:{playerData.cloudData.id}]: {error}");
        }
        
        public static void LogPlayerTrace(this ILogger logger, object sender, PlayerData playerData, string message)
        {
            logger.LogTrace(sender, $"[PlayerId:{playerData.cloudData.id}]: {message}");
        }
        
        public static void LogPlayerInfo(this ILogger logger, object sender, PlayerData playerData, string error)
        {
            logger.LogInfo(sender, $"[PlayerId:{playerData.cloudData.id}]: {error}");
        }
        
        public static void LogPlayerWarning(this ILogger logger, object sender, PlayerData playerData, string error)
        {
            logger.LogWarning(sender, $"[PlayerId:{playerData.cloudData.id}]: {error}");
        }
    }
}