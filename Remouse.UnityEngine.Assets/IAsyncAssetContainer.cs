using System;
using System.Threading.Tasks;

namespace Remouse.UnityEngine.Assets
{
    public interface IAsyncAssetContainer<TAsset> : IDisposable
    {
        public TAsset Asset { get; }
        public Task LoadTask { get; }
        public bool IsDone { get; }
        public bool Disposed { get; }
    }
}