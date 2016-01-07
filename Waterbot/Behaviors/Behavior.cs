using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        /// When overridden in a derived class, initializes a new instance of
        /// the <see cref="Behavior"/> class using the specified configuration.
        /// </summary>
        /// <param name="config">The current configuration.</param>
        protected Behavior(Configuration config)
        {
            Config = config;

            var user = Regex.Escape(UserName);
            CommandPatterns = new List<Regex>()
            {
                new Regex($"^@?{user}\\s+!?(?<command>.+)$", RegexOptions.IgnoreCase), // @username command
                new Regex($"^!(?<command>.+?)(\\s+@?{user})?$", RegexOptions.IgnoreCase), // !command [@username]
                new Regex($"^!?(?<command>.+?)\\s+@?{user}$", RegexOptions.IgnoreCase) // [!]command @username
            };
        }

        /// <summary>
        /// Gets the current configuration.
        /// </summary>
        public Configuration Config { get; }

        /// <summary>
        /// Gets the name of the user the bot responds to.
        /// </summary>
        public string UserName
        {
            get { return Config.Credentials.UserName; }
        }

        /// <summary>
        /// Gets a list of regular expressions used to parse incoming messages
        /// for commands.
        /// </summary>
        protected IList<Regex> CommandPatterns { get; }

        /// <summary>
        /// When overridden in a derived class, determines the bot's messages
        /// when idle in the specified channel.
        /// </summary>
        /// <param name="channel">The channel to send the message to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public abstract Task<ChatMessage> GetIdleMessage(Channel channel);

        /// <summary>
        /// When overridden in a derived class, determines the bot's message
        /// when joining a channel.
        /// </summary>
        /// <param name="channel">The channel that was joined.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public virtual ChatMessage GetJoinMessage(string channel)
        {
            return null;
        }

        /// <summary>
        /// When overridden in a derived class, determines the bot's message
        /// when leaving a channel.
        /// </summary>
        /// <param name="channel">The channel that was left.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public virtual ChatMessage GetPartMessage(string channel)
        {
            return null;
        }

        /// <summary>
        /// Determine's the bot's response to the specified message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        /// <remarks>
        /// This method first attempts to process any commands contained within
        /// the message by calling <see cref="HandleCommand"/>. If processing
        /// the command did not result in a response, the method <see
        /// cref="GetResponse"/> will be used to determine the bot's response.
        /// </remarks>
        public virtual async Task<ChatMessage> ProcessMessage(ChatMessage message)
        {
            var command = GetCommand(message);
            if (!string.IsNullOrEmpty(command))
            {
                if (Config.Behavior.CommandAliases.ContainsKey(command))
                    command = Config.Behavior.CommandAliases[command];

                var response = await HandleCommand(message, command);
                if (response != null)
                    return response;
            }

            return await GetResponse(message);
        }

        /// <summary>
        /// Returns a value indicating the command contained in the specified
        /// message, if any.
        /// </summary>
        /// <param name="message">The message to process.</param>
        /// <returns>
        /// A string value indicating the command given to the bot, or
        /// <c>null</c> if the specified message did not seem to be a command.
        /// </returns>
        protected virtual string GetCommand(ChatMessage message)
        {
            foreach (var regex in CommandPatterns)
            {
                var match = regex.Match(message.Contents);
                if (match.Success)
                {
                    var command = match.Groups["command"];
                    if (command.Success)
                    {
                        return command.Value.Trim();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// When overridden in a derived class, determines the bot's response to
        /// the specified message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        protected abstract Task<ChatMessage> GetResponse(ChatMessage message);

        /// <summary>
        /// When overridden in a derived class, determines the bot's response to
        /// chat commands.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <param name="command">The command to handle.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        protected abstract Task<ChatMessage> HandleCommand(ChatMessage message, string command);
    }
}
