using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waterbot.Common
{
    /// <summary>
    /// Provides additional functionality for interacting with timespans.
    /// </summary>
    public static class TimeSpanUtils
    {
        /// <summary>
        /// Returns a string representing the time interval in text.
        /// </summary>
        /// <param name="time">The <see cref="TimeSpan"/> object to use.</param>
        /// <returns>A string representation of the time interval.</returns>
        public static string ToText(this TimeSpan time)
        {
            var builder = new StringBuilder();
            if (time.TotalDays >= 2)
            {
                var days = Math.Floor(time.TotalDays);
                builder.AppendFormat("about {0:N0} days", days);

                var hours = time.TotalHours - days * 24;
                if (hours > 1)
                    builder.AppendFormat(" and {0:N0} hours", hours);
                if (hours == 1)
                    builder.Append(" and an hour");
            }
            else if (time.TotalHours >= 2)
            {
                var hours = Math.Floor(time.TotalHours);
                builder.AppendFormat("about {0:N0} hours", hours);

                var minutes = time.TotalMinutes - hours * 60;
                if (minutes > 1)
                    builder.AppendFormat(" and {0:N0} minutes", minutes);
                if (minutes == 1)
                    builder.Append(" and one minute");
            }
            else if (time.TotalMinutes >= 2)
            {
                var minutes = Math.Floor(time.TotalMinutes);
                builder.AppendFormat("about {0:N0} minutes", minutes);

                var seconds = time.TotalSeconds - minutes * 60;
                if (seconds > 1)
                    builder.AppendFormat(" and {0:N0} seconds", seconds);
                if (seconds == 1)
                    builder.Append(" and one second");
            }
            else if (time.TotalSeconds >= 2)
            {
                var seconds = Math.Floor(time.TotalSeconds);
                builder.AppendFormat("about {0:N0} seconds", seconds);
            }
            else
            {
                builder.AppendFormat("a couple of seconds");
            }

            return builder.ToString();
        }
    }
}
