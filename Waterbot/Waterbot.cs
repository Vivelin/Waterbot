using System;
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
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Waterbot"/> class.
        /// </summary>
        public Waterbot()
        {
            TwitchClient = new TwitchClient();
            TwitchClient.Disconnected += TwitchClient_Disconnected;
            TwitchClient.MessageReceived += TwitchClient_MessageReceived;
        }

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
                    behavior = GetBehavior();
                return behavior;
            }
            set { behavior = value; }
        }

        /// <summary>
        /// Gets or sets the OAuth key used in place of a password when
        /// connecting to Twitch chat.
        /// </summary>
        /// <remarks>
        /// An OAuth key may be generated at https://twitchapps.com/tmi/.
        /// </remarks>
        public string OAuthKey { get; set; }

        /// <summary>
        /// Gets or sets the user name that the bot identifies itself as and
        /// which is used to connect to Twitch chat
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets the <see cref="TwitchClient"/> object used to communicate with
        /// Twitch chat.
        /// </summary>
        protected TwitchClient TwitchClient { get; }

        /// <summary>
        /// Releases all resources used by the <see cref="Waterbot"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Connects to Twitch chat and begins performing operations.
        /// </summary>
        /// <param name="channels">
        /// An array of strings containing the names of the channels to
        /// initially connect to.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public async Task StartAsync(params string[] channels)
        {
            await TwitchClient.ConnectAsync(UserName, OAuthKey, channels);
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
            await TwitchClient.DisconnectAsync();
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
                    TwitchClient?.Dispose();
                }

                isDisposed = true;
            }
        }

        /// <summary>
        /// Gets a new instance of the behavior appropiate for this instance.
        /// </summary>
        /// <returns>A new <see cref="Behavior"/> object.</returns>
        protected virtual Behavior GetBehavior()
        {
            switch (UserName.ToLower())
            {
                case "kusogechan":
                    return new Behaviors.Kusogechan();

                default:
                    Trace.WriteLine($"No behavior specified for {UserName}, falling back to default behavior", "WARNING");
                    return new DefaultBehavior();
            }
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

        private void TwitchClient_Disconnected(object sender, EventArgs e)
        {
            // TODO: Implement automatic reconnecting
            Console.WriteLine("Disconnected from Twitch!");
        }

        private void TwitchClient_MessageReceived(object sender, ChatMessageEventArgs e)
        {
            OnMessageReceived(e);

            if (string.Compare(e.Message.UserName, UserName, true) == 0)
            {
                Trace.WriteLine("I think I'm talking to myself.");
                return;
            }

            var response = Behavior.GetResponse(e.Message);
            if (response != null)
            {
                TwitchClient.SendMessage(response);
                OnMessageSent(new ChatMessageEventArgs(response));
            }
        }
    }
}
