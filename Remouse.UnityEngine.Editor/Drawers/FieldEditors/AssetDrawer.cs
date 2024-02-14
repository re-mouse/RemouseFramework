using System;
using System.Linq;
using Remouse.UnityEngine.Assets;
using UnityEditor;
using UnityEngine;
using Remouse.Utils;

namespace Remouse.UnityEngine.Editor
{
    public class AssetDrawer: IFieldEditor
    {
        private AddressableTool _addressableTool = new AddressableTool();
        
        public bool CanHandle(Type type) { return type.BaseType == typeof(Asset); }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            Asset asset = value as Asset;
            GUILayout.BeginHorizontal();
            var assets = _addressableTool.FindAddressablesStartingWith(asset.prefix)
                .Select(addressable => addressable.address).ToArray();
            var selectedAsset = Array.IndexOf(assets, asset.key);
            
            if (selectedAsset < 0)
            {
                LLogger.Current.LogError(this, $"Not found {type} for key {asset.key}. Reseting");
                selectedAsset = 0;
                isChanged = true;
            }
            
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            var newSelectedAsset = EditorGUILayout.Popup(selectedAsset, assets.ToArray());
            GUILayout.EndHorizontal();

            asset.key = assets[newSelectedAsset];
            
            if (selectedAsset != newSelectedAsset)
                isChanged = true;
            
            return asset;
        }
    }
}