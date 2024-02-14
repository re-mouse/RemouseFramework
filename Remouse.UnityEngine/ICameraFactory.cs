using Cysharp.Threading.Tasks;
using Remouse.UnityEngine.Assets;
using UnityEngine;

namespace Remouse.UnityEngine
{
    public interface ICameraFactory
    {
        UniTask<Camera> GetAsync(LoadableCameraType loadableCameraType);
    }
}