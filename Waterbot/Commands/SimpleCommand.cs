using System.Threading.Tasks;
using Kappa;

namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a command that uses a pre-defined set of responses.
    /// </summary>
    public class SimpleCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCommand"/> class.
        /// </summary>
        /// <param name="responseSet">The set of responses to use.</param>
        public SimpleCommand(PhraseSet responseSet)
        {
            Responses = responseSet;
        }

        /// <summary>
        /// Gets the set of possible responses.
        /// </summary>
        public PhraseSet Responses { get; }

        /// <summary>
        /// Executes the command and returns a <see cref="ChatMessage"/> object
        /// containing a randomly selected pre-defined response.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public Task<ChatMessage> GetResponse(ChatMessage message)
        {
            var text = Responses.Sample();
            return Task.FromResult(message.CreateResponse(text));
        }
    }
}
