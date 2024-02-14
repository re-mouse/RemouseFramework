using Remouse.UnityEngine.Assets;

namespace Remouse.UnityEngine.SimulationRender
{
    public class EntityAsset : Asset
    {
        public override string prefix { get => EntityRenderAssetKeys.GetPrefix(); }
    }
}