using System;

namespace Kappa
{
    /// <summary>
    /// Represents the message when a user joins a channel.
    /// </summary>
    public class JoinMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JoinMessage"/> class.
        /// </summary>
        /// <param name="name">The name of the channel to join.</param>
        public JoinMessage(string name) : this(new Channel(name))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to join.</param>
        public JoinMessage(Channel channel)
        {
            Channel = channel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinMessage"/> class
        /// with the contents of the parsed message.
        /// </summary>
        /// <param name="results">The parsed message.</param>
        protected internal JoinMessage(ParseResults results) : base(results)
        {
        }

        /// <summary>
        /// Gets the Twitch channel that was joined.
        /// </summary>
        public Channel Channel { get; }

        /// <summary>
        /// Gets the raw IRC command for sending this message.
        /// </summary>
        /// <returns>A string containing the IRC message to send.</returns>
        public override string ConstructCommand()
        {
            Command = Commands.JOIN;

            Parameters.Clear();
            Parameters.Add(Channel.ToIrcChannel());

            return base.ConstructCommand();
        }
    }
}
