using System;
using System.Collections.Generic;
using System.Linq;

namespace Waterbot.Common
{
    /// <summary>
    /// Provides additional functionality for interacting with collections.
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        /// Gets the value that is associated with the specified key, or the
        /// default value for <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TKey">
        /// The type of keys in the read-only dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// The type of values in the read-only dictionary.
        /// </typeparam>
        /// <param name="dictionary">
        /// The read-only dictionary to get the value from.
        /// </param>
        /// <param name="key">The key to locate.</param>
        /// <returns>
        /// The value that is associated with the specified key, or the default
        /// value for <typeparamref name="TValue"/>.
        /// </returns>
        public static TValue Get<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary,
            TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            TValue value = default(TValue);
            dictionary.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Gets the value that is associated with the specified key, or the
        /// default value for <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TKey">
        /// The type of keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// The type of values in the dictionary.
        /// </typeparam>
        /// <param name="dictionary">
        /// The dictionary to get the value from.
        /// </param>
        /// <param name="key">The key to locate.</param>
        /// <returns>
        /// The value that is associated with the specified key, or the default
        /// value for <typeparamref name="TValue"/>.
        /// </returns>
        public static TValue Get<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            TValue value = default(TValue);
            dictionary.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Determines the index of a specific string in the list, ignoring or
        /// honoring its case.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">The string to locate in the list.</param>
        /// <param name="ignoreCase">
        /// <c>true</c> to ignore case during the comparison; otherwise,
        /// <c>false</c>.
        /// </param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list;
        /// otherwise, -1.
        /// </returns>
        public static int IndexOf(this IList<string> list, string item, bool ignoreCase)
        {
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (string.Compare(list[i], item, ignoreCase) == 0)
                        return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns a random element from the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to return an element from.</param>
        /// <returns>A random element from the list.</returns>
        public static T Sample<T>(this IList<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (list.Count == 0) return default(T);
            if (list.Count == 1) return list[0];

            var i = RNG.Next(0, list.Count);
            return list[i];
        }
    }
}
