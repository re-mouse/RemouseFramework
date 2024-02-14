using Remouse.UnityEngine.Assets;

namespace Remouse.UnityEngine.UI
{
    public abstract class LoadableUIView : LoadableAsset
    {
        public CanvasType canvasType;
        public bool alwaysNew;

        public override string GetAssetKey()
        {
            return UIAssetKeys.GetUIViewKey(GetType());
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}