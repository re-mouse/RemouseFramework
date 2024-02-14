using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Remouse.DI;
using Remouse.Utils;
using UnityEngine;

namespace Remouse.UnityEngine
{
    public class CameraFactory : ICameraFactory
    {
        private readonly Dictionary<LoadableCameraType, Camera> _loadedCameras = new Dictionary<LoadableCameraType, Camera>();

        private readonly Dictionary<LoadableCameraType, UniTask> _loadingTasks = new Dictionary<LoadableCameraType, UniTask>();

        private IGameObjectFactory _gameObjectFactory;

        public void Construct(Container container)
        {
            _gameObjectFactory = container.Resolve<IGameObjectFactory>();
        }

        public async UniTask<Camera> GetAsync(LoadableCameraType loadableCameraType)
        {
            LLogger.Current.LogInfo(this, $"Accessed camera [CameraType:{loadableCameraType}]");
            
            if (!_loadedCameras.ContainsKey(loadableCameraType))
            {
                await LoadCameraAsync(loadableCameraType);
            }

            if (_loadedCameras.TryGetValue(loadableCameraType, out Camera camera))
            {
                LLogger.Current.LogInfo(this, $"Found loaded camera [CameraType:{loadableCameraType}]");
                
                return camera;
            }

            LLogger.Current.LogError(this, $"Failed to create camera: {loadableCameraType}");

            return null;
        }

        public async UniTask LoadCameraAsync(LoadableCameraType loadableCameraType)
        {
            if (_loadedCameras.ContainsKey(loadableCameraType))
            {
                return;
            }

            if (_loadingTasks.TryGetValue(loadableCameraType, out var loadingTask))
            {
                await loadingTask;
                return;
            }

            string addressKey = CameraAssetKeys.GetCameraKey(loadableCameraType);
            LLogger.Current.LogInfo(this, $"Loading camera of type {loadableCameraType} at address {addressKey}");
            
            var task = _gameObjectFactory.CreateGameObjectAsync(addressKey, true);

            _loadingTasks[loadableCameraType] = task;
            var gameObject = await task;
            
            _loadedCameras[loadableCameraType] = gameObject.GetComponent<Camera>();
            _loadingTasks.Remove(loadableCameraType);
        }
    }
}