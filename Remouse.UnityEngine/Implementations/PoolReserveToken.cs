using System;
using Remouse.UnityEngine.Utils;

namespace Remouse.UnityEngine
{
    public class PoolReserveToken : IPoolReserveToken
    {
        public GameObjectPool Pool { get; }
        private readonly Action<PoolReserveToken> _disposeCallback;
        private bool _disposed;

        public PoolReserveToken(GameObjectPool pool, Action<PoolReserveToken> disposeCallback)
        {
            Pool = pool;
            _disposeCallback = disposeCallback;
        }

        public void Release()
        {
            if (_disposed)
                return;
            
            _disposed = true;
            _disposeCallback?.Invoke(this);
        }
    }
}