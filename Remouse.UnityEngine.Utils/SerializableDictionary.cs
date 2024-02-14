using System;
using System.Collections.Generic;
using UnityEngine;

namespace Remouse.UnityEngine.Utils
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        // Unity serialization callback before serialization
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var kvp in this)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        // Unity serialization callback after deserialization
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
            {
                throw new Exception("The number of keys and values in the dictionary do not match.");
            }
        
            for (int i = 0; i < keys.Count; i++) 
            {
                this[keys[i]] = values[i];
            }
        }
    }
}