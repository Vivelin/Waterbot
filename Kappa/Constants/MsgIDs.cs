using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kappa
{
    /// <summary>
    /// Provides constants that specify notice message IDs.
    /// </summary>
    internal static class MsgIDs
    {
        /// <summary>
        /// Indicates a timeout command failed because it targeted a moderator.
        /// </summary>
        public const string BadTimeoutMod = "bad_timeout_mod";
    }
}
