using System;
using horsedrowner.Common;
using Kappa;

namespace Waterbot
{
    /// <summary>
    /// Provides the types of events that can be logged.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// Log.Add(Events.MuteEnabled.With(message));
    /// ]]></code>
    /// </example>
    internal static class Events
    {
        /// <summary>
        /// A user disabled Mute in a certain channel.
        /// </summary>
        public static readonly MessageLogEvent MuteDisabled
            = new MessageLogEvent(Strings.MuteDisabled);

        /// <summary>
        /// A user enabled Mute in a certain channel.
        /// </summary>
        public static readonly MessageLogEvent MuteEnabled
            = new MessageLogEvent(Strings.MuteEnabled);

        /// <summary>
        /// The bot caught itself trying to reply to its own messages.
        /// </summary>
        public static readonly LogEvent TalkingToMyself
            = new LogEvent(Strings.TalkingToMyself);
    }
}
