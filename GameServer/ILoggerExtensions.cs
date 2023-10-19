using Remouse.GameServer.ServerTransport;
using Remouse.Shared.Models;
using Remouse.Shared.Utils.Log;

namespace Remouse.GameServer
{
    public static class LoggerExtensions
    {
        public static void LogPlayerError(this ILogger logger, object sender, IPlayerConnection playerConnection, string error)
        {
            logger.LogError(sender, $"[PlayerId:{playerConnection.Data.cloudData.id}]: {error}");
        }
        
        public static void LogPlayerTrace(this ILogger logger, object sender, IPlayerConnection playerConnection, string message)
        {
            logger.LogTrace(sender, $"[PlayerId:{playerConnection.Data.cloudData.id}]: {message}");
        }
        
        public static void LogPlayerInfo(this ILogger logger, object sender, IPlayerConnection playerConnection, string error)
        {
            logger.LogInfo(sender, $"[PlayerId:{playerConnection.Data.cloudData.id}]: {error}");
        }
        
        public static void LogPlayerWarning(this ILogger logger, object sender, IPlayerConnection playerConnection, string error)
        {
            logger.LogWarning(sender, $"[PlayerId:{playerConnection.Data.cloudData.id}]: {error}");
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