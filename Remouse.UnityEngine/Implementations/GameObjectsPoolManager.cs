using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ReDI;
using Remouse.UnityEngine.Assets;
using Remouse.UnityEngine.Utils;
using Remouse.Utils;
using UnityEngine;

namespace Remouse.UnityEngine
{
    public class GameObjectsPoolManager : IGameObjectsPoolManager
    {
        [Inject] private IAssetLoader _assetLoader;
        
        private Dictionary<string, HashSet<PoolReserveToken>> _poolReferenceTokens = new Dictionary<string, HashSet<PoolReserveToken>>();
        private Dictionary<string, GameObjectPool> _pools = new Dictionary<string, GameObjectPool>();

        private Dictionary<string, IAsyncAssetContainer<GameObject>> _gameObjectAssetContainers = new Dictionary<string, IAsyncAssetContainer<GameObject>>();
        
        public async UniTask<PoolReserveToken> GetPoolAsync(string key)
        {
            if (!_poolReferenceTokens.ContainsKey(key))
            {
                _poolReferenceTokens[key] = new HashSet<PoolReserveToken>();
            }

            if (!_gameObjectAssetContainers.ContainsKey(key))
            {
                _gameObjectAssetContainers[key] = _assetLoader.GetAsset<GameObject>(key);
                LLogger.Current.LogInfo(this, $"Started loading asset [Key:{key}]");
            }

            var assetContainer = _gameObjectAssetContainers[key];
            
            if (!assetContainer.IsDone)
            {
                await assetContainer.LoadTask;
            }

            if (!_pools.ContainsKey(key))
            {
                _pools[key] = new GameObjectPool(assetContainer.Asset);
                LLogger.Current.LogInfo(this, $"Created new pool [Key:{key}]");
            }

            var reserveToken = new PoolReserveToken(_pools[key], p => HandleTokenReleased(p, key));
            _poolReferenceTokens[key].Add(reserveToken);
            LLogger.Current.LogInfo(this, $"Pool reserved [Key:{key}] [ReservesCount:{_poolReferenceTokens[key].Count}]");
            return reserveToken;
        }

        private void HandleTokenReleased(PoolReserveToken token, string key)
        {
            LLogger.Current.LogInfo(this, $"Pool token released[Key:{key}]");

            if (_poolReferenceTokens[key].Remove(token) && _poolReferenceTokens[key].Count == 0)
            {
                if (_pools.ContainsKey(key))
                {
                    _pools[key].Dispose();
                    _pools.Remove(key);
                }

                if (_gameObjectAssetContainers.ContainsKey(key))
                {
                    _gameObjectAssetContainers[key].Dispose();
                    _gameObjectAssetContainers.Remove(key);
                }
                
                LLogger.Current.LogInfo(this, $"Pool disposed [Key:{key}]");

                _poolReferenceTokens.Remove(key);
            }
        }

        public void ReleaseAll()
        {
            foreach (var pool in _pools.Values)
            {
                pool.Dispose();
            }
            _pools.Clear();
            
            foreach (var assetContainer in _gameObjectAssetContainers.Values)
            {
                assetContainer.Dispose();
            }
            _gameObjectAssetContainers.Clear();
            
            _poolReferenceTokens.Clear();
            
            LLogger.Current.LogInfo(this, $"All pools disposed, reference token deleted");
        }
    }
}