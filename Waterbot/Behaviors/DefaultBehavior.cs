using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa;

namespace Waterbot
{
    /// <summary>
    /// Provides methods that determine the bot's default behavior.
    /// </summary>
    public class DefaultBehavior : Behavior
    {
        /// <summary>
        /// Determines the bot's default response to the specified message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public override ChatMessage GetResponse(ChatMessage message)
        {
            return null;
        }
    }
}
