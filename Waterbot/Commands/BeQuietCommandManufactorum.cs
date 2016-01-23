namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a class that provides <see cref="BeQuietCommand"/> objects.
    /// </summary>
    public class BeQuietCommandManufactorum : CommandManufactorum
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="BeQuietCommandManufactorum"/>.
        /// </summary>
        public BeQuietCommandManufactorum()
        {
            PublicCommands = new string[] { "be quiet" };
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
            return new BeQuietCommand(Sender as Behavior)
            {
                FailureResponses = Configuration.Behavior.AccessDeniedMessages,
                SuccessResponses = Configuration.Behavior.MuteMessages
            };
        }
    }
}
