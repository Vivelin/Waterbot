using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using horsedrowner.Common;

namespace Kappa
{
    /// <summary>
    /// Represents a user on Twitch.
    /// </summary>
    [Serializable]
    public class User : TwitchApiObject
    {
        private string _displayName;
        private string _userName;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class for the
        /// specified user name.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        public User(string name)
        {
            _userName = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class for the
        /// specified channel.
        /// </summary>
        /// <param name="channel">The user's channel.</param>
        public User(Channel channel)
        {
            _userName = channel.Name;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <remarks>
        /// This property will return a cached result from previous API calls
        /// from other instances, if available.
        /// </remarks>
        [JsonProperty("display_name")]
        public string Name
        {
            get
            {
                if (_displayName == null)
                {
                    // Check if we have the name with proper capitalization
                    // available already
                    var index = DisplayNames.IndexOf(_userName, true);
                    if (index >= 0)
                        _displayName = DisplayNames[index];
                    else
                        _displayName = _userName;
                }
                return _displayName;
            }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;

                    // Store the proper capitalization in a static list so we
                    // don't have to do an API call for just the name later
                    if (DisplayNames.IndexOf(value, true) < 0)
                        DisplayNames.Add(value);
                }
            }
        }

        /// <summary>
        /// Gets a collection of usernames using the proper capitalization.
        /// </summary>
        protected static IList<string> DisplayNames { get; } = new List<string>();

        /// <summary>
        /// Gets the Twitch API endpoint for this user.
        /// </summary>
        protected override string Endpoint => $"users/{_userName}";

        /// <summary>
        /// Converts the specified <see cref="User"/> object to a string.
        /// </summary>
        /// <param name="user">The <see cref="User"/> object to convert.</param>
        public static explicit operator string(User user)
        {
            return user.ToString();
        }

        /// <summary>
        /// Returns a string that represents the current user.
        /// </summary>
        /// <returns>A string representing the current user.</returns>
        public override string ToString()
        {
            return Name ?? _userName;
        }
    }
}
