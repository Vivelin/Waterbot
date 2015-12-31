using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa;
using Waterbot.Common;

namespace Waterbot.Behaviors
{
    /// <summary>
    /// Provides methods that determine the bot's behavior as Kusoge-chan.
    /// </summary>
    public class Kusogechan : DefaultBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Kusogechan"/> class
        /// using the specified user name.
        /// </summary>
        /// <param name="userName">The user name of the bot.</param>
        public Kusogechan(string userName) : base(userName)
        {
            DefaultResponses.Add("Where are my video games?");

            Farewells.Clear();
            Farewells.Add("See ya nerds!");
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
            string[] echo = { "JediRosh", "AWOOOO", "Poi!" };
            foreach (var item in echo)
            {
                if (message.Contents.Contains(item))
                    return message.CreateResponse(item);
            }

            // Fall back to default behavior
            return base.GetResponse(message);
        }
    }
}
