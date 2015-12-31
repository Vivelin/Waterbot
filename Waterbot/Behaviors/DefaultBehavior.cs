using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kappa;
using Waterbot.Common;

namespace Waterbot
{
    /// <summary>
    /// Provides methods that determine the bot's default behavior.
    /// </summary>
    public class DefaultBehavior : Behavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBehavior"/>
        /// class using the specified user name.
        /// </summary>
        /// <param name="userName">The user name of the bot.</param>
        public DefaultBehavior(string userName) : base(userName)
        {
            RNG = new Random();

            Greetings = new List<string>()
            {
                "Hey", "Hi", "Yo", "Hej", "'sup", "Hello", "Hallo", "Hoi", "Hiya", "What's up", "Whatsup", "HeyGuys"
            };

            Farewells = new List<string>()
            {
                "Bye", "See ya", "Cya"
            };

            DefaultResponses = new List<string>()
            {
                "I don't get it.", "What?", "What is it?", "What do you want?", "What do you want?", "Are you talking to me?", "Did you say something?"
            };
        }

        /// <summary>
        /// Gets a list of possible responses when the bot is mentioned but no
        /// other response is available.
        /// </summary>
        public IList<string> DefaultResponses { get; }

        /// <summary>
        /// Gets a list of possible responses when the bot is leaving a channel.
        /// </summary>
        public IList<string> Farewells { get; }

        /// <summary>
        /// Gets a list of greetings to respond to and with.
        /// </summary>
        public IList<string> Greetings { get; }

        /// <summary>
        /// Gets a random number generator for this instance.
        /// </summary>
        protected Random RNG { get; }

        /// <summary>
        /// Determines the bot's message when joining a channel.
        /// </summary>
        /// <param name="channel">The channel that was joined.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public override ChatMessage GetJoinMessage(string channel)
        {
            var greeting = Greetings.Sample(RNG);
            var text = $"{greeting} chat!";
            return new ChatMessage(channel, text);
        }

        /// <summary>
        /// Determines the bot's message when leaving a channel.
        /// </summary>
        /// <param name="channel">The channel that was left.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public override ChatMessage GetPartMessage(string channel)
        {
            var farewell = Farewells.Sample(RNG);
            return new ChatMessage(channel, farewell);
        }

        /// <summary>
        /// Determines the bot's default response to the specified message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with, or <c>null</c>.
        /// </returns>
        public override ChatMessage GetResponse(ChatMessage message)
        {
            if (message.Mentions(UserName))
            {
                if (message.MentionsAny(Greetings))
                    return Greet(message);

                return message.CreateResponse(DefaultResponses.Sample(RNG), true);
            }

            return null;
        }

        /// <summary>
        /// Returns a message that greets the user in response to a message.
        /// </summary>
        /// <param name="message">The message to respond to.</param>
        /// <returns>
        /// A <see cref="ChatMessage"/> object that represents the message to
        /// respond with.
        /// </returns>
        public virtual ChatMessage Greet(ChatMessage message)
        {
            var greeting = Greetings.Sample(RNG);
            var text = $"{greeting} {message.DisplayName}!";
            return message.CreateResponse(text);
        }
    }
}
