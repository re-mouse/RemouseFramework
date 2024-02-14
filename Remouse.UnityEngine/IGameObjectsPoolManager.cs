using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Remouse.UnityEngine
{
    public interface IGameObjectsPoolManager
    {
        UniTask<PoolReserveToken> GetPoolAsync(string key);
        void ReleaseAll();
    }
}