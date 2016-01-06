using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kappa
{
    /// <summary>
    /// Represents the result of a GET streams/:channel API call.
    /// </summary>
    internal class StreamResults : TwitchApiObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamResults"/> class
        /// for the specified channel.
        /// </summary>
        /// <param name="channel">The channel to check.</param>
        public StreamResults(Channel channel)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            Channel = channel;
        }

        /// <summary>
        /// Gets the channel to check.
        /// </summary>
        public Channel Channel { get; }

        /// <summary>
        /// Gets the returned <see cref="Stream"/> object.
        /// </summary>
        [JsonProperty("stream")]
        public Stream Stream { get; protected set; }

        /// <summary>
        /// Gets the Twitch API endpoint for streams.
        /// </summary>
        protected override string Endpoint => "streams/" + Channel.Name;

        /// <summary>
        /// Populates the current object with the specified data.
        /// </summary>
        /// <param name="data">The data to populate the object with.</param>
        protected override void Populate(string data)
        {
            base.Populate(data);

            if (Stream != null)
            {
                Stream.Loaded = Loaded;
                if (Stream.Channel != null)
                    Stream.Channel.Loaded = Loaded;
            }
        }
    }
}
