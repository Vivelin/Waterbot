using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using horsedrowner.Common;

namespace Waterbot
{
    /// <summary>
    /// Provides the types of events that can be logged when errors occur.
    /// </summary>
    internal static class Errors
    {
        /// <summary>
        /// An error occurred while trying to reconnect.
        /// </summary>
        public static readonly ExceptionLogEvent Reconnect 
            = new ExceptionLogEvent(EventType.Warning, 4001);
    }
}
