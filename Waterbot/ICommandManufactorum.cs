using System.Collections.Generic;

namespace Waterbot
{
    /// <summary>
    /// Defines a class that can create <see cref="ICommand"/> instances.
    /// </summary>
    public interface ICommandManufactorum
    {
        /// <summary>
        /// Gets or sets the configuration to use.
        /// </summary>
        Configuration Configuration { get; set; }

        /// <summary>
        /// Gets a collection of the publicly available commands this factory
        /// can create.
        /// </summary>
        IEnumerable<string> PublicCommands { get; }

        /// <summary>
        /// Indicates whether the command manufactorum is capable of creating
        /// commands for the specified command.
        /// </summary>
        /// <param name="command">The name of the command.</param>
        /// <returns>
        /// <c>true</c> if manufactorum can create the specified command;
        /// otherwise, <c>false</c>.
        /// </returns>
        bool CanCreate(string command);

        /// <summary>
        /// Creates a new instance of the specified command.
        /// </summary>
        /// <param name="command">The name of the command to create.</param>
        /// <returns>
        /// A new <see cref="ICommand"/> object, or <c>null</c> if <paramref
        /// name="command"/> does not represent a valid command.
        /// </returns>
        ICommand Create(string command);
    }
}
