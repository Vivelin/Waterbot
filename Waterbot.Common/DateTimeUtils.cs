using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waterbot.Common
{
    /// <summary>
    /// Provides additional functionality for interacting with dates.
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// Compares a date to the current time and returns a text
        /// representation of the elapsed time.
        /// </summary>
        /// <param name="date">
        /// The <see cref="DateTime"/> object to compare.
        /// </param>
        /// <returns>A string representation of the elapsed time.</returns>
        public static string ToRelativeTimeString(this DateTime date)
        {
            var elapsed = DateTime.Now - date;
            return elapsed.ToText();
        }
    }
}
