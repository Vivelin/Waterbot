using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using horsedrowner.Common;

namespace Kappa
{
    /// <summary>
    /// Represents a Twitch whisper message.
    /// </summary>
    public class Whisper : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Whisper"/> class.
        /// </summary>
        /// <param name="target">
        /// The name of the user to send a whisper to.
        /// </param>
        /// <param name="contents">The contents of the message.</param>
        public Whisper(string target, string contents)
        {
            Target = new User(target);
            Contents = contents;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Whisper"/> class.
        /// </summary>
        /// <param name="target">The user to send a whisper to.</param>
        /// <param name="contents">The contents of the message.</param>
        public Whisper(User target, string contents)
        {
            Target = target;
            Contents = contents;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Whisper"/> class with
        /// the contents of the parsed message.
        /// </summary>
        /// <param name="results">The parsed message.</param>
        protected internal Whisper(ParseResults results) : base(results)
        {
            if (Parameters.Count < 2)
            {
                throw new ArgumentException(Strings.Arg_WhisperParamCount,
                    nameof(results));
            }

            Target = new User(Parameters[0]);
            Contents = Parameters[1];

            var displayName = Tags?.Get(MessageTags.DisplayName);
            if (!string.IsNullOrEmpty(displayName))
                User.Name = displayName;
        }

        /// <summary>
        /// Gets the contents of the whisper.
        /// </summary>
        public string Contents { get; protected set; }

        /// <summary>
        /// Gets the target of the whisper.
        /// </summary>
        public User Target { get; }

        /// <summary>
        /// Gets the raw IRC command for sending this whisper.
        /// </summary>
        /// <returns>A string containing the IRC message to send.</returns>
        public override string ConstructCommand()
        {
            // :tmi.twitch.tv 421 kusogechan WHISPER :Unknown command
            return IrcUtil.FormatMessage(Commands.PRIVMSG,
                IrcUtil.EscapeChannelName("kusogechan"),
                $".w {Target.Name} {Contents}"
            );
        }

        /// <summary>
        /// Gets the raw IRC command for sending this whisper, using the
        /// specified function to format the contents of the whisper.
        /// </summary>
        /// <remarks>
        /// Calling this overload will permanently format the whisper contents.
        /// </remarks>
        /// <param name="formatter">
        /// A function that can be used to format the contents of the whisper
        /// before it is sent.
        /// </param>
        /// <returns>A string containing the IRC message to send.</returns>
        public virtual string ConstructCommand(Func<string, string> formatter)
        {
            Contents = formatter(Contents);
            return ConstructCommand();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Whisper"/> class with the
        /// specified response text.
        /// </summary>
        /// <param name="text">The text to respond to the whisper with.</param>
        /// <returns>
        /// A new <see cref="Whisper"/> object, with the message sender as
        /// target.
        /// </returns>
        public Whisper CreateResponse(string text)
        {
            return new Whisper(User, text);
        }

        /// <summary>
        /// Indicates whether the specified word is mentioned in this whisper.
        /// </summary>
        /// <param name="value">The word to seek.</param>
        /// <returns>
        /// <c>true</c> if the word is mentioned; otherwise, <c>false</c>.
        /// </returns>
        public bool Mentions(string value)
        {
            var contents = StringUtils.Normalize(Contents);
            value = Regex.Escape(StringUtils.Normalize(value));
            return Regex.IsMatch(contents, $"\\b{value}\\b", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Indicates whether any of the specified words are mentioned in this
        /// whisper.
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
        /// Returns a string representing the whisper.
        /// </summary>
        /// <returns>A string that represents this instance.</returns>
        public override string ToString()
        {
            return $"{User} -> {Target}: {Contents}";
        }
    }
}
