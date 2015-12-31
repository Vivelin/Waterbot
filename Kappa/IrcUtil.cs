using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa
{
    /// <summary>
    /// Provides common utility methods for interacting with Twitch chat IRC.
    /// </summary>
    public static class IrcUtil
    {
        /// <summary>
        /// Escapes the specified string as a channel name.
        /// </summary>
        /// <param name="channel">The channel name to escape.</param>
        /// <returns>
        /// The channel name, prefixed with a '#' if necessary.
        /// </returns>
        public static string EscapeChannelName(string channel)
        {
            if (channel == null) return null;
            if (channel.Length == 0) return string.Empty;

            if (channel[0] != '#')
                channel = '#' + channel;
            return channel;
        }

        /// <summary>
        /// Escapes the specified parameter so it can be used as the last
        /// parameter in the list.
        /// </summary>
        /// <param name="param">The parameter value to escape.</param>
        /// <returns>
        /// If the parameter value contains spaces, returns the parameter value
        /// prefixed with a colon; otherwise, returns the parameter value
        /// unchanged.
        /// </returns>
        public static string EscapeLastParam(string param)
        {
            if (param == null) return null;
            if (param.Length == 0) return ":";

            if (param[0] != ':' && param.IndexOf(' ') >= 0)
                param = ':' + param;
            return param;
        }

        /// <summary>
        /// Constructs a raw IRC message.
        /// </summary>
        /// <param name="command">The IRC command to send.</param>
        /// <param name="parameters">
        /// A string array containing the command parameters.
        /// </param>
        /// <returns>
        /// A string representing a raw IRC message for sending.
        /// </returns>
        public static string FormatMessage(string command, params string[] parameters)
        {
            return FormatMessage(command, new List<string>(parameters));
        }

        /// <summary>
        /// Constructs a raw IRC message.
        /// </summary>
        /// <param name="command">The IRC command to send.</param>
        /// <param name="parameters">
        /// A list containing the command parameters.
        /// </param>
        /// <returns>
        /// A string representing a raw IRC message for sending.
        /// </returns>
        public static string FormatMessage(string command, IList<string> parameters)
        {
            var raw = new StringBuilder();

            raw.Append(command.ToUpper());

            var i = 0;
            for (i = 0; i < (parameters.Count - 1); i++)
            {
                raw.Append(' ');
                raw.Append(parameters[i]);
            }

            raw.Append(' ');
            raw.Append(EscapeLastParam(parameters[i]));

            return raw.ToString();
        }

        /// <summary>
        /// Unescapes the specified channel name.
        /// </summary>
        /// <param name="channel">The channel name to unescape.</param>
        /// <returns>The name of the channel without a leading '#'.</returns>
        public static string UnescapeChannelName(string channel)
        {
            if (channel == null) return null;
            if (channel.Length == 0) return string.Empty;

            if (channel[0] == '#')
                channel = channel.Substring(1);
            return channel;
        }
    }
}
