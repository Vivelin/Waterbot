namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a class that provides <see cref="UnmuteCommand"/> objects.
    /// </summary>
    public class UnmuteCommandManufactorum : CommandManufactorum
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="UnmuteCommandManufactorum"/>.
        /// </summary>
        public UnmuteCommandManufactorum()
        {
            PublicCommands = new string[] { "unmute" };
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
            return new UnmuteCommand(Sender as Behavior)
            {
                SuccessResponses = Configuration.Behavior.UnmuteMessages
            };
        }
    }
}
