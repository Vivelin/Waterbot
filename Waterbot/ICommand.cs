using System.Threading.Tasks;
using Kappa;

namespace Waterbot
{
    /// <summary>
    /// Defines a class that represents a chat command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command and returns a <see cref="ChatMessage"/> object
        /// that represents the bot's response.
        /// </summary>
        /// <param name="message">The message that issued the command.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        Task<ChatMessage> GetResponse(ChatMessage message);
    }
}
