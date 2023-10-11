using System;

namespace Shared.Utils.Log
{
    public static class Logger
    {
        public static ILogger Current { get; private set; } = new DummyLogger();

        public static void SetLogger(ILogger logger)
        {
            if (logger == null)
                throw new NullReferenceException("Logger is null");
            
            Current = logger;
        }
    }
}