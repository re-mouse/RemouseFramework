using System;

namespace Remouse.Shared.Utils.Log
{
    public class DummyLogger : ILogger
    {
        public void LogError(object sender, string error) {  }

        public void LogInfo(object sender, string info) { }

        public void LogWarning(object sender, string warning) { }

        public void LogTrace(object sender, string trace) { }

        public void LogException(object sender, Exception exception, string message = "") { }
    }
}