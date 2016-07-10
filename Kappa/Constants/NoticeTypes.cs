using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa
{
    /// <summary>
    /// Provides constants that specify the types of notices (msg-id).
    /// </summary>
    internal static class NoticeTypes
    {
        /// <summary>
        /// Indicates a timeout command failed because it targeted the
        /// broadcaster.
        /// </summary>
        public const string BadTimeoutBroadcaster = "bad_timeout_broadcaster";

        /// <summary>
        /// Indicates a timeout command failed because it targeted a moderator.
        /// </summary>
        public const string BadTimeoutMod = "bad_timeout_mod";

        /// <summary>
        /// Indicates a timeout command failed because it targeted the sender.
        /// </summary>
        public const string BadTimeoutSelf = "bad_timeout_self";

        /// <summary>
        /// Indicates the channel is hosting another channel.
        /// </summary>
        public const string HostOn = "host_on";

        /// <summary>
        /// Indicates the channel exited host mode.
        /// </summary>
        public const string HostOff = "host_off";

        /// <summary>
        /// Indicates a timeout command was successful.
        /// </summary>
        public const string TimeoutSuccess = "timeout_success";
    }
}
