using System;
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
    public class Server : IDisposable
    {
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        public Server()
        {
            TwitchClient = new TwitchClient();
            TwitchClient.Disconnected += TwitchClient_Disconnected;
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
        /// Releases all resources used by the <see cref="Server"/> object.
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
        /// Releases all resources used by the <see cref="Server"/> object.
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

        private void TwitchClient_Disconnected(object sender, EventArgs e)
        {
            // TODO: Implement automatic reconnecting
            Console.WriteLine("Disconnected from Twitch!");
        }
    }
}
