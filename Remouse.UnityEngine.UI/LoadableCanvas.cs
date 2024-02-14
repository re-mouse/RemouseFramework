using Remouse.UnityEngine.Assets;
using UnityEngine;

namespace Remouse.UnityEngine.UI
{
    public class LoadableCanvas : LoadableAsset
    {
        public CanvasType type;
     
        private void OnValidate()
        {
            if (GetComponent<Canvas>() == null)
            {
                Debug.LogError("Canvas component not found on game object, creating new");

                gameObject.AddComponent<Canvas>();
            }
        }

        public override string GetAssetKey()
        {
            return UIAssetKeys.GetCanvasKey(type);
        }
    }
}