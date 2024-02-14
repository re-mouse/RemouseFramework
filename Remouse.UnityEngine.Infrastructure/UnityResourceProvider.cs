using System;
using System.IO;
using Infrastructure;
using Remouse.Utils;
using UnityEngine;

namespace Remouse.UnityEngine.Assets
{
    public class UnityResourceProvider : IResources
    {
        private string ResourcesPath { get => Application.streamingAssetsPath + "/RemouseFramework/"; }

        public byte[] GetResource(ResourceType type)
        {
            var resourceFilePath = GetResourceFilePath(type);
            
            if (!File.Exists(resourceFilePath))
            {
                LLogger.Current.LogError(this, $"Accessed resource not exist: {type}");
                return Array.Empty<byte>();
            }
            
            var loaded = File.ReadAllBytes(resourceFilePath);
            
            LLogger.Current.LogInfo(this, $"Accessed resource of type {type}, path {resourceFilePath}, size {loaded.Length}");
            return loaded;
        }

        public string GetResourceFilePath(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.DatabaseJson:
                    return Path.Combine(ResourcesPath, $"{type}.json");
            }
            return Path.Combine(ResourcesPath, type.ToString());
        }

        public void SaveAs(ResourceType type, byte[] data)
        {
            LLogger.Current.LogInfo(this, $"Saving resource of type {type}, path {GetResourceFilePath(type)}, size {data.Length}");

            try
            {
                var directoryPath = Path.GetDirectoryName(GetResourceFilePath(type));
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                File.WriteAllBytes(GetResourceFilePath(type), data);
            }
            catch (System.Exception ex)
            {
                LLogger.Current.LogError(this, $"Failed to save resource of type {type}. Error: {ex.Message}");
            }
        }
    }
}