using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa;

namespace Waterbot
{
    /// <summary>
    /// Provides methods that determine the bot's behavior. This is an abstract
    /// class.
    /// </summary>
    public abstract class Behavior
    {
        /// <summary>
        /// When overridden in a derived class, determines the bot's response to
        /// the specified message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public abstract ChatMessage GetResponse(ChatMessage message);
    }
}
