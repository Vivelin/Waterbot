using System;
using System.Collections.Generic;

namespace Waterbot.Config
{
    /// <summary>
    /// Represents a set of preferences for the bot's behavior.
    /// </summary>
    [Serializable]
    public class BehaviorPreferences
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorPreferences"/>
        /// class.
        /// </summary>
        public BehaviorPreferences()
        {
            Properties = new Dictionary<string, IList<string>>();
            DefaultResponses = new List<string>()
            {
                "I don't get it.", "What?", "What is it?", "What do you want?",
                "What do you want?", "Are you talking to me?",
                "Did you say something?"
            };
            Farewells = new List<string>
            {
                "I should go.", "I'll be going now.", "Bye!", "See ya!",
                "Cave Johnson, we're done here."
            };
            Greetings = new List<string>
            {
                "Hey", "Hi", "Yo", "Hej", "'sup", "Hello", "Hallo", "Hoi", "Hiya",
                "What's up", "Whatsup", "HeyGuys"
            };
            StaticCommands = new Dictionary<string, string>
            {
                { "help", "This is a bot account. For more information, see https://github.com/horsedrowner/Waterbot" }
            };
            CommandAliases = new Dictionary<string, string>
            {
                { "botinfo", "help" }
            };
        }

        /// <summary>
        /// Gets or sets a dictionary that maps command aliases to other
        /// commands.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "Json.NET sucks")]
        public IDictionary<string, string> CommandAliases { get; set; }

        /// <summary>
        /// Gets or sets a list of possible responses when the bot is mentioned
        /// but no other response is available.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "Json.NET sucks")]
        public IList<string> DefaultResponses { get; set; }

        /// <summary>
        /// Gets or sets a list of possible responses when the bot is leaving a
        /// channel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "Json.NET sucks")]
        public IList<string> Farewells { get; set; }

        /// <summary>
        /// Gets or sets a list of greetings to respond to and with.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "Json.NET sucks")]
        public IList<string> Greetings { get; set; }

        /// <summary>
        /// Gets or sets a dictionary containing custom properties.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "Json.NET sucks")]
        public IDictionary<string, IList<string>> Properties { get; set; }

        /// <summary>
        /// Gets or sets a dictionary that maps commands to static responses.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "Json.NET sucks")]
        public IDictionary<string, string> StaticCommands { get; set; }

        /// <summary>
        /// Gets the custom property with the specified key.
        /// </summary>
        /// <param name="key">The key of the property to get.</param>
        /// <returns>
        /// The property with the specified key, or <c>null</c> if the specified
        /// key does not exist.
        /// </returns>
        public IList<string> this[string key]
        {
            get
            {
                if (Properties.ContainsKey(key))
                    return Properties[key];
                return null;
            }
        }
    }
}
