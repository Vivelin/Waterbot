using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using IrcDotNet;

namespace Kappa
{
    /// <summary>
    /// Represents a client that communicates with Twitch chat.
    /// </summary>
    public class TwitchClient : IDisposable
    {
        private readonly EndPoint twitchEP = new DnsEndPoint("irc.twitch.tv", 6667);
        private TaskCompletionSource<bool> connectTask;
        private TaskCompletionSource<bool> disconnectTask;
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitchClient"/> class,
        /// using the specified user name and OAuth key.
        /// </summary>
        public TwitchClient()
        {
            IrcClient = new StandardIrcClient()
            {
                FloodPreventer = new IrcStandardFloodPreventer(10, 1000)
            };

            IrcClient.Connected += IrcClient_Connected;
            IrcClient.ConnectFailed += IrcClient_ConnectFailed;
            IrcClient.Disconnected += IrcClient_Disconnected;
            IrcClient.RawMessageReceived += IrcClient_RawMessageReceived;
        }

        /// <summary>
        /// Occurs when the Twitch client was disconnected without calling the
        /// <see cref="DisconnectAsync"/> method.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Occurs when a chat message has been received.
        /// </summary>
        public event EventHandler<ChatMessageEventArgs> MessageReceived;

        /// <summary>
        /// Gets the IRC client used to communicate with the Twitch chat IRC
        /// server.
        /// </summary>
        [CLSCompliant(false)]
        protected StandardIrcClient IrcClient { get; }

        /// <summary>
        /// Connects to the Twitch chat server.
        /// </summary>
        /// <param name="userName">The user name to connect as.</param>
        /// <param name="key">The OAuth key for the specified user.</param>
        /// <param name="channels">
        /// An array of strings containing the names of the channels to
        /// initially connect to.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task ConnectAsync(string userName, string key, params string[] channels)
        {
            connectTask = new TaskCompletionSource<bool>();

            var credentials = new IrcUserRegistrationInfo()
            {
                NickName = userName,
                UserName = userName,
                Password = key
            };
            IrcClient.Connect(twitchEP, false, credentials);
            await connectTask.Task;

            Trace.WriteLine(string.Format("Connected to {0}", twitchEP), "Info");
            IrcClient.SendRawMessage("CAP REQ :twitch.tv/membership");
            IrcClient.SendRawMessage("CAP REQ :twitch.tv/tags");
            IrcClient.SendRawMessage("CAP REQ :twitch.tv/commands");

            foreach (var channel in channels)
            {
                IrcClient.SendRawMessage("JOIN #" + channel);

                // JOINs are rate-limited at 50 per 15 seconds => 3/s
                await Task.Delay(333);
            }
        }

        /// <summary>
        /// Disconnects from the Twitch chat server.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task DisconnectAsync()
        {
            disconnectTask = new TaskCompletionSource<bool>();

            IrcClient.Quit("Cave Johnson, we're done here.");
            await disconnectTask.Task;

            disconnectTask = null;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="TwitchClient"/>
        /// object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Sends a chat message to the specified channel.
        /// </summary>
        /// <param name="channel">
        /// The name of the channel to send the message to. The leading '#' is
        /// optional.
        /// </param>
        /// <param name="message">
        /// The contents of the chat message to send.
        /// </param>
        public virtual void SendMessage(string channel, string message)
        {
            var raw = TwitchUtil.FormatMessage(Commands.PRIVMSG,
                TwitchUtil.EscapeChannelName(channel), message);

            IrcClient.SendRawMessage(raw);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="TwitchClient"/>
        /// object.
        /// </summary>
        /// <param name="disposing">
        /// Indicates whether to release managed resources.
        /// </param>
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Justification = "Auto-implemented property is disposed")]
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    IrcClient?.Dispose();
                }

                isDisposed = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="Disconnected"/> event.
        /// </summary>
        protected virtual void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="MessageReceived"/> event.
        /// </summary>
        /// <param name="message">The message that was received.</param>
        protected virtual void OnMessageReceived(ChatMessage message)
        {
            var args = new ChatMessageEventArgs(message);
            MessageReceived?.Invoke(this, args);
        }

        private void IrcClient_Connected(object sender, EventArgs e)
        {
            Debug.Assert(connectTask != null,
                nameof(connectTask) + " cannot be null",
                nameof(connectTask) + " should always be created before calling Connect");

            connectTask.SetResult(true);
        }

        private void IrcClient_ConnectFailed(object sender, IrcErrorEventArgs e)
        {
            Debug.Assert(connectTask != null,
                nameof(connectTask) + " cannot be null",
                nameof(connectTask) + " should always be created before calling Connect");

            connectTask.SetException(e.Error);
        }

        private void IrcClient_Disconnected(object sender, EventArgs e)
        {
            if (disconnectTask != null)
            {
                // Use TrySetResult here as opposed to SetResult, as this could
                // cause issues during debugging when we set it to null.
                disconnectTask.TrySetResult(true);
            }
            else
            {
                OnDisconnected();
            }
        }

        private void IrcClient_RawMessageReceived(object sender, IrcRawMessageEventArgs e)
        {
            var message = Message.Parse(e.RawContent);

            if (message is ChatMessage)
            {
                OnMessageReceived((ChatMessage)message);
            }
            else
            {
                Trace.WriteLine(message.RawMessage, "Unhandled message received");
            }
        }
    }
}
