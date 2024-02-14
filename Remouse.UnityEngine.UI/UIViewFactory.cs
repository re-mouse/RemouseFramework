using Cysharp.Threading.Tasks;
using Remouse.DI;
using UnityEngine;

namespace Remouse.UnityEngine.UI
{
    public class UIViewFactory : IUIViewFactory
    {
        private ICanvasDispatcher _canvasDispatcher;
        private IGameObjectFactory _gameObjectFactory;

        public void Construct(Container container)
        {
            _gameObjectFactory = container.Resolve<IGameObjectFactory>();
            _canvasDispatcher = container.Resolve<ICanvasDispatcher>();
        }

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