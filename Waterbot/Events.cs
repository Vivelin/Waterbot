using System;
using Waterbot.Common;
using Kappa;

namespace Waterbot
{
    /// <summary>
    /// Represents the types of events that can be logged.
    /// </summary>
    /// <example>
    /// <code>
    /// Log.Add(Events.MuteEnabled.With(message));
    /// </code>
    /// </example>
    internal static class Events
    {
        /// <summary>
        /// Indicates a user disabled Mute in a certain channel.
        /// </summary>
        public static readonly MessageLogEvent MuteDisabled
            = new MessageLogEvent(Strings.MuteDisabled);

        /// <summary>
        /// Indicates a user enabled Mute in a certain channel.
        /// </summary>
        public static readonly MessageLogEvent MuteEnabled
            = new MessageLogEvent(Strings.MuteEnabled);

        /// <summary>
        /// Indicates the bot caught itself trying to reply to its own messages.
        /// </summary>
        public static readonly LogEvent TalkingToMyself
            = new LogEvent(Strings.TalkingToMyself);
    }
}
