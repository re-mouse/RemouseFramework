using System;

namespace Infrastructure
{
    [Serializable]
    public class ProjectConfiguration
    {
        public int ticksInSecond;
        public BuildType buildType;
        public Platform platform;
        public IResources folderResourcesProvider;
    }
}