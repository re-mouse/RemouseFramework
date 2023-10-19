using System;

namespace Remouse.Shared.Utils.ObjectPool
{
    public static class StaticPool<T> where T : new()
    {
        private static ObjectPool<T> pool = new ObjectPool<T>();

        public static T Get()
        {
            return pool.Get();
        }

        public static void Return(StaticPoolToken<T> token)
        {
            pool.Return(token.value);
        }
    }

    public struct StaticPoolToken<T> : IDisposable where T : new()
    {
        internal readonly T value;
        
        private bool _isDisposed;
        
        internal StaticPoolToken(T value)
        {
            this.value = value;
            _isDisposed = false;
        }


        public void Dispose()
        {
            StaticPool<T>.Return(this);
        }
    }
}