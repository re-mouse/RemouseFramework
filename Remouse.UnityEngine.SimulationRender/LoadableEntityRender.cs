using Remouse.UnityEngine.Assets;
using UnityEngine;

namespace Remouse.UnityEngine.SimulationRender
{
    [RequireComponent(typeof(EntityInstallerComponent))]
    [RequireComponent(typeof(EntityRender))]
    public class LoadableEntityRender : LoadableAsset
    {
        public override string GetAssetKey()
        {
            return EntityRenderAssetKeys.GetKey(GetComponent<EntityInstallerComponent>().typeId);
        }
    }
}