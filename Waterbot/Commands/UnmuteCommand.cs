using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa;
using Waterbot.Common;

namespace Waterbot.Commands
{
    /// <summary>
    /// Represents a mod-only command that unmutes the bot.
    /// </summary>
    public class UnmuteCommand : ICommand
    {
        private Behavior _sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnmuteCommand"/> class
        /// for the specified behavior.
        /// </summary>
        /// <param name="sender">
        /// The <see cref="Behavior"/> instance to mute.
        /// </param>
        public UnmuteCommand(Behavior sender)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));

            _sender = sender;
        }

        /// <summary>
        /// Gets or sets the responses to use when a mod tries to use the
        /// command.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
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
            if (message.IsBroadcaster || message.IsMod)
            {
                _sender.Mute = false;
                Log.Add(Events.MuteDisabled.With(message));

                var response = SuccessResponses.Sample();
                return Task.FromResult(message.CreateResponse(response));
            }

            return Task.FromResult<ChatMessage>(null);
        }
    }
}
