using Cysharp.Threading.Tasks;

namespace Remouse.UnityEngine.UI
{
    public interface IUIViewFactory
    {
        UniTask<TUIView> CreateViewAsync<TUIView>(bool unloadAfterCreate = true) where TUIView : LoadableUIView;
    }
}