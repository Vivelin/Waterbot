using System;
using Newtonsoft.Json;

namespace Kappa
{
    /// <summary>
    /// Represents a stream on Twitch.
    /// </summary>
    [Serializable]
    public class Stream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Stream"/> class.
        /// </summary>
        protected Stream() { }

        /// <summary>
        /// Gets the channel of the stream.
        /// </summary>
        [JsonProperty("channel")] // Fuck Json.NET
        public Channel Channel { get; protected set; }

        /// <summary>
        /// Gets the name of the game being streamed.
        /// </summary>
        [JsonProperty("game")]
        public string Game { get; protected set; }

        /// <summary>
        /// Gets a value that indicates when the object has last been loaded
        /// using live data; otherwise, returns <c>null</c>.
        /// </summary>
        public DateTime? Loaded { get; internal set; }

        /// <summary>
        /// Gets the start time of the stream.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime Started { get; protected set; }

        /// <summary>
        /// Gets the average framerate of the stream.
        /// </summary>
        [JsonProperty("average_fps")]
        public double VideoFramerate { get; protected set; }

        /// <summary>
        /// Gets the height of the stream, in pixels.
        /// </summary>
        [JsonProperty("video_height")]
        public int VideoHeight { get; protected set; }

        /// <summary>
        /// Gets the amount of viewers currently watching the stream.
        /// </summary>
        [JsonProperty("viewers")]
        public int Viewers { get; protected set; }

        /// <summary>
        /// Returns a string that represents the current stream.
        /// </summary>
        /// <returns>A string representing the current stream.</returns>
        public override string ToString()
        {
            if (Game != null)
                return string.Format(Strings.Stream_String, Channel.Name, Game);
            return Channel.Name ?? base.ToString();
        }
    }
}
