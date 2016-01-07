using System;
using Kappa;

namespace Waterbot
{
    /// <summary>
    /// Provides data for the events that are raised when a channel is joined or
    /// left.
    /// </summary>
    public class ChannelEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelEventArgs"/>
        /// class using the specified channel.
        /// </summary>
        /// <param name="channel">The channel that was joined or left.</param>
        public ChannelEventArgs(Channel channel)
        {
            Channel = channel;
        }

        /// <summary>
        /// Gets the channel that was joined or left.
        /// </summary>
        public Channel Channel { get; }
    }
}
