using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa;

namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a mod-only command that mutes the bot.
    /// </summary>
    public class BeQuietCommand : ICommand
    {
        private Behavior _sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeQuietCommand"/> class
        /// for the specified behavior.
        /// </summary>
        /// <param name="sender">
        /// The <see cref="Behavior"/> instance to mute.
        /// </param>
        public BeQuietCommand(Behavior sender)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));

            _sender = sender;
        }

        /// <summary>
        /// Gets or sets the responses to use when a regular user tries to use
        /// the command.
        /// </summary>
        public PhraseSet FailureResponses { get; set; }

        /// <summary>
        /// Gets or sets the responses to use when a mod tries to use the
        /// command.
        /// </summary>
        public PhraseSet SuccessResponses { get; set; }

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
            string response;

            if (message.IsBroadcaster || message.IsMod)
            {
                _sender.Mute = true;
                Trace.WriteLine(
                    string.Format("{0} enabled Mute in channel {1}",
                        message.User, message.Channel),
                    "Info"); // TODO: Event log?

                response = SuccessResponses.Sample();
            }
            else
            {
                response = FailureResponses.Sample();
            }

            return Task.FromResult(message.CreateResponse(response));
        }
    }
}
