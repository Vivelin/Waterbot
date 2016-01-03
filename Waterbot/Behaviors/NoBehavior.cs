using System;
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
        /// <param name="channel">The channel that was joined.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        public override ChatMessage GetJoinMessage(string channel)
        {
            return null;
        }

        /// <summary>
        /// Returns <c>null</c>.
        /// </summary>
        /// <param name="channel">The channel that was left.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        public override ChatMessage GetPartMessage(string channel)
        {
            return null;
        }

        /// <summary>
        /// Returns <c>null</c>.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        public override ChatMessage ProcessMessage(ChatMessage message)
        {
            return null;
        }

        /// <summary>
        /// Returns <c>null</c>.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        protected override ChatMessage GetResponse(ChatMessage message)
        {
            return null;
        }

        /// <summary>
        /// Returns <c>null</c>.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <param name="command">The command to handle.</param>
        /// <returns>Always returns <c>null</c>.</returns>
        protected override ChatMessage HandleCommand(ChatMessage message, string command)
        {
            return null;
        }
    }
}
