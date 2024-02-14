using Remouse.UnityEngine.Assets;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class SaveAsPrefabOrAddressableDrawer
    {
        private readonly LoadableAsset _target;
        private readonly Component _component;

        public string errorHandler;
        
        private readonly LoadableAssetTool _loadableAssetTool;
        
        public SaveAsPrefabOrAddressableDrawer(LoadableAsset target)
        {
            _component = target as Component;
            _loadableAssetTool = new LoadableAssetTool(_component.gameObject);
        }

        public void Draw()
        {
            if (!_loadableAssetTool.IsPrefabOrPrefabInstance() || !_loadableAssetTool.IsAddressable())
            {
                DrawSaveTypeButton();
            }
            else if (!_loadableAssetTool.IsAddressCorrect())
            {
                DrawFixAddressButton();

                GUILayout.Space(20);
            }
        }
        
        private void DrawSaveTypeButton()
        {
            var background = GUIColorConsts.Default.StartBackground();
            
            errorHandler = $"Object have type {_target} but it's not a prefab or addressable, type not exist and cannot be loaded trough map config";
            
            if (!_loadableAssetTool.IsAddressable())
            {
                GUILayout.Label($"Address: {_loadableAssetTool.GetSavingAssetKey()}");
            }
            
            if (GUILayout.Button("Save"))
            {
                if (!_loadableAssetTool.IsPrefabOrPrefabInstance())
                {
                    _loadableAssetTool.SaveAsPrefab();
                }

                if (!_loadableAssetTool.IsAddressable())
                {
                    _loadableAssetTool.AddToAddressable();
                }
            }
            
            GUILayout.Space(20);
            
            background.End();
        }
        
        private void DrawFixAddressButton()
        {
            if (!_loadableAssetTool.IsAddressCorrect())
            {
                GUILayout.Label($"Current address: {_loadableAssetTool.GetAddress()}");
                GUILayout.Label($"Correct address path: {_loadableAssetTool.GetSavingAssetKey()}");
            }
            
            if (!_loadableAssetTool.IsAddressCorrect() && GUILayout.Button("Update address"))
            {
                _loadableAssetTool.SetCorrectAddress();
            }
        }
    }
}