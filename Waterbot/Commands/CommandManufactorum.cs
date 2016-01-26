using System.Collections.Generic;
using System.Linq;

namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a class that provides <see cref="ICommand"/> objects. This is
    /// an abstract class.
    /// </summary>
    public abstract class CommandManufactorum : ICommandManufactorum
    {
        /// <summary>
        /// Gets or sets the configuration to use.
        /// </summary>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the source of the command.
        /// </summary>
        public object Sender { get; set; }

        /// <summary>
        /// Gets or sets a collection of the names of publicly available
        /// commands this factory can create.
        /// </summary>
        /// <remarks>
        /// The default implemention of <see cref="GetNames"/> will return this
        /// list. If the list of publicly available commands is not static,
        /// override <see cref="GetNames"/> instead.
        /// </remarks>
        protected IEnumerable<string> PublicCommands { get; set; }

        /// <summary>
        /// Indicates whether the manufactorum is capable of creating the
        /// specified command.
        /// </summary>
        /// <param name="command">The name of the command.</param>
        /// <returns>
        /// <c>true</c> if this class can create the specified command;
        /// otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanCreate(string command)
        {
            var commands = GetNames();
            return commands.Contains(command);
        }

        /// <summary>
        /// When overriden in a derived class, creates a new instance of the
        /// specified command.
        /// </summary>
        /// <param name="command">The name of the command to create.</param>
        /// <returns>
        /// A new <see cref="ICommand"/> object, or <c>null</c> if <paramref
        /// name="command"/> does not represent a valid command.
        /// </returns>
        public abstract ICommand Create(string command);

        /// <summary>
        /// Returns a collection of the names of publicly available commands
        /// this factory can create.
        /// </summary>
        /// <remarks>
        /// Unless overridden in a derived class, this method returns the value
        /// of the <see cref="PublicCommands"/> property. This method should be
        /// overridden when custom behavior is required, rather than overriding
        /// the getter of <see cref="PublicCommands"/>.
        /// </remarks>
        public virtual IEnumerable<string> GetNames()
        {
            return PublicCommands;
        }
    }
}
