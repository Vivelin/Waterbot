using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waterbot.Common;

namespace Kappa
{
    /// <summary>
    /// Represents notices sent by the Twitch chat server.
    /// </summary>
    public class NoticeMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoticeMessage"/> class
        /// with the contents of the parsed message.
        /// </summary>
        /// <param name="results">The parsed message.</param>
        protected internal NoticeMessage(ParseResults results) : base(results)
        {
            Channel = new Channel(Parameters[0]);
            Text = Parameters[1];

            MsgID = Tags?.Get(MessageTags.MsgID);
        }

        /// <summary>
        /// Gets the Twitch channel that sent the notice.
        /// </summary>
        public Channel Channel { get; }

        /// <summary>
        /// Gets the notice message ID.
        /// </summary>
        /// <remarks>
        /// For a list of known message IDs, see <see cref="MsgIDs"/>.
        /// </remarks>
        public string MsgID { get; }

        /// <summary>
        /// Gets the text of the notice message.
        /// </summary>
        public string Text { get; }
    }
}
