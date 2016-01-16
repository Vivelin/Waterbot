using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa;
using Waterbot.Common;

namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a command that checks the uptime of a stream.
    /// </summary>
    public class UptimeCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeCommand"/> class.
        /// </summary>
        /// <param name="responseSet">
        /// The set of responses to use when offline.
        /// </param>
        public UptimeCommand(PhraseSet responseSet)
        {
            FailureResponses = responseSet;
        }

        /// <summary>
        /// Gets the set of possible responses to use when the command is issued
        /// on a channel that is offline.
        /// </summary>
        public PhraseSet FailureResponses { get; }

        /// <summary>
        /// Executes the command and returns a <see cref="ChatMessage"/> object
        /// that represents the bot's response.
        /// </summary>
        /// <param name="message">The message that issued the command.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task<ChatMessage> GetResponse(ChatMessage message)
        {
            var stream = await message.Channel.GetStreamAsync();
            if (stream == null)
            {
                var response = FailureResponses.Sample();
                return message.CreateResponse(response);
            }
            else
            {
                var startTime = stream.Started;
                var elapsedTime = startTime.ToRelativeTimeString();

                var response = string.Format("{0} started streaming {1}.",
                    stream.Channel, elapsedTime);
                return message.CreateResponse(response);
            }
        }
    }
}
