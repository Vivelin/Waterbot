using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa
{
    /// <summary>
    /// Extends .NET framework classes with some much-needed methods.
    /// </summary>
    public static class Extensions
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
    }
}
