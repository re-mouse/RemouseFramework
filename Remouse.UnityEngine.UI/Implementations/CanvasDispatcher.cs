using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ReDI;
using Remouse.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Remouse.UnityEngine.UI
{
    public class CanvasDispatcher : IDisposable, ICanvasDispatcher
    {
        [Inject] private IGameObjectFactory _gameObjectFactory;
        
        private readonly Dictionary<CanvasType, Canvas> _loadedCanvases = new Dictionary<CanvasType, Canvas>();
        private readonly Dictionary<CanvasType, UniTask> _loadingTasks = new Dictionary<CanvasType, UniTask>();

        public async UniTask<Canvas> GetAsync(CanvasType canvasType)
        {
            if (!_loadedCanvases.ContainsKey(canvasType))
            {
                await LoadCanvasAsync(canvasType);
            }

            if (_loadedCanvases.TryGetValue(canvasType, out Canvas canvas))
            {
                return canvas;
            }

            LLogger.Current.LogError(this, $"Failed to create canvas [CanvasType:{canvasType}]");

            return null;
        }

        private async UniTask LoadCanvasAsync(CanvasType canvasType)
        {
            if (_loadedCanvases.ContainsKey(canvasType))
            {
                return;
            }

            if (_loadingTasks.TryGetValue(canvasType, out var task))
            {
                await task;
                return;
            }

            string canvasKey = UIAssetKeys.GetCanvasKey(canvasType);
            LLogger.Current.LogInfo(this, $"Loading canvas of type {canvasType} at address {canvasKey}");
            
            var loadingTask = _gameObjectFactory.CreateGameObjectAsync(canvasKey, true);

            _loadingTasks[canvasType] = loadingTask;

            var gameObject = await loadingTask;
            
            _loadedCanvases[canvasType] = gameObject.GetComponent<Canvas>();;
            
            _loadingTasks.Remove(canvasType);
        }

        public void Dispose()
        {
            foreach (var loadedCanvas in _loadedCanvases)
            {
                Object.Destroy(loadedCanvas.Value.gameObject);
            }

            _loadedCanvases.Clear();
        }
    }
}