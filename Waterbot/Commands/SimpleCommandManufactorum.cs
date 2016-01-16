namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a class that is capable of creating <see
    /// cref="SimpleCommand"/> instances.
    /// </summary>
    public class SimpleCommandManufactorum : ICommandManufactorum
    {
        private Configuration _config;

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="SimpleCommandManufactorum"/> class.
        /// </summary>
        /// <param name="config">The configuration to use.</param>
        public SimpleCommandManufactorum(Configuration config)
        {
            _config = config;
        }

        /// <summary>
        /// Indicates whether the <see cref="SimpleCommandManufactorum"/> is
        /// capable of creating the specified command.
        /// </summary>
        /// <param name="command">The name of the command.</param>
        /// <returns>
        /// <c>true</c> if this class can create the specified command;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool CanCreate(string command)
        {
            return _config.Behavior.SimpleCommands.ContainsKey(command);
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
            var responseSet = _config.Behavior.SimpleCommands[command];
            return new SimpleCommand(responseSet);
        }
    }
}
