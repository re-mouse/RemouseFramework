using System.Collections.Generic;

namespace Remouse.Utils
{
    public static class CollectionExtensions
    {
        public static bool ReplaceKey<T, A>(this Dictionary<T, A> dictionary, T oldKey, T newKey)
        {
            A value = dictionary[oldKey];
            if (dictionary.ContainsKey(oldKey) && !dictionary.ContainsKey(newKey) && dictionary.TryAdd(newKey, value))
            {
                dictionary.Remove(oldKey);
                return true;
            }

            return false;
        }
    }
}