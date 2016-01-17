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
    public class CommandsCommandManufactorum : ICommandManufactorum
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="CommandsCommandManufactorum"/> class.
        /// </summary>
        public CommandsCommandManufactorum()
        {
        }

        /// <summary>
        /// Gets or sets the configuration to use.
        /// </summary>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// Gets a collection of the publicly available commands this factory
        /// can create.
        /// </summary>
        public IEnumerable<string> PublicCommands
        {
            get { yield return "commands"; }
        }

        /// <summary>
        /// Indicates whether the manufactorum is capable of creating the
        /// specified command.
        /// </summary>
        /// <param name="command">The name of the command.</param>
        /// <returns>
        /// <c>true</c> if this class can create the specified command;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool CanCreate(string command)
        {
            return string.Compare(command, "commands", true) == 0;
        }

        /// <summary>
        /// Creates a new instance of the specified command.
        /// </summary>
        /// <param name="command">The name of the command to create.</param>
        /// <returns>
        /// A new <see cref="ICommand"/> object, or <c>null</c> if <paramref
        /// name="command"/> does not represent a valid command.
        /// </returns>
        public ICommand Create(string command)
        {
            var commands = new List<string>();
            var factories = Waterbot.EnumerateCommandFactories();
            foreach (var factory in factories)
            {
                factory.Configuration = Configuration;
                commands.AddRange(factory.PublicCommands);
            }

            return new CommandsCommand(commands);
        }
    }
}
