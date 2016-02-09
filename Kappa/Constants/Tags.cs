using System;

namespace Kappa
{
    /// <summary>
    /// Provides constants that specify supported Twitch message tags.
    /// </summary>
    public static class MessageTags
    {
        /// <summary>
        /// Indicates the display name of a user ("Customize capitalization for
        /// your username").
        /// </summary>
        public const string DisplayName = "display-name";

        /// <summary>
        /// Indicates the notice message ID.
        /// </summary>
        public const string MsgID = "msg-id";

        /// <summary>
        /// Indicates whether the user is subbed.
        /// </summary>
        public const string Subscriber = "subscriber";

        /// <summary>
        /// Indicates the type of user (mod).
        /// </summary>
        public const string UserType = "user-type";

        /// <summary>
        /// Indicates the user is a mod.
        /// </summary>
        public const string Mod = "mod";
    }
}
