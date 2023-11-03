using System;
using NUnit.Framework;
using Remouse.Shared.Utils.Log;

namespace Shared.Test
{
    public class TestLogger : ILogger
    {
        public void LogError(object sender, string error)
        {
            TestContext.Out.WriteLine(error);
        }

        public void LogInfo(object sender, string info)
        {
            TestContext.Out.WriteLine(info);
        }

        public void LogWarning(object sender, string warning) 
        {
            TestContext.Out.WriteLine(warning);
        }

        public void LogTrace(object sender, string trace)
        {
            TestContext.Out.WriteLine(trace);
        }

        public void LogException(object sender, Exception exception, string message = "") 
        {
            TestContext.Out.WriteLine(message);
        }
    }
}