using System;

namespace Remouse.Shared.Infrastructure
{
    public static class Project
    {
        public static BuildType Build { get; private set; }
        private static bool isInitialized;

        public static void Initialize(ProjectConfiguration configuration)
        {
            if (isInitialized && configuration.buildType == BuildType.Debug)
                throw new Exception("Double project initialization");

            Build = configuration.buildType;
            
            isInitialized = true;
        }
    }
}