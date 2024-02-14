using System;

namespace Remouse.Utils
{
    public static class LLogger
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