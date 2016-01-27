using System;
using System.Collections.Generic;

namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a class that is capable of creating <see
    /// cref="SimpleCommand"/> instances.
    /// </summary>
    public class SimpleCommandManufactorum : CommandManufactorum
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="SimpleCommandManufactorum"/> class.
        /// </summary>
        public SimpleCommandManufactorum() { }

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
            ThrowIfNoConfiguration();

            var responseSet = Configuration.Behavior.SimpleCommands[command];
            return new SimpleCommand(responseSet);
        }

        /// <summary>
        /// Returns a collection of the names of publicly available commands
        /// this factory can create.
        /// </summary>
        public override IEnumerable<string> GetNames()
        {
            ThrowIfNoConfiguration();

            return Configuration.Behavior.SimpleCommands?.Keys;
        }

        private void ThrowIfNoConfiguration()
        {
            if (Configuration == null)
            {
                throw new InvalidOperationException(Strings.ConfigRequired);
            }
        }
    }
}
