using System;
using System.Collections.Generic;
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
    public class TwitchChat : IDisposable
    {
        private readonly EndPoint _twitchEP = new DnsEndPoint("irc.twitch.tv", 6667);
        private TaskCompletionSource<bool> _connectTask;
        private TaskCompletionSource<bool> _disconnectTask;
        private bool _isDisposed = false;
        private TaskCompletionSource<bool> _sendMessageTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitchChat"/> class,
        /// using the specified user name and OAuth key.
        /// </summary>
        public TwitchChat()
        {
            IrcClient = new StandardIrcClient()
            {
                FloodPreventer = new IrcStandardFloodPreventer(10, 1000)
            };

            IrcClient.Connected += IrcClient_Connected;
            IrcClient.ConnectFailed += IrcClient_ConnectFailed;
            IrcClient.Disconnected += IrcClient_Disconnected;
            IrcClient.RawMessageReceived += IrcClient_RawMessageReceived;
            IrcClient.RawMessageSent += IrcClient_RawMessageSent;
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
        /// Occurs when a notice has been received.
        /// </summary>
        public event EventHandler<MessageEventArgs> NoticeReceived;

        /// <summary>
        /// Occurs when someone has joined chat.
        /// </summary>
        public event EventHandler<MessageEventArgs> ViewerJoined;

        /// <summary>
        /// Occurs when someone has left chat.
        /// </summary>
        public event EventHandler<MessageEventArgs> ViewerLeft;

        /// <summary>
        /// Gets the name of the user that is connected to Twitch.
        /// </summary>
        public string UserName { get; set; }

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
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task ConnectAsync(string userName, string key)
        {
            _connectTask = new TaskCompletionSource<bool>();

            var credentials = new IrcUserRegistrationInfo()
            {
                NickName = userName,
                UserName = userName,
                Password = key
            };
            IrcClient.Connect(_twitchEP, false, credentials);
            await _connectTask.Task;

            Trace.WriteLine(string.Format("Connected to {0}", _twitchEP), "Info");
            UserName = userName;
            IrcClient.SendRawMessage("CAP REQ :twitch.tv/membership");
            IrcClient.SendRawMessage("CAP REQ :twitch.tv/tags");
            IrcClient.SendRawMessage("CAP REQ :twitch.tv/commands");
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
            _disconnectTask = new TaskCompletionSource<bool>();

            IrcClient.Quit("Cave Johnson, we're done here.");
            await _disconnectTask.Task;

            _disconnectTask = null;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="TwitchChat"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Joins the specified channel asynchronously.
        /// </summary>
        /// <param name="channel">The name of the channel to join.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task JoinAsync(string channel)
        {
            _sendMessageTask = new TaskCompletionSource<bool>();

            var message = new JoinMessage(channel);
            IrcClient.SendRawMessage(message.ConstructCommand());
            await _sendMessageTask.Task;

            _sendMessageTask = null;
            var user = new User(channel);
            await user.Load();
        }

        /// <summary>
        /// Joins the specified channels asynchronously.
        /// </summary>
        /// <param name="channels">
        /// A collection of strings containing the names of the channels to
        /// connect to.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task JoinAsync(IEnumerable<string> channels)
        {
            foreach (var channel in channels)
            {
                await JoinAsync(channel);

                // JOINs are rate-limited at 50 per 15 seconds => 3/s
                await Task.Delay(333);
            }
        }

        /// <summary>
        /// Sends a chat message to the specified channel.
        /// </summary>
        /// <param name="message">The chat message to send.</param>
        public virtual async Task SendMessage(ChatMessage message)
        {
            if (message != null)
            {
                var raw = message.ConstructCommand(contents =>
                {
                    return string.Format(contents,
                        message.Channel,
                        UserName,
                        message.Target?.Name ?? message.Channel.Name);
                });
                _sendMessageTask = new TaskCompletionSource<bool>();

                IrcClient.SendRawMessage(raw);
                await _sendMessageTask.Task;

                _sendMessageTask = null;
                message.User = new User(UserName);
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="TwitchChat"/> object.
        /// </summary>
        /// <param name="disposing">
        /// Indicates whether to release managed resources.
        /// </param>
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Justification = "Auto-implemented property is disposed")]
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    IrcClient?.Dispose();
                }

                _isDisposed = true;
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

        /// <summary>
        /// Raises the <see cref="NoticeReceived"/> event.
        /// </summary>
        /// <param name="message">The message that was received.</param>
        protected virtual void OnNoticeReceived(Message message)
        {
            var args = new MessageEventArgs(message);
            NoticeReceived?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="ViewerJoined"/> event.
        /// </summary>
        /// <param name="message">The message that was received.</param>
        protected virtual void OnViewerJoined(Message message)
        {
            var args = new MessageEventArgs(message);
            ViewerJoined?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="ViewerLeft"/> event.
        /// </summary>
        /// <param name="message">The message that was received.</param>
        protected virtual void OnViewerLeft(Message message)
        {
            var args = new MessageEventArgs(message);
            ViewerLeft?.Invoke(this, args);
        }

        private void IrcClient_Connected(object sender, EventArgs e)
        {
            Debug.Assert(_connectTask != null,
                nameof(_connectTask) + " cannot be null",
                nameof(_connectTask) + " should always be created before calling Connect");

            _connectTask.SetResult(true);
        }

        private void IrcClient_ConnectFailed(object sender, IrcErrorEventArgs e)
        {
            Debug.Assert(_connectTask != null,
                nameof(_connectTask) + " cannot be null",
                nameof(_connectTask) + " should always be created before calling Connect");

            _connectTask.SetException(e.Error);
        }

        private void IrcClient_Disconnected(object sender, EventArgs e)
        {
            if (_disconnectTask != null)
            {
                // Use TrySetResult here as opposed to SetResult, as this could
                // cause issues during debugging when we set it to null.
                _disconnectTask.TrySetResult(true);
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
                OnMessageReceived((ChatMessage)message);
            else if (message is JoinMessage)
                OnViewerJoined(message);
            else if (message is PartMessage)
                OnViewerLeft(message);
            else if (message is NoticeMessage)
                OnNoticeReceived(message);
            else
                Trace.WriteLine(message.RawMessage, "Unhandled message received");
        }

        private void IrcClient_RawMessageSent(object sender, IrcRawMessageEventArgs e)
        {
            if (_sendMessageTask != null)
                _sendMessageTask.TrySetResult(true);
        }
    }
}
