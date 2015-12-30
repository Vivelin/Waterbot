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
        public ChatMessage(string channel, string contents) : base()
        {
            Channel = channel;
            Contents = contents;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> class
        /// with the contents of the parsed message.
        /// </summary>
        /// <param name="message">The raw IRC message.</param>
        /// <param name="tags">A dictionary containing the message tags.</param>
        /// <param name="prefix">The message prefix.</param>
        /// <param name="command">The message command.</param>
        /// <param name="parameters">The parameters of the command.</param>
        protected internal ChatMessage(string message,
            IReadOnlyDictionary<string, string> tags,
            string prefix,
            string command,
            IList<string> parameters)
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
        /// Gets the Twitch channel that the message belongs to.
        /// </summary>
        public string Channel { get; }

        /// <summary>
        /// Gets the contents of the chat message.
        /// </summary>
        public string Contents { get; }

        /// <summary>
        /// Gets the user's display name.
        /// </summary>
        public string DisplayName { get; protected internal set; }

        /// <summary>
        /// Gets the raw IRC command for sending this message.
        /// </summary>
        /// <returns>A string containing the IRC message to send.</returns>
        public override string ConstructCommand()
        {
            Command = Commands.PRIVMSG;

            Parameters.Clear();
            Parameters.Add(TwitchUtil.EscapeChannelName(Channel));
            Parameters.Add(Contents);

            return base.ConstructCommand();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ChatMessage"/> class with
        /// the specified response text.
        /// </summary>
        /// <param name="text">The text to respond to the message with.</param>
        /// <returns>A new <see cref="ChatMessage"/> object.</returns>
        public ChatMessage CreateResponse(string text)
        {
            return new ChatMessage(Channel, text);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ChatMessage"/> class with
        /// the specified response text, optionally including the name of the
        /// sender in the response.
        /// </summary>
        /// <param name="text">The text to respond to the message with.</param>
        /// <param name="mention">
        /// Indicates whether to mention the sender of the message in the
        /// response.
        /// </param>
        /// <returns>A new <see cref="ChatMessage"/> object.</returns>
        public ChatMessage CreateResponse(string text, bool mention)
        {
            if (mention && !text.Contains(UserName, ignoreCase: true))
                text = $"@{DisplayName} {text}";

            return new ChatMessage(Channel, text);
        }

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
