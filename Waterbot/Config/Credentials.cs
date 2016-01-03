using System;

namespace Waterbot.Config
{
    /// <summary>
    /// Represents the credentials used in accessing Twitch chat and the Twitch
    /// API.
    /// </summary>
    [Serializable]
    public class Credentials
    {
        /// <summary>
        /// Gets or sets the Twitch Developer Application Client ID which is
        /// used in accessing the Twitch API.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Twitch Developer Application Client Secret which is
        /// used in some authentication flows and is considered confidential.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the OAuth token used in place of a password when
        /// connecting to Twitch chat.
        /// </summary>
        /// <remarks>
        /// An OAuth key may be generated at https://twitchapps.com/tmi/.
        /// </remarks>
        public string OAuthToken { get; set; }

        /// <summary>
        /// Gets or sets the bot's user name.
        /// </summary>
        public string UserName { get; set; }
    }
}
