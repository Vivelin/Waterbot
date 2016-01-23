using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a class that is capable of creating <see
    /// cref="CommandsCommand"/> instances.
    /// </summary>
    public class CommandsCommandManufactorum : CommandManufactorum
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="CommandsCommandManufactorum"/> class.
        /// </summary>
        public CommandsCommandManufactorum()
        {
            PublicCommands = new string[] { "commands" };
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
            var commands = new List<string>();
            var factories = Waterbot.EnumerateCommandFactories();
            foreach (var factory in factories)
            {
                factory.Configuration = Configuration;
                commands.AddRange(factory.GetNames());
            }

            return new CommandsCommand(commands);
        }
    }
}
