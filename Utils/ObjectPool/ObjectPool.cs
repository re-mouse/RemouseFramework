using System.Collections.Generic;

namespace Remouse.Shared.Utils.ObjectPool
{
    public class ObjectPool<T> where T : new()
    {
        private const int DefaultCapacity = 10;
        
        private readonly List<T> _objects = new List<T>();
        private readonly object _lock = new object();
        private int _count;

        public ObjectPool() : this(DefaultCapacity)
        {
        }

        public ObjectPool(int initialCount)
        {
            for (int i = 0; i < initialCount; i++)
            {
                IncreasePool();
            }
        }

        public T Get()
        {
            lock (_lock)
            {
                if (_count == 0)
                {
                    IncreasePool();
                    return _objects[--_count];
                }

                var obj = _objects[--_count];
                _objects.RemoveAt(_count);
                return obj;
            }
        }

        public void Return(T obj)
        {
            lock (_lock)
            {
                _objects.Add(obj);
                _count++;
            }
        }

        private void IncreasePool()
        {
            _objects.Add(new T());
            _count++;
        }
    }
}