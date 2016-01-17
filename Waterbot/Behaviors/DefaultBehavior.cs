using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            IdleCounts = new Dictionary<string, int>();
        }

        /// <summary>
        /// Gets a dictionary that keeps the current index in the idle messages
        /// cycle for each channel.
        /// </summary>
        protected IDictionary<string, int> IdleCounts { get; }

        /// <summary>
        /// Determines the bot's default response to notices and error messages.
        /// </summary>
        /// <param name="message">The notice that was received.</param>
        /// <returns>
        /// A <see cref="Task"/> objec representing the result of the
        /// asynchronous operation.
        /// </returns>
        public override Task<ChatMessage> GetFailureResponse(NoticeMessage message)
        {
            if (message.IsError)
            {
                var format = Config.Behavior.FailureMessages.Sample();
                var text = string.Format(format,
                    message.Channel, Config.Credentials.UserName, message.Text);

                var response = new ChatMessage(message.Channel, text);
                return Task.FromResult(response);
            }

            return Task.FromResult<ChatMessage>(null);
        }

        /// <summary>
        /// When overridden in a derived class, determines the bot's messages
        /// when idle in the specified channel.
        /// </summary>
        /// <param name="channel">The channel to send the message to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public override Task<ChatMessage> GetIdleMessage(Channel channel)
        {
            var n = Config.Behavior.IdleMessages.Count;
            var i = IdleCounts.Get(channel.Name);
            var text = Config.Behavior.IdleMessages[i];

            IdleCounts[channel.Name] = (i + 1) % n;

            var message = new ChatMessage(channel, text);
            return Task.FromResult(message);
        }

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
            var greeting = Config.Behavior.Greetings.Sample();
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
            var farewell = Config.Behavior.Farewells.Sample();
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
            var greeting = Config.Behavior.Greetings.Sample();
            var text = $"{greeting} {message.User}!";
            return message.CreateResponse(text);
        }

        /// <summary>
        /// Returns a command factory that is capable of creating the specified
        /// command.
        /// </summary>
        /// <param name="command">The command to create.</param>
        /// <returns>
        /// A command factory that is capable of creating the specified command;
        /// if no command factories are applicable, returns <c>null</c>.
        /// </returns>
        protected ICommandManufactorum GetFactory(string command)
        {
            var factories = Waterbot.EnumerateCommandFactories();
            foreach (var factorum in factories)
            {
                factorum.Configuration = Config;
                if (factorum.CanCreate(command))
                    return factorum;
            }

            return null;
        }

        /// <summary>
        /// Determines the bot's default response to the specified message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        protected override Task<ChatMessage> GetResponse(ChatMessage message)
        {
            if (message.Mentions(UserName))
            {
                if (message.MentionsAny(Config.Behavior.Greetings))
                    return Task.FromResult(Greet(message));

                var response = Config.Behavior.DefaultResponses.Sample();
                return Task.FromResult(message.CreateResponse(response, true));
            }

            return Task.FromResult<ChatMessage>(null);
        }

        /// <summary>
        /// Determines the bot's response to chat commands.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <param name="args">The command to handle.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        protected override async Task<ChatMessage> HandleCommand(ChatMessage message, string args)
        {
            var factorum = GetFactory(args.ToLowerInvariant());
            if (factorum != null)
            {
                var command = factorum.Create(args);
                return await command.GetResponse(message);
            }

            return null;
        }
    }
}
