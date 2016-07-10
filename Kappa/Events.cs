using System;
using horsedrowner.Common;

namespace Kappa
{
    /// <summary>
    /// Provides the types of events that can be logged.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// Events.RawMessageReceived.Log(message);
    /// ]]></code>
    /// </example>
    internal static class Events
    {
        /// <summary>
        /// A raw IRC message has been received.
        /// </summary>
        public static readonly LogEvent<string> RawMessageReceived
            = new LogEvent<string>(Strings.RawMessageReceived, EventType.Verbose, 1);

        /// <summary>
        /// A notice message has been received.
        /// </summary>
        public static readonly LogEvent<string> NoticeReceived
            = new LogEvent<string>(Strings.NoticeReceived, EventType.Info, 3);

        /// <summary>
        /// A connection has been established.
        /// </summary>
        public static readonly LogEvent<System.Net.EndPoint> Connected
            = new LogEvent<System.Net.EndPoint>(Strings.Connected, EventType.Info, 2);
    }
}
