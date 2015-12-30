using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa;

namespace Waterbot.Behaviors
{
    /// <summary>
    /// Provides methods that determine the bot's behavior as Kusoge-chan.
    /// </summary>
    public class Kusogechan : DefaultBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Kusogechan"/> class.
        /// </summary>
        public Kusogechan()
        {
        }

        /// <summary>
        /// Determines Kusoge-chan's response to the specified message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public override ChatMessage GetResponse(ChatMessage message)
        {
            if (message.Contents.Contains("JediRosh"))
                return message.CreateResponse("JediRosh");
            return base.GetResponse(message);
        }
    }
}
