using System;
using System.Collections.Generic;

namespace Remouse.UnityEngine.Assets
{
    public interface IAssetLoader : IDisposable
    {
        public IAsyncAssetContainer<TAsset> GetAsset<TAsset>(string key);
        public void ForceUnload(string key);
        public List<string> GetLoadedKeys();
    }
}