using System;
using System.Collections.Generic;
using System.Linq;

namespace Pluggy.SDK.Helpers
{
    public static class DictionaryHelper
    {
        public static Dictionary<K,V> RemoveNulls<K,V>(this Dictionary<K, V> dictionary)
        {
            return dictionary
                .Where(pair => pair.Value != null)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
