using System;
using System.Collections.Generic;

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
        /// Returns a random element from the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to return an element from.</param>
        /// <returns>A random element from the list.</returns>
        public static T Sample<T>(this IList<T> list)
        {
            var rng = new Random();
            return list.Sample(rng);
        }

        /// <summary>
        /// Returns a random element from the list using the specified random
        /// number generator.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to return an element from.</param>
        /// <param name="rng">The random number generator to use.</param>
        /// <returns>A random element from the list.</returns>
        public static T Sample<T>(this IList<T> list, Random rng)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (list.Count == 0) return default(T);
            if (list.Count == 1) return list[0];

            var i = rng.Next(0, list.Count);
            return list[i];
        }
    }
}
