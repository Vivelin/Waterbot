using System;
using System.Diagnostics;
using horsedrowner.Common;
using Kappa;

namespace Waterbot
{
    /// <summary>
    /// Represents an event to log, using a <see cref="T:Message"/> to specify
    /// more information.
    /// </summary>
    internal class MessageLogEvent : LogEvent<Message>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLogEvent"/>
        /// class for the specified message.
        /// </summary>
        /// <param name="format">
        /// The message to log, specified as a composite format string.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public MessageLogEvent(string format) : base(format) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLogEvent"/>
        /// class for the specified message, with the specified event type.
        /// </summary>
        /// <param name="format">
        /// The message to log, specified as a composite format string.
        /// </param>
        /// <param name="type">The type of event.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public MessageLogEvent(string format, EventType type) : base(format, type) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLogEvent"/>
        /// class for the specified message, with the specified event type.
        /// </summary>
        /// <param name="format">
        /// The message to log, specified as a composite format string.
        /// </param>
        /// <param name="type">The type of event.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public MessageLogEvent(string format, EventType type, int id) : base(format, type, id) { }

        /// <summary>
        /// Gets the message to log.
        /// </summary>
        public override string Message
        {
            get
            {
                if (LogObject == null)
                {
                    throw new InvalidOperationException(
                        Strings.InvalidOperation_Null.With(nameof(LogObject)));
                }

                return string.Format(Format, LogObject.User, LogObject.Command);
            }
        }

        /// <summary>
        /// Logs the event using the specified message.
        /// </summary>
        /// <param name="data">The message to log.</param>
        public override void Log(Message data)
        {
            Trace.TraceEvent((TraceEventType)Type, ID, Format, LogObject.User, LogObject.Command);
        }
    }
}
