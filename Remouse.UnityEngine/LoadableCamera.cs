using Remouse.UnityEngine.Assets;
using UnityEngine;

namespace Remouse.UnityEngine
{
    public class LoadableCamera : LoadableAsset
    {
        [SerializeField] private LoadableCameraType _loadableCameraType;

        public override string GetAssetKey()
        {
            return CameraAssetKeys.GetCameraKey(_loadableCameraType);
        }
    }
}