namespace Remouse.UnityEngine
{
    public class CameraAssetKeys
    {
        public static string GetCameraPrefix()
        {
            return "Camera/";
        }
        
        public static string GetCameraKey(LoadableCameraType loadableCameraType)
        {
            return $"{GetCameraPrefix()}{loadableCameraType}";
        }
    }
}