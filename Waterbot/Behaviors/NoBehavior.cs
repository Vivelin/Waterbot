using System;
using System.Threading.Tasks;
using Kappa;

namespace Waterbot.Behaviors
{
    /// <summary>
    /// Represents a <see cref="Behavior"/> that does not do anything.
    /// </summary>
    public class NoBehavior : Behavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoBehavior"/> class
        /// using the specified configuration.
        /// </summary>
        /// <param name="config">The current configuration.</param>
        public NoBehavior(Configuration config) : base(config)
        {
        }

        /// <summary>
        /// Returns <c>null</c>.
        /// </summary>
        /// <param name="channel">The channel to not send messages to.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        public override Task<ChatMessage> GetIdleMessage(Channel channel)
        {
            // async: never again
            return Task.FromResult<ChatMessage>(null);
        }

        /// <summary>
        /// Returns <c>null</c>.
        /// </summary>
        /// <param name="channel">The channel that was joined.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        public override ChatMessage GetJoinMessage(Channel channel)
        {
            return null;
        }

        /// <summary>
        /// Returns <c>null</c>.
        /// </summary>
        /// <param name="channel">The channel that was left.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        public override ChatMessage GetPartMessage(Channel channel)
        {
            return null;
        }

        /// <summary>
        /// Returns <c>null</c>.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        public override Task<ChatMessage> ProcessMessage(ChatMessage message)
        {
            // async: never again
            return Task.FromResult<ChatMessage>(null);
        }

        /// <summary>
        /// Returns <c>null</c>.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        protected override Task<ChatMessage> GetResponse(ChatMessage message)
        {
            // async: never again
            return Task.FromResult<ChatMessage>(null);
        }

        /// <summary>
        /// Returns <c>null</c>.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <param name="command">The command to handle.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        protected override Task<ChatMessage> HandleCommand(ChatMessage message, string command)
        {
            // async: never again
            return Task.FromResult<ChatMessage>(null);
        }
    }
}
