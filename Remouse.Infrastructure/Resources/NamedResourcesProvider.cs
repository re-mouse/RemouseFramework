using System;
using System.IO;

namespace Infrastructure
{
    public class NamedResourcesProvider : IResources
    {
        public const string DatabaseRelativePath = "/database.json";
        private readonly string _resourcesFolder;

        public NamedResourcesProvider(string resourcesFolder)
        {
            _resourcesFolder = resourcesFolder;
        }
        
        public byte[] GetResource(ResourceType type)
        {
            string filePath = GetResourceFilePath(type);

            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }

            return new byte[0];
        }

        public void SaveAs(ResourceType type, byte[] data)
        {
            string filePath = GetResourceFilePath(type);

            File.WriteAllBytes(filePath, data);
        }

        public string GetResourceFilePath(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.DatabaseJson:
                    return Path.Combine(_resourcesFolder, DatabaseRelativePath);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown resource type");
            }
        }
    }
}