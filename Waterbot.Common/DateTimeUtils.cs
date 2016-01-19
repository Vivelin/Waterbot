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
        /// representation of the time difference.
        /// </summary>
        /// <param name="date">
        /// The <see cref="DateTime"/> object to compare.
        /// </param>
        /// <returns>A string representation of the time difference.</returns>
        public static string ToRelativeTimeString(this DateTime date)
        {
            var text = new StringBuilder();

            if (DateTime.Now > date)
            {
                var elapsed = DateTime.Now - date;
                if (elapsed.TotalSeconds >= 2)
                    text.AppendFormat("{0} ago", elapsed.ToText());
                else
                    text.Append("just now");
            }
            else if (DateTime.Now > date)
            {
                var interval = date - DateTime.Now;
                if (interval.TotalSeconds >= 2)
                    text.AppendFormat("in {0}", interval.ToText());
                else
                    text.Append("about now");
            }
            else
            {
                text.Append("now");
            }

            return text.ToString();
        }
    }
}
