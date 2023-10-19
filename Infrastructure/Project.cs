using System;

namespace Remouse.Infrastructure
{
    public static class Project
    {
        public static BuildType Build { get; private set; }
        public static Platform Platform { get; private set; }
        private static bool isInitialized;

        public static void Initialize(ProjectConfiguration configuration)
        {
            if (isInitialized && configuration.buildType == BuildType.Debug)
                throw new Exception("Double project initialization");

            Build = configuration.buildType;
            Platform = configuration.platform;
            
            isInitialized = true;
        }
    }

    public enum Platform
    {
        Fallback,
        Mobile,
        Desktop
    }
}