using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Kappa;

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
        private Behavior behavior = null;
        private Configuration currentConfig = null;
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Waterbot"/> class using
        /// the specified configuration.
        /// </summary>
        /// <param name="config">The configuration to use.</param>
        public Waterbot(Configuration config)
        {
            currentConfig = config;

            TwitchChat = new TwitchChat();
            TwitchChat.Disconnected += TwitchChat_Disconnected;
            TwitchChat.MessageReceived += TwitchChat_MessageReceived;

            Channels = new List<string>();
        }

        /// <summary>
        /// Occurs when the current configuration has changed.
        /// </summary>
        public event EventHandler ConfigChanged;

        /// <summary>
        /// Occurs when a chat message has been received.
        /// </summary>
        public event EventHandler<ChatMessageEventArgs> MessageReceived;

        /// <summary>
        /// Occurs when a chat message was sent by the bot.
        /// </summary>
        public event EventHandler<ChatMessageEventArgs> MessageSent;

        /// <summary>
        /// Gets or sets <see cref="Waterbot"/>'s behavior.
        /// </summary>
        public Behavior Behavior
        {
            get
            {
                if (behavior == null)
                    behavior = CreateBehavior();
                return behavior;
            }
            set { behavior = value; }
        }

        /// <summary>
        /// Gets a list of channels the bot is currently connected to.
        /// </summary>
        public IList<string> Channels { get; }

        /// <summary>
        /// Gets or sets the configuration to use.
        /// </summary>
        public Configuration Config
        {
            get { return currentConfig; }
            set
            {
                if (currentConfig != value)
                {
                    currentConfig = value;
                    OnConfigChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="TwitchChat"/> object used to communicate with
        /// Twitch chat.
        /// </summary>
        protected TwitchChat TwitchChat { get; }

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
        /// <param name="channel">The name of the channel to join.</param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task JoinAsync(string channel)
        {
            await TwitchChat.JoinAsync(channel);
            Channels.Add(channel);

            var message = Behavior.GetJoinMessage(channel);
            if (message != null)
            {
                await TwitchChat.SendMessage(message);
                OnMessageSent(new ChatMessageEventArgs(message));
            }
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
            foreach (var channel in Channels)
            {
                var message = Behavior.GetPartMessage(channel);
                if (message != null)
                {
                    await TwitchChat.SendMessage(message);
                    OnMessageSent(new ChatMessageEventArgs(message));
                }
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
                    return new Behaviors.Kusogechan(user);

                case "horsedrowner":
                    return new Behaviors.NoBehavior(user);

                default:
                    Trace.WriteLine($"No behavior specified for {user}, falling back to default behavior", "WARNING");
                    return new DefaultBehavior(user);
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
            if (!isDisposed)
            {
                if (disposing)
                {
                    TwitchChat?.Dispose();
                }

                isDisposed = true;
            }
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
        }

        private void TwitchChat_Disconnected(object sender, EventArgs e)
        {
            // TODO: Implement automatic reconnecting
            Console.WriteLine("Disconnected from Twitch!");
        }

        private async void TwitchChat_MessageReceived(object sender, ChatMessageEventArgs e)
        {
            OnMessageReceived(e);

            if (string.Compare(e.Message.UserName, Config.Credentials.UserName, true) == 0)
            {
                Trace.WriteLine("I think I'm talking to myself.");
                return;
            }

            var response = Behavior.ProcessMessage(e.Message);
            if (response != null)
            {
                await TwitchChat.SendMessage(response);
                OnMessageSent(new ChatMessageEventArgs(response));
            }
        }
    }
}
