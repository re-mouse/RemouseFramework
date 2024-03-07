using Remouse.UnityEngine.Assets;

namespace Remouse.UnityEngine.SimulationRender
{
    public class EntityRenderAsset : Asset
    {
        public override string prefix { get => EntityRenderAssetKeys.GetPrefix(); }
    }
}