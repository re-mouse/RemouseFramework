using System;

namespace Shared.Utils.Log
{
    public interface ILogger
    {
        public void LogError(object sender, string error);
        public void LogInfo(object sender, string info);
        public void LogWarning(object sender, string warning);
        public void LogTrace(object sender, string trace);
        public void LogException(object sender, Exception exception, string message = "");
    }
}