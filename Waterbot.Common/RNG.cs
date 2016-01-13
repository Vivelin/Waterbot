using System;

namespace Waterbot.Common
{
    /// <summary>
    /// Provides a static, thread-safe way to get random numbers.
    /// </summary>
    public static class RNG
    {
        private static Random _global = new Random();

        [ThreadStatic]
        private static Random t_local;

        /// <summary>
        /// Gets a random number generator for the current thread.
        /// </summary>
        private static Random Local
        {
            get
            {
                if (t_local == null)
                {
                    int seed;
                    lock (_global)
                        seed = _global.Next();
                    t_local = new Random(seed);
                }

                return t_local;
            }
        }

        /// <summary>
        /// Returns a nonnegative random integer.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero and less than
        /// <see cref="int.MaxValue"/>.
        /// </returns>
        public static int Next()
        {
            return Local.Next();
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">
        /// The inclusive lower bound of the random number returned.
        /// </param>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number returned. <paramref
        /// name="maxValue"/> must be greater than or equal to <paramref
        /// name="minValue"/>.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to <paramref
        /// name="minValue"/> and less than <paramref name="maxValue"/>.
        /// </returns>
        public static int Next(int minValue, int maxValue)
        {
            return Local.Next(minValue, maxValue);
        }
    }
}
