using System;

namespace Kappa
{
    /// <summary>
    /// Provides data for the event that is raised when a message is sent or
    /// received.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventArgs"/>
        /// class for the specified message.
        /// </summary>
        /// <param name="message">The message that was sent or received.</param>
        public MessageEventArgs(Message message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the message that was sent or received.
        /// </summary>
        public Message Message { get; }
    }
}
