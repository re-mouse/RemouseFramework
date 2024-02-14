using Remouse.UnityEngine.Utils;

namespace Remouse.UnityEngine
{
    public interface IPoolReserveToken
    {
        public void Release();
        public GameObjectPool Pool { get; }
    }
}