using System;

namespace Shared.Utils.Log
{
    public class ConsoleLogger : ILogger
    {
        public void LogError(object sender, string error)
        {
            Console.WriteLine($"{sender} - Error: {error}");
        }

        public void LogInfo(object sender, string info)
        {
            Console.WriteLine($"{sender} - Info: {info}");

        }

        public void LogWarning(object sender, string warning)
        {
            Console.WriteLine($"{sender} - Warning: {warning}");
        }

        public void LogTrace(object sender, string trace)
        {
            Console.WriteLine($"{sender} - Trace: {trace}");
        }

        public void LogException(object sender, Exception exception, string message)
        {
            Console.WriteLine($"{sender} - Message {message}. Exception: {exception.Message}.");
        }
    }
}