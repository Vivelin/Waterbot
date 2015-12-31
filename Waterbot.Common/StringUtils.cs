using System;
using System.Linq;

namespace Waterbot.Common
{
    /// <summary>
    /// Provides additional functionality for interacting with strings.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Returns a value indicating whether a specified substring occurs
        /// within this string, ignoring or honing their case.
        /// </summary>
        /// <param name="self">The string to find the substring in.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="ignoreCase">
        /// <c>true</c> to ignore case during comparison; otherwise,
        /// <c>false</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if this string contains the specified substring;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this string self, string value, bool ignoreCase)
        {
            if (ignoreCase)
            {
                self = self.ToLowerInvariant();
                value = value.ToLowerInvariant();
            }

            return self.Contains(value);
        }

        /// <summary>
        /// Normalizes a string for comparison by stripping punctuation.
        /// </summary>
        /// <param name="value">The string to normalize.</param>
        /// <returns>A string that does not contain punctuation.</returns>
        public static string Normalize(string value)
        {
            char[] keep = value.Where(c => !Char.IsPunctuation(c)).ToArray();
            return new string(keep);
        }
    }
}
