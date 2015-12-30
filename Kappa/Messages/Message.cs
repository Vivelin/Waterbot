﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrcDotNet;

namespace Kappa
{
    /// <summary>
    /// Represents an IRC message sent by Twitch.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
            Parameters = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class with
        /// the contents of the parsed message.
        /// </summary>
        /// <param name="message">The raw IRC message.</param>
        /// <param name="tags">A dictionary containing the message tags.</param>
        /// <param name="prefix">The message prefix.</param>
        /// <param name="command">The message command.</param>
        /// <param name="parameters">The parameters of the command.</param>
        protected internal Message(string message,
            IReadOnlyDictionary<string, string> tags,
            string prefix,
            string command,
            IList<string> parameters)
        {
            RawMessage = message;
            Tags = tags;
            Prefix = prefix;
            Command = command;
            Parameters = parameters;

            if (prefix != null)
            {
                var i = prefix.IndexOf('!');
                if (i > 0)
                    UserName = prefix.Substring(0, i);
            }
        }

        /// <summary>
        /// Gets the IRC command name of the message.
        /// </summary>
        public string Command { get; protected set; }

        /// <summary>
        /// Gets a list that contains the command's parameters.
        /// </summary>
        public IList<string> Parameters { get; }

        /// <summary>
        /// Gets a value containing the message prefix, or <c>null</c> if no
        /// prefix was specified. This property does not apply to messages to be
        /// sent.
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        /// Gets the raw message as it was sent by the server. This property
        /// does not apply to messages to be sent.
        /// </summary>
        public string RawMessage { get; }

        /// <summary>
        /// Gets a dictionary containing the tags for the message, or
        /// <c>null</c> if the message did not have any tags. This property does
        /// not apply to messages to be sent.
        /// </summary>
        public IReadOnlyDictionary<string, string> Tags { get; }

        /// <summary>
        /// Gets the name of the user from which this message originates.
        /// </summary>
        public string UserName { get; protected internal set; }

        /// <summary>
        /// Parses the specified IRC message.
        /// </summary>
        /// <param name="message">The raw IRC message to parse.</param>
        /// <returns>A new <see cref="Message"/> object.</returns>
        public static Message Parse(string message)
        {
            Dictionary<string, string> tags = null;
            string prefix = null;
            string command = null;
            List<string> parameters = new List<string>();

            var length = message.Length;
            var index = 0;
            while (index < length)
            {
                var currentChar = message[index];
                if (currentChar == '@' && command == null)
                {
                    // Parse message tags, e.g.:
                    // @color=#DAA520;display-name=horsedrowner;emotes=;subscriber=0;turbo=0;user-id=11762707;user-type=mod
                    var segmentStart = index + 1;
                    var segmentEnd = message.IndexOf(' ', segmentStart);
                    var tagsString = message.Substring(segmentStart, segmentEnd - segmentStart);

                    tags = ParseTagsString(tagsString);

                    index = segmentEnd + 1;
                    continue;
                }
                else if (currentChar == ':' && command == null)
                {
                    // Parse message prefix, e.g.:
                    // :horsedrowner!horsedrowner@horsedrowner.tmi.twitch.tv
                    var segmentStart = index + 1;
                    var segmentEnd = message.IndexOf(' ', index);
                    prefix = message.Substring(segmentStart, segmentEnd - segmentStart);

                    index = segmentEnd + 1;
                    continue;
                }
                else if (currentChar == ':')
                {
                    // Parse the last parameter, which may contain spaces if
                    // prefixed with a colon, e.g. :This message contains
                    // spaces.
                    var param = message.Substring(index + 1);
                    parameters.Add(param);

                    index = length;
                    break;
                }
                else
                {
                    // The first segment that doesn't start with an @ or an : is
                    // the command. Segments that follow are parameters.
                    var segmentEnd = message.IndexOf(' ', index);
                    if (segmentEnd < 0)
                        segmentEnd = length;

                    var segment = message.Substring(index, segmentEnd - index);

                    if (command == null)
                        command = segment;
                    else
                        parameters.Add(segment);

                    index = segmentEnd + 1;
                    continue;
                }
            }

            return Create(message, tags, prefix, command, parameters);
        }

        /// <summary>
        /// Gets the raw IRC command for sending this message.
        /// </summary>
        /// <returns>A string containing the IRC message to send.</returns>
        public virtual string ConstructCommand()
        {
            return TwitchUtil.FormatMessage(Command, Parameters);
        }

        /// <summary>
        /// Returns a string representation of the message.
        /// </summary>
        /// <returns>A string representing the current instance.</returns>
        public override string ToString()
        {
            return RawMessage;
        }

        private static Message Create(string message,
            Dictionary<string, string> tags,
            string prefix,
            string command,
            List<string> parameters)
        {
            Trace.WriteLine(message, "IRC");

            switch (command)
            {
                case Commands.PRIVMSG:
                    return new ChatMessage(message, tags, prefix, command, parameters);

                default:
                    return new Message(message, tags, prefix, command, parameters);
            }
        }

        private static Dictionary<string, string> ParseTagsString(string tagsString)
        {
            const char VALUE_SEPARATOR = '=';
            const char TAG_SEPARATOR = ';';
            var tags = new Dictionary<string, string>();

            var length = tagsString.Length;
            var index = 0;
            while (index < length)
            {
                var keyStart = index;
                var keyEnd = tagsString.IndexOf(VALUE_SEPARATOR, keyStart);
                var valueStart = keyEnd + 1;
                var valueEnd = tagsString.IndexOf(TAG_SEPARATOR, valueStart);
                if (valueEnd < 0)
                    valueEnd = length;

                var key = tagsString.Substring(keyStart, keyEnd - keyStart);
                var value = tagsString.Substring(valueStart, valueEnd - valueStart);
                tags.Add(key, value);

                index = valueEnd + 1;
            }

            return tags;
        }
    }
}