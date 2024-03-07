using Cysharp.Threading.Tasks;
using ReDI;
using UnityEngine;

namespace Remouse.UnityEngine.UI
{
    public class UIViewFactory : IUIViewFactory
    {
        [Inject] private ICanvasDispatcher _canvasDispatcher;
        [Inject] private IGameObjectFactory _gameObjectFactory;

        public async UniTask<TUIView> CreateViewAsync<TUIView>(bool unloadAfterCreate) where TUIView : LoadableUIView
        {
            var viewKey = UIAssetKeys.GetUIViewKey(typeof(TUIView));

            var gameObject = await _gameObjectFactory.CreateGameObjectAsync(viewKey, unloadAfterCreate);
            
            var view = gameObject.GetComponent<TUIView>();
            
            var canvas = await _canvasDispatcher.GetAsync(view.canvasType);
            
            var rectTransform = gameObject.GetComponent<RectTransform>();
            
            var anchoredPosition = rectTransform.anchoredPosition;
            
            rectTransform.SetParent(canvas.transform);
            rectTransform.anchoredPosition = anchoredPosition;

            return view;
        }
    }
}