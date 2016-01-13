using System.Collections.ObjectModel;
using Waterbot.Common;

namespace Waterbot
{
    /// <summary>
    /// Represents a set of phrases.
    /// </summary>
    public class PhraseSet : Collection<string>
    {
        /// <summary>
        /// Selects a random phrase from the set.
        /// </summary>
        /// <returns>A random phrase from the set.</returns>
        public string Sample()
        {
            if (Count == 0) return null;
            if (Count == 1) return this[0];

            var i = RNG.Next(0, Count);
            return this[i];
        }

        /// <summary>
        /// Selects a random phrase from the set.
        /// </summary>
        /// <returns>A random phrase from the set.</returns>
        public override string ToString()
        {
            return Sample();
        }
    }
}
