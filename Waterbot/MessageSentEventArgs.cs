using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waterbot
{
    /// <summary>
    /// Provides data for the event that is raised when a chat message is sent.
    /// </summary>
    public class MessageSentEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSentEventArgs"/>
        /// class with the specified channel name and message.
        /// </summary>
        /// <param name="channel">The name of the channel.</param>
        /// <param name="message">The contents of the message.</param>
        public MessageSentEventArgs(string channel, string message)
        {
            Channel = channel;
            Contents = message;
        }

        /// <summary>
        /// Gets the channel name that the message was sent to.
        /// </summary>
        public string Channel { get; }

        /// <summary>
        /// Gets the contents of the message that was sent.
        /// </summary>
        public string Contents { get; }
    }
}
