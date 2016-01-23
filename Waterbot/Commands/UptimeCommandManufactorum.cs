using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a class that provides <see cref="UptimeCommand"/> objects.
    /// </summary>
    public class UptimeCommandManufactorum : CommandManufactorum
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="UptimeCommandManufactorum"/> class.
        /// </summary>
        public UptimeCommandManufactorum()
        {
            PublicCommands = new string[] { "uptime" };
        }

        /// <summary>
        /// Creates a new instance of the specified command.
        /// </summary>
        /// <param name="command">The name of the command to create.</param>
        /// <returns>
        /// A new <see cref="ICommand"/> object, or <c>null</c> if <paramref
        /// name="command"/> does not represent a valid command.
        /// </returns>
        public override ICommand Create(string command)
        {
            var responseSet = Configuration.Behavior.UptimeOfflineResponses;
            return new UptimeCommand(responseSet);
        }
    }
}
