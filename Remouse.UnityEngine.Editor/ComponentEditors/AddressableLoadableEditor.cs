using Remouse.UnityEngine.Assets;
using UnityEditor;

namespace Remouse.UnityEngine.Editor
{
    [CustomEditor(typeof(LoadableAsset), true)]
    public class AddressableLoadableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var tool = new SaveAsPrefabOrAddressableDrawer(target as LoadableAsset);
            tool.Draw();
        }
    }
}
