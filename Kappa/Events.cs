using System;
using Waterbot.Common;

namespace Kappa
{
    /// <summary>
    /// Represents the types of events that can be logged.
    /// </summary>
    /// <example>
    /// <code>
    /// Log.Add(Events.RawMessageReceived.With(message));
    /// </code>
    /// </example>
    internal static class Events
    {
        /// <summary>
        /// Indicates a raw IRC message was received.
        /// </summary>
        public static readonly LogEvent<string> RawMessageReceived
            = new LogEvent<string>(Strings.RawMessageReceived);

        /// <summary>
        /// Indicates an IRC message was received but not handled.
        /// </summary>
        public static readonly LogEvent<string> UnhandledMessageReceived
            = new LogEvent<string>(Strings.UnhandledMessageReceived);

        /// <summary>
        /// Indicates a connection was established.
        /// </summary>
        public static readonly LogEvent<System.Net.EndPoint> Connected
            = new LogEvent<System.Net.EndPoint>(Strings.Connected);
    }
}
