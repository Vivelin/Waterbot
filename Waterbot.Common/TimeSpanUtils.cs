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
                builder.AppendFormat(Strings.AboutXDays, days);

                var hours = time.TotalHours - days * 24;
                if (hours > 1)
                    builder.AppendFormat(Strings.AndXHours, hours);
                if (hours == 1)
                    builder.Append(Strings.AndOneHour);
            }
            else if (time.TotalHours >= 2)
            {
                var hours = Math.Floor(time.TotalHours);
                builder.AppendFormat(Strings.AboutXHours, hours);

                var minutes = time.TotalMinutes - hours * 60;
                if (minutes > 1)
                    builder.AppendFormat(Strings.AndXMinutes, minutes);
                if (minutes == 1)
                    builder.Append(Strings.AndOneMinute);
            }
            else if (time.TotalMinutes >= 2)
            {
                var minutes = Math.Floor(time.TotalMinutes);
                builder.AppendFormat(Strings.AboutXMinutes, minutes);

                var seconds = time.TotalSeconds - minutes * 60;
                if (seconds > 1)
                    builder.AppendFormat(Strings.AndXSeconds, seconds);
                if (seconds == 1)
                    builder.Append(Strings.AndOneSecond);
            }
            else if (time.TotalSeconds >= 2)
            {
                var seconds = Math.Floor(time.TotalSeconds);
                builder.AppendFormat(Strings.AboutXSeconds, seconds);
            }
            else
            {
                builder.AppendFormat(Strings.FewSecs);
            }

            return builder.ToString();
        }
    }
}
