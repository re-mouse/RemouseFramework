using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Remouse.UnityEngine.Assets
{
    internal class AddressableAssetLoader : IAssetLoader
    {
        private Dictionary<string, HashSet<IDisposable>> _assetContainers = new Dictionary<string, HashSet<IDisposable>>();
        private Dictionary<string, object> _containersByKey = new Dictionary<string, object>();
        private Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();
        private bool _disposed;
        
        public IAsyncAssetContainer<TAsset> GetAsset<TAsset>(string key)
        {
            return CreateAssetContainer<TAsset>(key);
        }

        private IAsyncAssetContainer<TAsset> CreateAssetContainer<TAsset>(string key)
        {
            if (!_assetContainers.ContainsKey(key))
            {
                _assetContainers[key] = new HashSet<IDisposable>();
            }
            
            if (_containersByKey.ContainsKey(key))
            {
                var assetContainer = (AsyncAssetContainer<TAsset>)_containersByKey[key];
                var newContainer = assetContainer.CreateCopy();
                _assetContainers[key].Add(newContainer);
                return newContainer;
            }
            
            var handle = Addressables.LoadAssetAsync<TAsset>(key);
            _handles[key] = handle;
            var container = new AsyncAssetContainer<TAsset>(handle, c => HandleContainerDisposed(c, key));
            _containersByKey[key] = container;
            
            _assetContainers[key].Add(container);
            return container;
        }

        private void HandleContainerDisposed(IDisposable container, string key)
        {
            _assetContainers[key].Remove(container);

            if (_assetContainers[key].Count == 0)
            {
                ReleaseAsset(key);
                _assetContainers.Remove(key);
                _containersByKey.Remove(key);
            }
        }
        
        private void ReleaseAsset(string key)
        {
            Addressables.Release(_handles[key]);
            _handles.Remove(key);
        }

        public void ForceUnload(string key)
        {
            if (_assetContainers.ContainsKey(key))
            {
                foreach (var container in _assetContainers[key])
                    container.Dispose();
            }
        }

        public List<string> GetLoadedKeys()
        {
            return _containersByKey.Keys.ToList();
        }

        public void Dispose()
        {
            foreach (var key in _containersByKey.Keys.ToList())
            {
                if (_assetContainers.ContainsKey(key))
                {
                    foreach (var container in _assetContainers[key])
                        container.Dispose();
                }
            }
            
            _containersByKey.Clear();
        }
    }
}