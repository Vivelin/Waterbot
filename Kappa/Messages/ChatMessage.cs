using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            Channel = new Channel(channel);
            Contents = contents;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> class.
        /// </summary>
        public ChatMessage(Channel channel, string contents) : base()
        {
            Channel = channel;
            Contents = contents;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> class
        /// with the contents of the parsed message.
        /// </summary>
        /// <param name="results">The parsed message.</param>
        protected internal ChatMessage(ParseResults results) : base(results)
        {
            if (Parameters.Count < 2)
                throw new ArgumentException(
                    "A chat message should always contain at least two parameters.",
                    nameof(results));

            Channel = new Channel(Parameters[0]);
            Contents = Parameters[1];

            DisplayName = Tags?.Get(MessageTags.DisplayName) ?? UserName;
            if (DisplayName.Length == 0) DisplayName = UserName;
        }

        /// <summary>
        /// Gets the Twitch channel that the message belongs to.
        /// </summary>
        public Channel Channel { get; }

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
        /// Gets a value indicating whether the user who sent the message is a
        /// Twitch admin.
        /// </summary>
        public bool IsAdmin
        {
            get
            {
                var userType = Tags?.Get(MessageTags.UserType);
                return userType == "admin";
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user who sent the message is the
        /// broadcaster.
        /// </summary>
        public bool IsBroadcaster
        {
            get
            {
                return string.Compare(UserName, Channel.Name, true) == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user who sent the message is a
        /// Twitch global moderator.
        /// </summary>
        public bool IsGlobalMod
        {
            get
            {
                var userType = Tags?.Get(MessageTags.UserType);
                return userType == "global_mod";
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user who sent the message is a
        /// moderator in the current channel.
        /// </summary>
        public bool IsMod
        {
            get
            {
                var userType = Tags?.Get(MessageTags.UserType);
                return userType == "mod" || userType == "global_mod";
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user who sent the message is
        /// part of the Twitch staff.
        /// </summary>
        public bool IsStaff
        {
            get
            {
                var userType = Tags?.Get(MessageTags.UserType);
                return userType == "staff";
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user who sent the message is
        /// subscribed to the current channel.
        /// </summary>
        public bool IsSub
        {
            get
            {
                var subscriber = Tags?.Get(MessageTags.Subscriber);
                return subscriber == "1";
            }
        }

        /// <summary>
        /// Gets the raw IRC command for sending this message.
        /// </summary>
        /// <returns>A string containing the IRC message to send.</returns>
        public override string ConstructCommand()
        {
            Command = Commands.PRIVMSG;

            Parameters.Clear();
            Parameters.Add(Channel.ToIrcChannel());
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
