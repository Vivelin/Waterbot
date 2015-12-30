using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa
{
    /// <summary>
    /// Provides data for the event that is raised when a chat message is sent
    /// or received.
    /// </summary>
    public class ChatMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageEventArgs"/>
        /// class for the specified message.
        /// </summary>
        /// <param name="message">The message that was sent or received.</param>
        public ChatMessageEventArgs(ChatMessage message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the chat message that was sent or received.
        /// </summary>
        public ChatMessage Message { get; }
    }
}
