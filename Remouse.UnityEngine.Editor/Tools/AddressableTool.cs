using System.Collections.Generic;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class AddressableTool
    {
        public List<AddressableAssetEntry> FindAddressablesStartingWith(string prefix)
        {
            var result = new List<AddressableAssetEntry>();
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;

            if (addressableSettings == null)
            {
                Debug.Log("No Addressable Asset Settings found.");
                return result;
            }

            foreach (var group in addressableSettings.groups)
            {
                if (group == null)
                    continue;
                
                foreach (var entry in group.entries)
                {
                    if (entry == null)
                        continue;
                    
                    if (entry.address.StartsWith(prefix))
                    {
                        result.Add(entry);
                    }
                }
            }

            return result;
        }
    }
}