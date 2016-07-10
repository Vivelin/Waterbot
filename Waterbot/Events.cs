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
    /// Events.MuteEnabled.Log(message);
    /// ]]></code>
    /// </example>
    internal static class Events
    {
        /// <summary>
        /// A user disabled Mute in a certain channel.
        /// </summary>
        public static readonly MessageLogEvent MuteDisabled
            = new MessageLogEvent(Strings.MuteDisabled, EventType.Info, 8001);

        /// <summary>
        /// A user enabled Mute in a certain channel.
        /// </summary>
        public static readonly MessageLogEvent MuteEnabled
            = new MessageLogEvent(Strings.MuteEnabled, EventType.Info, 8002);

        /// <summary>
        /// The bot caught itself trying to reply to its own messages.
        /// </summary>
        public static readonly LogEvent TalkingToMyself
            = new LogEvent(Strings.TalkingToMyself, EventType.Info, 8003);

        /// <summary>
        /// No custom behavior is defined for the specified username.
        /// </summary>
        public static readonly LogEvent<string> BehaviorFallback
            = new LogEvent<string>(Strings.BehaviorFallback, EventType.Info, 8004);
    }
}
