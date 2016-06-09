using System;
using horsedrowner.Common;

namespace Kappa
{
    /// <summary>
    /// Provides the types of events that can be logged.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// Log.Add(Events.RawMessageReceived.With(message));
    /// ]]></code>
    /// </example>
    internal static class Events
    {
        /// <summary>
        /// A raw IRC message has been received.
        /// </summary>
        public static readonly LogEvent<string> RawMessageReceived
            = new LogEvent<string>(Strings.RawMessageReceived);

        /// <summary>
        /// A connection has been established.
        /// </summary>
        public static readonly LogEvent<System.Net.EndPoint> Connected
            = new LogEvent<System.Net.EndPoint>(Strings.Connected);
    }
}
