using System;
using Kappa;
using Waterbot.Common;

namespace Waterbot
{
    /// <summary>
    /// Provides methods that determine the bot's default behavior.
    /// </summary>
    public class DefaultBehavior : Behavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBehavior"/>
        /// class using the specified configuration.
        /// </summary>
        /// <param name="config">The current configuration.</param>
        public DefaultBehavior(Configuration config) : base(config)
        {
            RNG = new Random();
        }

        /// <summary>
        /// Gets a random number generator for this instance.
        /// </summary>
        protected Random RNG { get; }

        /// <summary>
        /// Determines the bot's message when joining a channel.
        /// </summary>
        /// <param name="channel">The channel that was joined.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public override ChatMessage GetJoinMessage(string channel)
        {
            var greeting = Config.Behavior.Greetings.Sample(RNG);
            var text = $"{greeting} chat!";
            return new ChatMessage(channel, text);
        }

        /// <summary>
        /// Determines the bot's message when leaving a channel.
        /// </summary>
        /// <param name="channel">The channel that was left.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public override ChatMessage GetPartMessage(string channel)
        {
            var farewell = Config.Behavior.Farewells.Sample(RNG);
            return new ChatMessage(channel, farewell);
        }

        /// <summary>
        /// Returns a message that greets the user in response to a message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with.
        /// </returns>
        public virtual ChatMessage Greet(ChatMessage message)
        {
            var greeting = Config.Behavior.Greetings.Sample(RNG);
            var text = $"{greeting} {message.DisplayName}!";
            return message.CreateResponse(text);
        }

        /// <summary>
        /// Determines the bot's default response to the specified message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        protected override ChatMessage GetResponse(ChatMessage message)
        {
            if (message.Mentions(UserName))
            {
                if (message.MentionsAny(Config.Behavior.Greetings))
                    return Greet(message);

                var response = Config.Behavior.DefaultResponses.Sample(RNG);
                return message.CreateResponse(response, true);
            }

            return null;
        }

        /// <summary>
        /// Determines the bot's response to chat commands.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <param name="command">The command to handle.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        protected override ChatMessage HandleCommand(ChatMessage message, string command)
        {
            switch (command.ToLowerInvariant())
            {
                case "uptime":
                    return Uptime(message);

                default:
                    if (Config.Behavior.StaticCommands.ContainsKey(command))
                    {
                        var response = Config.Behavior.StaticCommands[command];
                        return message.CreateResponse(response);
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// Determines the bot's response to the "uptime" command.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        protected virtual ChatMessage Uptime(ChatMessage message)
        {
            var channel = message.Channel;

            var startTime = DateTime.MinValue;
            var elapsedTime = (DateTime.Now - startTime).ToRelativeTimeString();

            var responseFormat = "{0} started streaming {1} (since {2:g})";
            var response = string.Format(responseFormat, channel, elapsedTime, startTime);
            return message.CreateResponse(response);
        }
    }
}
