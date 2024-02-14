using Remouse.UnityEngine.Assets;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class LoadableAssetTool
    {
        private readonly LoadableAsset _addressableLoadable;
        private readonly GameObject _gameObject;

        public LoadableAssetTool(GameObject gameObject)
        {
            _gameObject = gameObject;
            _addressableLoadable = _gameObject.GetComponent<LoadableAsset>();
        }

        public void SaveAsPrefab()
        {
            var transform = _gameObject.transform;
            
            var oldPosition = transform.position;
            transform.position = new Vector3(0, 0, 0);
            PrefabUtility.SaveAsPrefabAssetAndConnect(_gameObject, $"Assets/{_gameObject.name}.prefab", InteractionMode.UserAction);
            
            _gameObject.transform.position = oldPosition; 
        }

        public void AddToAddressable()
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);

            var defaultGroup = settings.DefaultGroup;
            settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(GetPrefabPath()), defaultGroup).address = GetSavingAssetKey();

            Debug.Log($"Asset converted to addressable. Address: {GetSavingAssetKey()}");
        }

        public string GetSavingAssetKey()
        {
            return _addressableLoadable.GetAssetKey();
        }

        private AddressableAssetEntry AsAddressable()
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);

            if (settings == null)
            {
                Debug.LogError("Addressables Settings not found.");
                return null;
            }

            var guid = AssetDatabase.AssetPathToGUID(GetPrefabPath());
            var entry = settings.FindAssetEntry(guid);

            return entry;
        }

        public bool IsAddressCorrect()
        {
            return GetAddress() == GetSavingAssetKey();
        }

        public bool IsPrefabOrPrefabInstance()
        {
            var assetType = PrefabUtility.GetPrefabAssetType(_gameObject);
            var instanceStatus = PrefabUtility.GetPrefabInstanceStatus(_gameObject);

            return IsEditingPrefab() || assetType != PrefabAssetType.NotAPrefab || instanceStatus == PrefabInstanceStatus.Connected;
        }

        public bool IsAddressable()
        {
            return AsAddressable() != null;
        }

        public bool IsEditingPrefab()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            return prefabStage?.prefabContentsRoot == _gameObject;
        }

        public bool IsEditingOnScene()
        {
            return !IsEditingPrefab();
        }

        public string GetAddress()
        {
            return AsAddressable()?.address ?? string.Empty;
        }
        
        public void SetCorrectAddress()
        {
            if (!IsAddressable())
                return;

            var addressable = AsAddressable();
            addressable?.SetAddress(GetSavingAssetKey());
        }

        public string GetPrefabPath()
        {
            if (!IsPrefabOrPrefabInstance())
                return string.Empty;

            if (IsEditingPrefab())
                return PrefabStageUtility.GetCurrentPrefabStage().assetPath;
            
            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(_gameObject);

            if (prefab != null)
            {
                return AssetDatabase.GetAssetPath(prefab);
            }
            
            return AssetDatabase.GetAssetPath(_gameObject) ?? string.Empty;
        }

        public GameObject GetPrefab()
        {
            if (IsEditingPrefab())
            {
                return _gameObject;
            }
            
            var prefabObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(_gameObject);

            if (prefabObject != null)
            {
                return prefabObject.gameObject;
            }
            
            return null;
        }
    }
}