using GameServer.ServerTransport;
using Shared.Online.Models;
using Shared.Utils.Log;

namespace GameServer
{
    public static class ILoggerExtensions
    {
        public static void LogPlayerError(this ILogger logger, object sender, IPlayerConnection playerConnection, string error)
        {
            logger.LogError(sender, $"[PlayerId:{playerConnection.data.saveData.id}]: {error}");
        }
        
        public static void LogPlayerTrace(this ILogger logger, object sender, IPlayerConnection playerConnection, string message)
        {
            logger.LogTrace(sender, $"[PlayerId:{playerConnection.data.saveData.id}]: {message}");
        }
        
        public static void LogPlayerInfo(this ILogger logger, object sender, IPlayerConnection playerConnection, string error)
        {
            logger.LogInfo(sender, $"[PlayerId:{playerConnection.data.saveData.id}]: {error}");
        }
        
        public static void LogPlayerWarning(this ILogger logger, object sender, IPlayerConnection playerConnection, string error)
        {
            logger.LogWarning(sender, $"[PlayerId:{playerConnection.data.saveData.id}]: {error}");
        }
        
        public static void LogPlayerError(this ILogger logger, object sender, PlayerData playerData, string error)
        {
            logger.LogError(sender, $"[PlayerId:{playerData.saveData.id}]: {error}");
        }
        
        public static void LogPlayerTrace(this ILogger logger, object sender, PlayerData playerData, string message)
        {
            logger.LogTrace(sender, $"[PlayerId:{playerData.saveData.id}]: {message}");
        }
        
        public static void LogPlayerInfo(this ILogger logger, object sender, PlayerData playerData, string error)
        {
            logger.LogInfo(sender, $"[PlayerId:{playerData.saveData.id}]: {error}");
        }
        
        public static void LogPlayerWarning(this ILogger logger, object sender, PlayerData playerData, string error)
        {
            logger.LogWarning(sender, $"[PlayerId:{playerData.saveData.id}]: {error}");
        }
    }
}