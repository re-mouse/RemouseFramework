using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.Utils
{
    public class CollectionsDictionary<TKey, TValue> : IEnumerable
    {
        private Dictionary<TKey, ICollection<TValue>> _dictionary = new Dictionary<TKey, ICollection<TValue>>();

    public IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int CollectionCount(TKey key)
    {
        if (_dictionary.TryGetValue(key, out var values))
        {
            return values.Count;
        }
        else
        {
            return 0;
        }
    }

    public void Add(KeyValuePair<TKey, ICollection<TValue>> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, ICollection<TValue>> item)
    {
        return TryGetValue(item.Key, out var values) && values.SequenceEqual(item.Value);
    }

    public bool Remove(KeyValuePair<TKey, ICollection<TValue>> item)
    {
        if (TryGetValue(item.Key, out var values) && values.SequenceEqual(item.Value))
        {
            return Remove(item.Key);
        }
        return false;
    }

    public int Count { get => _dictionary.Count; }
    public bool IsReadOnly { get => false; }

    public void Add(TKey key, ICollection<TValue> value)
    {
        if (!_dictionary.TryGetValue(key, out var values))
        {
            values = value.ToHashSet();
            _dictionary.Add(key, values);
        }
        else
        {
            _dictionary[key] = values.Union(value).ToHashSet();
        }
    }
    
    public void Add(TKey key, TValue value)
    {
        if (!_dictionary.TryGetValue(key, out var values))
        {
            values = new HashSet<TValue>();
            values.Add(value);
            _dictionary.Add(key, values);
        }
        else
        {
            _dictionary[key].Add(value);
        }
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return _dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, out ICollection<TValue> value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public ICollection<TValue> this[TKey key]
    {
        get
        {
            if (!_dictionary.TryGetValue(key, out var values))
            {
                values = new HashSet<TValue>();
                _dictionary.Add(key, values);
            }
            return values;
        }
        set
        {
            HashSet<TValue> values = new HashSet<TValue>(value);
            _dictionary[key] = values;
        }
    }

    public ICollection<TKey> Keys { get => _dictionary.Keys; }
    public ICollection<ICollection<TValue>> Values { get => _dictionary.Values; }
    }
}