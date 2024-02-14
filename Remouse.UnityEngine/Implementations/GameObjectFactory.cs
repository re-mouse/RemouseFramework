using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Remouse.DI;
using Remouse.UnityEngine.Assets;
using UnityEngine;
using Remouse.Utils;

namespace Remouse.UnityEngine
{
    public class GameObjectFactory : IGameObjectFactory
    {
        private Dictionary<string, IAsyncAssetContainer<GameObject>> _assetContainers =
            new Dictionary<string, IAsyncAssetContainer<GameObject>>();

        private IAssetLoader _assetLoader;

        public void Construct(Container container)
        {
            _assetLoader = container.Resolve<IAssetLoader>();
        }
        
        public async UniTask<GameObject> CreateGameObjectAsync(string key, bool unloadAfterCreate)
        {
            LLogger.Current.LogTrace(this, $"Creating GameObject [AssetKey:{key}] [UnloadAfterCreate:{unloadAfterCreate}]");
            
            if (_assetContainers.ContainsKey(key))
            {
                if (!_assetContainers[key].IsDone)
                    await _assetContainers[key].LoadTask;
                
                return GameObject.Instantiate(_assetContainers[key].Asset); 
            }

            var assetContainer = _assetLoader.GetAsset<GameObject>(key);
            
            if (!unloadAfterCreate)
            {
                _assetContainers[key] = assetContainer;
            }

            await assetContainer.LoadTask;

            var gameObject = GameObject.Instantiate(assetContainer.Asset);
            
            if (unloadAfterCreate)
            {
                assetContainer.Dispose();
            }

            return gameObject;
        }

        public async UniTask LoadGameObjectAsync(string key)
        {
            if (!_assetContainers.ContainsKey(key))
            {
                _assetContainers[key] = _assetLoader.GetAsset<GameObject>(key);
            }

            if (!_assetContainers[key].IsDone)
            {
                await _assetContainers[key].LoadTask;
            }
        }

        public void UnloadGameObject(string key)
        {
            if (_assetContainers.ContainsKey(key))
            {
                _assetContainers[key].Dispose();
                _assetContainers.Remove(key);
            }
        }

        public void UnloadAll()
        {
            foreach (var assetContainers in _assetContainers.Values)
            {
                assetContainers.Dispose();
            }
            
            _assetContainers.Clear();
        }
    }
}