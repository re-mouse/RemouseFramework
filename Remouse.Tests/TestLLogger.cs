using System;
using NUnit.Framework;
using Remouse.Utils;

namespace Shared.Test
{
    public class TestLLogger : ILogger
    {
        public void LogError(object sender, string error)
        {
            TestContext.Out.WriteLine(Format("Error", $"{sender.GetType().Name}: {error}"));
        }

        public void LogInfo(object sender, string info)
        {
            TestContext.Out.WriteLine(Format("Info", $"{sender.GetType().Name}: {info}"));
        }

        public void LogWarning(object sender, string warning) 
        {
            TestContext.Out.WriteLine(Format("Warning", $"{sender.GetType().Name}: {warning}"));
        }

        public void LogTrace(object sender, string trace)
        {
            TestContext.Out.WriteLine(Format("Trace", $"{sender.GetType().Name}: {trace}"));
        }

        public void LogException(object sender, Exception exception, string message = "") 
        {
            TestContext.Out.WriteLine(Format("Exception", $"{sender.GetType().Name}: {message}. Exception: {exception}"));
        }

        private string Format(string level, string message)
        {
            return $"{DateTime.Now.ToLongTimeString()} [{level.ToUpper()}] {message}";
        }
    }
}