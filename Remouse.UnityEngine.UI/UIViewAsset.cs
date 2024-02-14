using Remouse.UnityEngine.Assets;

namespace Remouse.UnityEngine.UI
{
    public class UIViewAsset : Asset
    {
        public override string prefix { get => UIAssetKeys.GetUIViewPrefix(); }
    }
}