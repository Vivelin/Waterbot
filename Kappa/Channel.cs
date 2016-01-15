using System;
using System.Collections.Generic;
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
        private string _displayName;
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class with
        /// the specified name.
        /// </summary>
        /// <param name="name">The name of the channel.</param>
        public Channel(string name)
        {
            _name = IrcUtil.UnescapeChannelName(name);
        }

        /// <summary>
        /// Gets the display name of the channel.
        /// </summary>
        [JsonProperty("display_name")]
        public string Name
        {
            get
            {
                if (_displayName == null)
                {
                    var user = new User(_name);
                    _displayName = user.Name;
                }
                return _displayName;
            }
            protected set { _displayName = value; }
        }

        /// <summary>
        /// Gets the Twitch API endpoint for this channel.
        /// </summary>
        protected override string Endpoint => $"channels/{_name}";

        /// <summary>
        /// Loads the stream data if the current channel is live; otherwise,
        /// returns <c>null</c>.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Calling the method two times in succession creates different results.")]
        public async Task<Stream> GetStreamAsync()
        {
            var results = new StreamResults(this);
            await results.Load();

            return results.Stream;
        }

        /// <summary>
        /// Returns the name of the channel formatted to work in IRC.
        /// </summary>
        /// <returns>The name of the channel, prefixed with a '#'.</returns>
        public string ToIrcChannel()
        {
            return IrcUtil.EscapeChannelName(_name);
        }

        /// <summary>
        /// Returns a string that represents the current channel.
        /// </summary>
        /// <returns>A string representing the current channel.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
