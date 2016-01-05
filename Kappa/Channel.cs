using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kappa
{
    /// <summary>
    /// Represents a channel on Twitch.
    /// </summary>
    [Serializable]
    public class Channel : TwitchApiObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class with
        /// the specified name.
        /// </summary>
        /// <param name="name">The name of the channel.</param>
        public Channel(string name)
        {
            Name = IrcUtil.UnescapeChannelName(name);
        }

        /// <summary>
        /// Gets the display name of the channel.
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; protected set; }

        /// <summary>
        /// Gets the name of the channel.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the Twitch API endpoint for streams.
        /// </summary>
        protected override string Endpoint => "channels/" + Name;

        /// <summary>
        /// Loads the stream data if the current channel is live; otherwise,
        /// returns <c>null</c>.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task<Stream> GetStream()
        {
            var stream = new Stream(this);
            await stream.Load();

            return stream;
        }

        /// <summary>
        /// Returns the name of the channel formatted to work in IRC.
        /// </summary>
        /// <returns>The name of the channel, prefixed with a '#'.</returns>
        public string ToIrcChannel()
        {
            return IrcUtil.EscapeChannelName(Name);
        }

        /// <summary>
        /// Returns a string that represents the current channel.
        /// </summary>
        /// <returns>A string representing the current channel.</returns>
        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
