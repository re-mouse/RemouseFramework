using System;
using UnityEngine;
using ILogger = Remouse.Utils.ILogger;

namespace Remouse.UnityEngine.Assets
{
    public class UnityLogger : ILogger
    {
        public void LogError(object sender, string error)
        {
            Debug.LogError($"[ERROR][{sender?.GetType().Name}] {error}");
        }

        public void LogInfo(object sender, string info)
        {
            Debug.Log($"[INFO][{sender?.GetType().Name}] {info}");
        }

        public void LogWarning(object sender, string warning)
        {
            Debug.LogWarning($"[WARNING][{sender?.GetType().Name}] {warning}");
        }

        public void LogTrace(object sender, string trace)
        {
            Debug.Log($"[TRACE][{sender?.GetType().Name}] {trace}");
        }

        public void LogException(object sender, Exception exception, string message = "")
        {
            Debug.LogException(exception);
        }
    }
}