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
        /// Concatenates all the elements of a string array, using the specified
        /// separator between each element and ignoring elements that are empty
        /// or <c>null</c>.
        /// </summary>
        /// <param name="separator">The string to use as a separator.</param>
        /// <param name="args">
        /// An array that contains the elements to concatenate.
        /// </param>
        /// <returns>
        /// A string that consists of the elements in <paramref name="args"/>
        /// delimited by the separator string. If <paramref name="args"/> is an
        /// empty array, the method returns an empty string.
        /// </returns>
        public static string Join(string separator, params string[] args)
        {
            if (args == null || args.Length == 0) return string.Empty;
            if (args.Length == 1) return args[0];

            var values = args.Where(x => !string.IsNullOrEmpty(x));
            return string.Join(separator, values);
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

        /// <summary>
        /// Replaces one or more format items in the string with the string
        /// representation of a specified object.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The object to format.</param>
        /// <returns>
        /// A copy of the string in which any format items are replaced by the
        /// string representation of <paramref name="arg0"/>.
        /// </returns>
        public static string With(this string format, object arg0)
        {
            return string.Format(format, arg0);
        }

        /// <summary>
        /// Replaces the format item in the string with the string
        /// representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">
        /// An object array that contains zero or more objects to format.
        /// </param>
        /// <returns>
        /// A copy of the string in which the format items have been replaced by
        /// the string representation of the corresponding objects in <paramref
        /// name="args"/>.
        /// </returns>
        public static string With(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}
