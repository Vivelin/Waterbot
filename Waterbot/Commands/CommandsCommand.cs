using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa;

namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a command that shows the available commands.
    /// </summary>
    public class CommandsCommand : ICommand
    {
        private IEnumerable<string> _commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsCommand"/>
        /// class.
        /// </summary>
        /// <param name="commands">The collection of available commands.</param>
        public CommandsCommand(IEnumerable<string> commands)
        {
            _commands = commands;
        }

        /// <summary>
        /// Executes the command and returns a <see cref="ChatMessage"/> object
        /// that represents the bot's response.
        /// </summary>
        /// <param name="message">The message that issued the command.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public Task<ChatMessage> GetResponse(ChatMessage message)
        {
            var response = new StringBuilder();
            foreach (var item in _commands)
                response.AppendFormat("{0}; ", item);

            return Task.FromResult(message.CreateResponse(response.ToString()));
        }
    }
}
