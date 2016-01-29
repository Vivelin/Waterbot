using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kappa;
using Waterbot.Common;

namespace Waterbot
{
    /// <summary>
    /// Represents the Waterbot server.
    /// </summary>
    /// <example>
    /// <code>
    /// using (var waterbot = new Waterbot.Server())
    /// {
    ///     waterbot.UserName = "horsedrowner";
    ///     waterbot.OAuthKey = "oauth:##############################";
    ///     await waterbot.StartAsync(waterbot.UserName);
    ///     // ...
    ///     await waterbot.StopAsync();
    /// }
    /// </code>
    /// </example>
    public class Waterbot : IDisposable
    {
        private Behavior _behavior = null;
        private CancellationTokenSource _cancelSource = new CancellationTokenSource();
        private Configuration _currentConfig = null;
        private bool _isDisposed = false;
        private bool _isMuted = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Waterbot"/> class using
        /// the specified configuration.
        /// </summary>
        /// <param name="config">The configuration to use.</param>
        public Waterbot(Configuration config)
        {
            _currentConfig = config;
            TwitchApiObject.ClientId = config.Credentials.ClientId;

            TwitchChat = new TwitchChat();
            TwitchChat.ConnectionLost += TwitchChat_ConnectionLost;
            TwitchChat.MessageReceived += TwitchChat_MessageReceived;
            TwitchChat.NoticeReceived += TwitchChat_NoticeReceived;

            Channels = new List<Channel>();
            LastActivity = new Dictionary<string, DateTime>();
        }

        /// <summary>
        /// Occurs when the bot has joined a channel.
        /// </summary>
        public event EventHandler<ChannelEventArgs> ChannelJoined;

        /// <summary>
        /// Occurs when the current configuration has changed.
        /// </summary>
        public event EventHandler ConfigChanged;

        /// <summary>
        /// Occurs when a chat message was not sent because the bot was muted.
        /// </summary>
        public event EventHandler<ChatMessageEventArgs> MessageMuted;

        /// <summary>
        /// Occurs when a chat message has been received.
        /// </summary>
        public event EventHandler<ChatMessageEventArgs> MessageReceived;

        /// <summary>
        /// Occurs when a chat message was sent by the bot.
        /// </summary>
        public event EventHandler<ChatMessageEventArgs> MessageSent;

        /// <summary>
        /// Occurs when a notice was received.
        /// </summary>
        [Obsolete("This is a temporary event and will be replaced by more specific events in the future.")]
        public event EventHandler<MessageEventArgs> NoticeReceived;

        /// <summary>
        /// Gets or sets <see cref="Waterbot"/>'s behavior.
        /// </summary>
        public Behavior Behavior
        {
            get
            {
                if (_behavior == null)
                    _behavior = CreateBehavior();
                return _behavior;
            }
            set { _behavior = value; }
        }

        /// <summary>
        /// Gets a list of channels the bot is currently connected to.
        /// </summary>
        public IList<Channel> Channels { get; }

        /// <summary>
        /// Gets or sets the configuration to use.
        /// </summary>
        public Configuration Config
        {
            get { return _currentConfig; }
            set
            {
                if (_currentConfig != value)
                {
                    _currentConfig = value;
                    OnConfigChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a dictionary that keeps the time of the last bot activity in
        /// each channel.
        /// </summary>
        public IDictionary<string, DateTime> LastActivity { get; }

        /// <summary>
        /// Gets the <see cref="TwitchChat"/> object used to communicate with
        /// Twitch chat.
        /// </summary>
        protected TwitchChat TwitchChat { get; }

        /// <summary>
        /// Returns an enumerable collection of command factories.
        /// </summary>
        /// <returns>
        /// An enumerable collection of objects that implement <see
        /// cref="ICommandManufactorum"/>.
        /// </returns>
        public static IEnumerable<ICommandManufactorum> EnumerateCommandFactories()
        {
            var interfaceType = typeof(ICommandManufactorum);
            var factorumTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract)
                .Where(type => interfaceType.IsAssignableFrom(type));

            foreach (var type in factorumTypes)
            {
                yield return Activator.CreateInstance(type) as ICommandManufactorum;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="Waterbot"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Joins the specified channel asynchronously.
        /// </summary>
        /// <param name="channelName">The name of the channel to join.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task JoinAsync(string channelName)
        {
            var channel = new Channel(channelName);
            await JoinAsync(channel);
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
            await TwitchChat.JoinAsync(channel);
            OnChannelJoined(new ChannelEventArgs(channel));

            var message = Behavior.GetJoinMessage(channel);
            await SendMessageAsync(message);
        }

        /// <summary>
        /// Joins the specified channels asynchronously.
        /// </summary>
        /// <param name="channels">
        /// A collection of strings containing the channel names to join.
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
        /// Joins the specified channels asynchronously.
        /// </summary>
        /// <param name="channels">A collection of channels to join.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task JoinAsync(IEnumerable<Channel> channels)
        {
            foreach (var channel in channels)
            {
                await JoinAsync(channel);

                // JOINs are rate-limited at 50 per 15 seconds => 3/s
                await Task.Delay(333);
            }
        }

        /// <summary>
        /// Sends a chat message.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public virtual async Task SendMessageAsync(ChatMessage message)
        {
            if (message != null)
            {
                var e = new ChatMessageEventArgs(message);
                if (Behavior.Mute && _isMuted)
                {
                    OnMessageMuted(e);
                }
                else
                {
                    await TwitchChat.SendMessage(message);
                    OnMessageSent(e);
                }

                // Delayed mute to allow one final response
                _isMuted = Behavior.Mute;
            }
        }

        /// <summary>
        /// Connects to Twitch chat and begins performing operations.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task StartAsync()
        {
            await TwitchChat.ConnectAsync(Config.Credentials.UserName,
                Config.Credentials.OAuthToken);

            await JoinAsync(Config.Credentials.UserName);
            await JoinAsync(Config.DefaultChannels);

            var thread = new Thread(async () =>
            {
                await RunAsync(_cancelSource.Token);
            });
            thread.Start();
        }

        /// <summary>
        /// Stops the server and disconnects the bot from Twitch chat.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task StopAsync()
        {
            _cancelSource.Cancel();

            foreach (var channel in Channels)
            {
                var message = Behavior.GetPartMessage(channel);
                await SendMessageAsync(message);
            }

            await TwitchChat.DisconnectAsync();
        }

        /// <summary>
        /// Gets a new instance of the behavior appropiate for this instance.
        /// </summary>
        /// <returns>A new <see cref="Behavior"/> object.</returns>
        protected virtual Behavior CreateBehavior()
        {
            var user = Config.Credentials.UserName;
            switch (user.ToLower())
            {
                case "kusogechan":
                    return new Behaviors.Kusogechan(Config);

                case "horsedrowner":
                    return new Behaviors.NoBehavior(Config);

                default:
                    Log.Add(Strings.BehaviorFallback.With(user));
                    return new DefaultBehavior(Config);
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="Waterbot"/> object.
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
                    TwitchChat?.Dispose();
                }

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="ChannelJoined"/> event.
        /// </summary>
        /// <param name="args">
        /// A <see cref="ChannelEventArgs"/> object that provides data for the
        /// event.
        /// </param>
        protected virtual void OnChannelJoined(ChannelEventArgs args)
        {
            ChannelJoined?.Invoke(this, args);

            Channels.Add(args.Channel);
            LastActivity[args.Channel.Name] = DateTime.Now;
        }

        /// <summary>
        /// Raises the <see cref="ConfigChanged"/> event.
        /// </summary>
        /// <param name="args">
        /// An <see cref="EventArgs"/> object providing data for the event.
        /// </param>
        protected virtual void OnConfigChanged(EventArgs args)
        {
            ConfigChanged?.Invoke(this, args);

            if (Config != null)
            {
                TwitchApiObject.ClientId = Config.Credentials.ClientId;
            }
        }

        /// <summary>
        /// Raises the <see cref="MessageMuted"/> event.
        /// </summary>
        /// <param name="args">
        /// A <see cref="ChatMessageEventArgs"/> object providing data for the
        /// event.
        /// </param>
        protected virtual void OnMessageMuted(ChatMessageEventArgs args)
        {
            MessageMuted?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="MessageReceived"/> event.
        /// </summary>
        /// <param name="args">
        /// A <see cref="ChatMessageEventArgs"/> object providing data for the
        /// event.
        /// </param>
        protected virtual void OnMessageReceived(ChatMessageEventArgs args)
        {
            MessageReceived?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="MessageSent"/> event.
        /// </summary>
        /// <param name="args">
        /// A <see cref="ChatMessageEventArgs"/> object providing data for the
        /// event.
        /// </param>
        protected virtual void OnMessageSent(ChatMessageEventArgs args)
        {
            MessageSent?.Invoke(this, args);

            LastActivity[args.Message.Channel.Name] = DateTime.Now;
        }

        /// <summary>
        /// Raises the <see cref="NoticeReceived"/> event.
        /// </summary>
        /// <param name="args">
        /// A <see cref="MessageEventArgs"/> object providing data for the
        /// event.
        /// </param>
        protected virtual void OnNoticeReceived(MessageEventArgs args)
        {
            NoticeReceived?.Invoke(this, args);
        }

        /// <summary>
        /// Attempts to reconnect to the Twitch chat servers and rejoin the
        /// specified channels.
        /// </summary>
        /// <param name="channels">
        /// A collection of the channels to rejoin.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> object that represents the result of
        /// the asynchronous operation, containing a boolean value that
        /// indicates whether the reconnect attempt was successful. If the bot
        /// is still connected, the result of the returned task will be
        /// <c>true</c>.
        /// </returns>
        protected async Task<bool> ReconnectAsync(IEnumerable<Channel> channels)
        {
            if (!TwitchChat.IsConnected)
            {
                try
                {
                    await TwitchChat.ConnectAsync(Config.Credentials.UserName,
                        Config.Credentials.OAuthToken);
                }
                catch (Exception ex)
                {
                    Log.AddException(ex);
                    return false;
                }

                await JoinAsync(channels);
            }
            return true;
        }

        /// <summary>
        /// Performs background operations that do not involve responding to
        /// events.
        /// </summary>
        /// <param name="cancelToken">
        /// A cancellation token that can be used to stop operations.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        protected async Task RunAsync(System.Threading.CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                for (var i = 0; i < LastActivity.Count; i++)
                {
                    if (cancelToken.IsCancellationRequested)
                        break;

                    var item = LastActivity.ElementAt(i);
                    var channel = item.Key;
                    var elapsed = DateTime.Now - item.Value;
                    if (elapsed > Config.Behavior.IdleTimeout)
                    {
                        var message = await Behavior.GetIdleMessage(new Channel(channel));
                        await SendMessageAsync(message);

                        LastActivity[channel] = DateTime.Now;
                    }
                }

                if (!cancelToken.IsCancellationRequested)
                    await Task.Delay(1000);
            }
        }

        private async void TwitchChat_ConnectionLost(object sender, EventArgs e)
        {
            Console.WriteLine(Strings.ConnectionLost);
            var prevChannels = Channels.ToList(); // Clone
            Channels.Clear();

            var reconnected = await ReconnectAsync(prevChannels);
            var timeout = TimeSpan.FromSeconds(15);
            while (!reconnected && !_cancelSource.IsCancellationRequested)
            {
                Console.WriteLine(Strings.ConnectionRetrying, timeout.ToText());
                await Task.Delay(timeout, _cancelSource.Token);

                reconnected = await ReconnectAsync(prevChannels);
                if (timeout < TimeSpan.FromHours(1))
                    timeout += timeout;
            }

            Console.WriteLine(Strings.ConnectionRestored);
        }

        private async void TwitchChat_MessageReceived(object sender, ChatMessageEventArgs e)
        {
            OnMessageReceived(e);

            if (string.Compare(e.Message.User.Name, Config.Credentials.UserName, true) == 0)
            {
                Log.Add(Strings.TalkingToMyself);
                return;
            }

            var response = await Behavior.ProcessMessage(e.Message);
            await SendMessageAsync(response);
        }

        private async void TwitchChat_NoticeReceived(object sender, MessageEventArgs e)
        {
            OnNoticeReceived(e);

            var response = await Behavior.GetFailureResponse(e.Message as NoticeMessage);
            await SendMessageAsync(response);
        }
    }
}
