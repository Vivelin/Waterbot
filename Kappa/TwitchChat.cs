using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using IrcDotNet;

namespace Kappa
{
    /// <summary>
    /// Represents a client that communicates with Twitch chat.
    /// </summary>
    public class TwitchChat : IDisposable
    {
        private static readonly TimeSpan s_timeout = TimeSpan.FromSeconds(30);
        private readonly EndPoint _twitchEP = new DnsEndPoint("irc.twitch.tv", 6667);
        private TaskCompletionSource<bool> _connect;
        private Timer _connectionTimer;
        private TaskCompletionSource<bool> _disconnect;
        private bool _isConnected = false;
        private bool _isDisposed = false;
        private TaskCompletionSource<bool> _ping;
        private TaskCompletionSource<bool> _sendMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitchChat"/> class,
        /// using the specified user name and OAuth key.
        /// </summary>
        public TwitchChat()
        {
            IrcClient = new StandardIrcClient()
            {
                FloodPreventer = new IrcStandardFloodPreventer(10, 3000)
            };

            IrcClient.Error += IrcClient_Error;
            IrcClient.Connected += IrcClient_Connected;
            IrcClient.ConnectFailed += IrcClient_ConnectFailed;
            IrcClient.Disconnected += IrcClient_Disconnected;
            IrcClient.RawMessageReceived += IrcClient_RawMessageReceived;
            IrcClient.RawMessageSent += IrcClient_RawMessageSent;
            IrcClient.PingReceived += IrcClient_PingReceived;
            IrcClient.PongReceived += IrcClient_PongReceived;

            _connectionTimer = new Timer(ConnectionTimerCallback,
                state: null, dueTime: s_timeout, period: s_timeout);
        }

        /// <summary>
        /// Occurs when the Twitch client was disconnected without calling the
        /// <see cref="DisconnectAsync"/> method.
        /// </summary>
        public event EventHandler ConnectionLost;

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
        /// Gets a value indicating whether a connection with Twitch chat
        /// servers is currently established.
        /// </summary>
        public bool IsConnected
        {
            // Note: IrcClient has an IsConnected property, but it is unreliable
            //       and will return true even without an internet connection.
            get { return _isConnected; }
            protected set { _isConnected = value; }
        }

        /// <summary>
        /// Gets the date and time of the last activity with the servers.
        /// </summary>
        public DateTime LastActivity { get; protected set; }

        /// <summary>
        /// Gets the name of the user that is connected to Twitch.
        /// </summary>
        public string UserName { get; protected set; }

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
            _connect = new TaskCompletionSource<bool>();

            var credentials = new IrcUserRegistrationInfo()
            {
                NickName = userName,
                UserName = userName,
                Password = key
            };
            IrcClient.Connect(_twitchEP, false, credentials);
            await _connect.Task;

            UserName = userName;
            _connect = null;
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
            _disconnect = new TaskCompletionSource<bool>();

            IrcClient.Quit(Strings.QuitMessage);
            await _disconnect.Task;

            _disconnect = null;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="TwitchChat"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Checks whether the connection to the Twitch chat server is still up.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> object representing the result of the
        /// asynchronous operation. The task result indicates whether the server
        /// responded to a PING request.
        /// </returns>
        public async Task<bool> IsAlive()
        {
            _ping = new TaskCompletionSource<bool>();

            IrcClient.Ping();
            var success = await _ping.Task;

            _ping = null;
            return success;
        }

        /// <summary>
        /// Joins the specified channel asynchronously.
        /// </summary>
        /// <param name="channel">The channel to join.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task JoinAsync(Channel channel)
        {
            _sendMessage = new TaskCompletionSource<bool>();

            var message = new JoinMessage(channel);
            IrcClient.SendRawMessage(message.ConstructCommand());
            await _sendMessage.Task;

            _sendMessage = null;

            // We don't really care about the result of the operation right now,
            // this is just to make sure we've seen the display name of the
            // channel host.
            var user = new User(channel);
            var task = user.Load();
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
            foreach (var channelName in channels)
            {
                var channel = new Channel(channelName);
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
                _sendMessage = new TaskCompletionSource<bool>();

                IrcClient.SendRawMessage(raw);
                await _sendMessage.Task;

                _sendMessage = null;
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
                    _connectionTimer?.Dispose();
                }

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="ConnectionLost"/> event.
        /// </summary>
        protected virtual void OnConnectionLost()
        {
            IsConnected = false;

            // If we don't manually disconnect after losing connection, we won't
            // be able to reconnect because of an IsConnected SocketException...
            IrcClient.Disconnect();

            ConnectionLost?.Invoke(this, EventArgs.Empty);
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

        private async void ConnectionTimerCallback(object state)
        {
            if (!IsConnected) return;

            if (!await IsAlive())
            {
                IsConnected = false;
                OnConnectionLost();
            }
        }

        private void IrcClient_Connected(object sender, EventArgs e)
        {
            Debug.Assert(_connect != null,
                nameof(_connect) + " cannot be null",
                nameof(_connect) + " should always be created before calling Connect");

            Debug.WriteLine(string.Format("Connected to {0}", _twitchEP), "Info");
            // IrcClient.SendRawMessage("CAP REQ :twitch.tv/membership");
            IrcClient.SendRawMessage("CAP REQ :twitch.tv/tags");
            IrcClient.SendRawMessage("CAP REQ :twitch.tv/commands");

            IsConnected = true;
            _connect.SetResult(true);
        }

        private void IrcClient_ConnectFailed(object sender, IrcErrorEventArgs e)
        {
            Debug.Assert(_connect != null,
                nameof(_connect) + " cannot be null",
                nameof(_connect) + " should always be created before calling Connect");

            IsConnected = false;
            _connect.SetException(e.Error);
        }

        private void IrcClient_Disconnected(object sender, EventArgs e)
        {
            IsConnected = false;

            if (_disconnect != null)
                _disconnect.TrySetResult(true);
        }

        private void IrcClient_Error(object sender, IrcErrorEventArgs e)
        {
            if (e.Error is SocketException)
            {
                var ex = e.Error as SocketException;
                switch (ex.SocketErrorCode)
                {
                    case SocketError.NetworkDown:
                    case SocketError.NetworkReset:
                    case SocketError.NetworkUnreachable:
                    case SocketError.ConnectionAborted:
                    case SocketError.ConnectionRefused:
                        // If we're connected, these errors indicate a lost
                        // connection and the client can retry. If we're not
                        // connected yet, we should throw instead.
                        if (IsConnected)
                        {
                            IsConnected = false;
                            OnConnectionLost();
                            return;
                        }
                        break;

                    case SocketError.OperationAborted:
                        // This error usually occurs while disconnected. Why
                        // doesn't IrcDotNet catch this?
                        Debug.WriteLine(ex, "Error ignored");
                        return;

                    case SocketError.IsConnected:
                    default:
                        // Anything else is probably a bug.
                        break;
                }

                if (_connect != null)
                    _connect.SetException(e.Error);
                else
                    throw e.Error;
            }

            // Don't throw just any error, because IrcDotNet is kinda shit and
            // anything could happen here.
            Debug.WriteLine(e.Error, Strings.IrcDotNetError);
        }

        private void IrcClient_PingReceived(object sender, IrcPingOrPongReceivedEventArgs e)
        {
            LastActivity = DateTime.Now;
        }

        private void IrcClient_PongReceived(object sender, IrcPingOrPongReceivedEventArgs e)
        {
            Debug.Assert(_ping != null,
                nameof(_ping) + " cannot be null",
                nameof(_ping) + " should always be created before calling Ping");

            LastActivity = DateTime.Now;
            _ping.SetResult(true);
        }

        private void IrcClient_RawMessageReceived(object sender, IrcRawMessageEventArgs e)
        {
            LastActivity = DateTime.Now;
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
                Debug.WriteLine(message.RawMessage, Strings.UnhandledMessageReceived);
        }

        private void IrcClient_RawMessageSent(object sender, IrcRawMessageEventArgs e)
        {
            LastActivity = DateTime.Now;
            if (_sendMessage != null)
                _sendMessage.TrySetResult(true);
        }
    }
}
