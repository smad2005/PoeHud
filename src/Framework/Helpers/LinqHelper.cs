using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PoeHUD.Framework.Helpers
{
    public static class LinqHelper
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static void ForEach<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary,
            Action<TKey, TValue> action)
        {
            Contract.Requires(dictionary != null);
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                action(pair.Key, pair.Value);
            }
        }
    }
}