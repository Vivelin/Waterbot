using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kappa
{
    /// <summary>
    /// Represents a stream on Twitch.
    /// </summary>
    [Serializable]
    public class Stream : TwitchApiObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Stream"/> class for the
        /// channel with the specified name.
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        public Stream(string channel)
            : this(new Channel(channel))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stream"/> class for the
        /// specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public Stream(Channel channel)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            Channel = channel;
        }

        /// <summary>
        /// Gets the channel of the stream.
        /// </summary>
        public Channel Channel { get; }

        /// <summary>
        /// Gets the name of the game being streamed.
        /// </summary>
        [JsonProperty("game")]
        public string Game { get; protected set; }

        /// <summary>
        /// Gets the start time of the stream.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime Started { get; protected set; }

        /// <summary>
        /// Gets the Twitch API endpoint for streams.
        /// </summary>
        protected override string Endpoint => "streams/" + Channel.Name;

        /// <summary>
        /// Gets the name of the JSON property that contains the actual data.
        /// </summary>
        protected override string PropertyName => "stream";

        /// <summary>
        /// Returns a string that represents the current stream.
        /// </summary>
        /// <returns>A string representing the current stream.</returns>
        public override string ToString()
        {
            if (Game != null)
                return string.Format("{0} playing {1}", Channel.Name, Game);
            return Channel.Name ?? base.ToString();
        }
    }
}
