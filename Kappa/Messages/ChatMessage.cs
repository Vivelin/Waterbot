using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa
{
    /// <summary>
    /// Represents a Twitch chat message.
    /// </summary>
    public class ChatMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> class.
        /// </summary>
        /// <param name="message">The raw IRC message.</param>
        /// <param name="tags">A dictionary containing the message tags.</param>
        /// <param name="prefix">The message prefix.</param>
        /// <param name="command">The message command.</param>
        /// <param name="parameters">The parameters of the command.</param>
        public ChatMessage(string message,
            IReadOnlyDictionary<string, string> tags,
            string prefix,
            string command,
            IReadOnlyList<string> parameters)
            : base(message, tags, prefix, command, parameters)
        {
            if (parameters.Count < 2)
                throw new ArgumentException(
                    "A chat message should always contain at least two parameters.",
                    nameof(parameters));

            Channel = TwitchUtil.UnescapeChannelName(parameters[0]);
            Contents = parameters[1];
            DisplayName = tags?.Get(MessageTags.DisplayName) ?? UserName;
        }

        /// <summary>
        /// Gets the channel in which the chat message was sent.
        /// </summary>
        public string Channel { get; }

        /// <summary>
        /// Gets the contents of the chat message.
        /// </summary>
        public string Contents { get; }

        /// <summary>
        /// Gets the user's display name.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Returns a string representing the chat message.
        /// </summary>
        /// <returns>A string that represents this instance.</returns>
        public override string ToString()
        {
            return $"{DisplayName}: {Contents}";
        }
    }
}
