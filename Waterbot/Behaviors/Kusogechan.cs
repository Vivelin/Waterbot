using System;
using System.Collections.Generic;
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
        /// Initializes a new instance of the <see cref="Kusogechan"/> class
        /// using the specified configuration.
        /// </summary>
        /// <param name="config">The current configuration.</param>
        public Kusogechan(Configuration config) : base(config)
        {
            Echo = Config.Behavior["Echo"] ?? new PhraseSet { "JediRosh", "AWOOOO", "Poi!" };
        }

        /// <summary>
        /// Gets a list of words to echo.
        /// </summary>
        public IList<string> Echo { get; }

        /// <summary>
        /// Determines Kusoge-chan's response to the specified message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        protected override async Task<ChatMessage> GetResponse(ChatMessage message)
        {
            foreach (var item in Echo)
            {
                if (message.Mentions(item))
                    return message.CreateResponse(item);
            }

            // Fall back to default behavior
            return await base.GetResponse(message);
        }

        /// <summary>
        /// Determines Kusoge-chan's response to chat commands.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <param name="command">The command to handle.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        protected override async Task<ChatMessage> HandleCommand(ChatMessage message, string command)
        {
            return await base.HandleCommand(message, command);
        }
    }
}
