using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Remouse.Utils.ObjectPool;

namespace Remouse.UnityEngine
{
    public interface IGameObjectFactory
    {
        public UniTask<GameObject> CreateGameObjectAsync(string key, bool unloadAfterCreate);
        public UniTask LoadGameObjectAsync(string key);
        public void UnloadGameObject(string key);
        public void UnloadAll();
    }
}