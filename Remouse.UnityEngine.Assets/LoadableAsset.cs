using UnityEngine;

namespace Remouse.UnityEngine.Assets
{
    [DisallowMultipleComponent]
    public class LoadableAsset : MonoBehaviour
    {
        public virtual string GetAssetKey()
        {
            return DefaultAssetKeys.GetKey(gameObject.name);
        }
    }
}