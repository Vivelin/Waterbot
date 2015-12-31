using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Waterbot.Common;

namespace Kappa
{
    /// <summary>
    /// Represents a Twitch chat message.
    /// </summary>
    public class ChatMessage : Message
    {
        private string contents;
        private string normalizedContents;

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

            Channel = IrcUtil.UnescapeChannelName(parameters[0]);
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
        public string Contents
        {
            get { return contents; }
            protected set
            {
                contents = value;
                normalizedContents = StringUtils.Normalize(value);
            }
        }

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
            Parameters.Add(IrcUtil.EscapeChannelName(Channel));
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
            if (mention && !Mentions(UserName))
                text = $"@{DisplayName} {text}";

            return new ChatMessage(Channel, text);
        }

        /// <summary>
        /// Indicates whether the specified word is mentioned in this message.
        /// </summary>
        /// <param name="value">The word to seek.</param>
        /// <returns>
        /// <c>true</c> if the word is mentioned; otherwise, <c>false</c>.
        /// </returns>
        public bool Mentions(string value)
        {
            value = Regex.Escape(StringUtils.Normalize(value));
            return Regex.IsMatch(normalizedContents, $"\\b{value}\\b", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Indicates whether any of the specified words are mentioned in this
        /// message.
        /// </summary>
        /// <param name="values">A list of words to seek.</param>
        /// <returns>
        /// <c>true</c> if any of the words are mentioned; otherwise,
        /// <c>false</c>.
        /// </returns>
        public bool MentionsAny(IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                if (Mentions(value))
                    return true;
            }

            return false;
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
