using System;

namespace Kappa
{
    /// <summary>
    /// Provides constants that specify supported IRC commands.
    /// </summary>
    internal static class Commands
    {
        /// <summary>
        /// Indicates the message that is sent when a user has joined a channel.
        /// </summary>
        public const string JOIN = "JOIN";

        /// <summary>
        /// Indicates a notice.
        /// </summary>
        public const string NOTICE = "NOTICE";

        /// <summary>
        /// Indicates the message that is sent when a user has left a channel.
        /// </summary>
        public const string PART = "PART";

        /// <summary>
        /// Indicates a chat message.
        /// </summary>
        public const string PRIVMSG = "PRIVMSG";

        /// <summary>
        /// Indicates a whisper message.
        /// </summary>
        public const string WHISPER = "WHISPER";
    }
}
