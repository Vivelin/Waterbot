using System;
using Kappa;
using Waterbot.Common;

namespace Kappa
{
    /// <summary>
    /// Represents an event to log, using a <see cref="T:Message"/> to specify
    /// more information.
    /// </summary>
    public class MessageLogEvent : LogEvent<Message>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLogEvent"/>
        /// class.
        /// </summary>
        /// <param name="format">
        /// The message to log, specified as a composite format string.
        /// </param>
        public MessageLogEvent(string format) : base(format) { }

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
    }
}
