using System;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Remouse.UnityEngine.Assets
{
    internal class AsyncAssetContainer<TAsset> : IAsyncAssetContainer<TAsset>
    {
        private readonly Action<IDisposable> _disposeCallback;
        public TAsset Asset { get => _handle.Result; }
        public Task LoadTask { get => _handle.Task; }
        public bool IsDone { get => _handle.IsDone || Disposed; }
        public bool Disposed { get; private set; }
        
        private AsyncOperationHandle<TAsset> _handle;

        public AsyncAssetContainer(AsyncOperationHandle<TAsset> handle, Action<IDisposable> disposeCallback)
        {
            _disposeCallback = disposeCallback;
            _handle = handle;
        }
        
        public void Dispose()
        {
            if (Disposed)
                return;

            Disposed = true;

            if (_handle.IsDone)
            {
                _disposeCallback?.Invoke(this);
            }
            else
            {
                _handle.Completed += x =>
                {
                    _disposeCallback?.Invoke(this);
                };
            }
            
            _handle = default;
        }

        public IAsyncAssetContainer<TAsset> CreateCopy()
        {
            return new AsyncAssetContainer<TAsset>(_handle, _disposeCallback);
        }
    }
}