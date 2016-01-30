using System;

namespace Waterbot.Common
{
    /// <summary>
    /// Represents an event to log.
    /// </summary>
    public class LogEvent : ILogEvent, ICloneable
    {
        private string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEvent"/> class.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public LogEvent(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Gets the message to log.
        /// </summary>
        public virtual string Message => _message;

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Returns a string representation of the event.
        /// </summary>
        /// <returns>A string representation of the event.</returns>
        public override string ToString()
        {
            return Message;
        }
    }

    /// <summary>
    /// Represents an event to log, using an object to specify more information.
    /// </summary>
    /// <typeparam name="T">
    /// The type of objects that contain information about the event.
    /// </typeparam>
    public class LogEvent<T> : ILogEvent, ICloneable
    {
        private string _format;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEvent{T}"/> class.
        /// </summary>
        /// <param name="format">
        /// The message to log, specified as a composite format string.
        /// </param>
        public LogEvent(string format)
        {
            _format = format;
        }

        /// <summary>
        /// Gets the message to log.
        /// </summary>
        public virtual string Message
        {
            get
            {
                return string.Format(Format, LogObject);
            }
        }

        /// <summary>
        /// Gets the message to log, specified as a composite format string.
        /// </summary>
        protected string Format => _format;

        /// <summary>
        /// Gets or sets an object that contains information about the event.
        /// </summary>
        protected T LogObject
        {
            get; set;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Returns a string representation of the event.
        /// </summary>
        /// <returns>A string representation of the event.</returns>
        public override string ToString()
        {
            return Message;
        }

        /// <summary>
        /// Creates a copy of the event, using the specified object that
        /// contains information about the event instance.
        /// </summary>
        /// <param name="value">
        /// The object containing information about the event instance.
        /// </param>
        /// <returns>A new <see cref="LogEvent{T}"/> object.</returns>
        public LogEvent<T> With(T value)
        {
            var clone = (LogEvent<T>)Clone();
            clone.LogObject = value;
            return clone;
        }
    }
}
