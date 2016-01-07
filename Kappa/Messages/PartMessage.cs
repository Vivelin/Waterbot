using System;

namespace Kappa
{
    /// <summary>
    /// Represents the message when a user leaves a channel.
    /// </summary>
    public class PartMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartMessage"/> class.
        /// </summary>
        /// <param name="name">The name of the channel to leave.</param>
        public PartMessage(string name) : this(new Channel(name))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to leave.</param>
        public PartMessage(Channel channel)
        {
            Channel = channel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartMessage"/> class
        /// with the contents of the parsed message.
        /// </summary>
        /// <param name="results">The parsed message.</param>
        protected internal PartMessage(ParseResults results) : base(results)
        {
        }

        /// <summary>
        /// Gets the Twitch channel that was left.
        /// </summary>
        public Channel Channel { get; }

        /// <summary>
        /// Gets the raw IRC command for sending this message.
        /// </summary>
        /// <returns>A string containing the IRC message to send.</returns>
        public override string ConstructCommand()
        {
            Command = Commands.PART;

            Parameters.Clear();
            Parameters.Add(Channel.ToIrcChannel());

            return base.ConstructCommand();
        }
    }
}
