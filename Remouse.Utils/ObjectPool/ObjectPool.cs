using System;
using System.Collections.Generic;

namespace Remouse.Utils.ObjectPool
{
    public abstract class AbstractPool<T> : IDisposable where T : class
    {
        protected readonly Stack<PooledObject<T>> availableObjects = new Stack<PooledObject<T>>();
        protected readonly HashSet<PooledObject<T>> createdObjects = new HashSet<PooledObject<T>>();
        protected int count;

        public PooledObject<T> Get()
        {
            if (count == 0)
            {
                IncreasePool();
            }

            var obj = availableObjects.Pop();
            OnGet(obj.Value);
            count--;
            return obj;
        }

        private void IncreasePool()
        {
            var poolToken = new PooledObject<T>(this, CreateNew());
            createdObjects.Add(poolToken);
            availableObjects.Push(poolToken);
            count++;
        }
        
        public void Return(PooledObject<T> obj)
        {
            OnReturn(obj.Value);
            obj.Reset();
            availableObjects.Push(obj);
            count++;
        }

        protected abstract T CreateNew();
        protected abstract void OnGet(T value);
        protected abstract void OnReturn(T value);
        public abstract void OnDispose();

        public void Dispose()
        {
            OnDispose();
            
            createdObjects.Clear();
            availableObjects.Clear();
            count = 0;
        }
    }

    public class PooledObject<T> where T : class
    {
        private readonly AbstractPool<T> _pool;
        public T Value { get; private set; }
        
        private bool _returned;

        public PooledObject(AbstractPool<T> pool, T value)
        {
            _pool = pool; 
            Value = value;
        }

        internal void Reset()
        {
            _returned = false;
        }

        public void Return()
        {
            if (_returned)
                return;
            
            _pool.Return(this);
            _returned = true;
        }
    }
}