using System;
using System.Collections.Generic;
using UnityEngine;
using Remouse.Utils.ObjectPool;

namespace Remouse.UnityEngine.Utils
{
    public class GameObjectPool : AbstractPool<GameObject>
    {
        private readonly GameObject _original;
        private readonly GameObject _poolParent;

        public GameObjectPool(GameObject gameObject)
        {
            _original = gameObject;
            _poolParent = new GameObject($"{_original.name} pool");
        }

        protected override void OnGet(GameObject value)
        {
            value.SetActive(true);
        }

        protected override void OnReturn(GameObject value)
        {
            value.SetActive(false);
            value.transform.parent = _poolParent.transform;
        }

        protected override GameObject CreateNew()
        {
            return GameObject.Instantiate(_original, _poolParent.transform);
        }

        public override void OnDispose()
        {
            foreach (var gameObjectToken in createdObjects)
            {
                GameObject.Destroy(gameObjectToken.Value);
            }
            
            GameObject.Destroy(_poolParent);
        }
    }
    
    public class GameObjectPool<T> : AbstractPool<T> where T : Component
    {
        private readonly T _original;
        private readonly GameObject _poolParent;

        public GameObjectPool(T original)
        {
            _original = original;
            _poolParent = new GameObject($"{_original.name} {typeof(T).Name} pool");
        }
        
        protected override void OnGet(T value)
        {
            value.gameObject.SetActive(true);
        }

        protected override void OnReturn(T value)
        {
            value.transform.parent = _poolParent.transform;
            value.gameObject.SetActive(false);
        }

        public override void OnDispose()
        {
            foreach (var gameObjectToken in createdObjects)
            {
                GameObject.Destroy(gameObjectToken.Value.gameObject);
            }
            
            GameObject.Destroy(_poolParent.gameObject);
        }

        protected override T CreateNew()
        {
            return GameObject.Instantiate(_original.gameObject, _poolParent.transform).GetComponent<T>();
        }
    }
}