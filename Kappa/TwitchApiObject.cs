using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kappa
{
    /// <summary>
    /// Represents an object whose properties can be loaded with a Twitch API
    /// call.
    /// </summary>
    public abstract class TwitchApiObject : ApiObject
    {
        private readonly Uri _apiBase = new Uri("https://api.twitch.tv/kraken/");

        /// <summary>
        /// Gets or sets the Twitch Developer Application's client ID.
        /// </summary>
        public static string ClientId { get; set; }

        /// <summary>
        /// Gets the address at which the API is located.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        protected override Uri ApiAddress => new Uri(_apiBase, Endpoint);

        /// <summary>
        /// When overridden in a derived object, gets the Twitch API endpoint to
        /// the object to load.
        /// </summary>
        protected abstract string Endpoint { get; }

        /// <summary>
        /// Makes a GET request to the API and returns the result.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        protected override async Task<string> PerformRequest()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v3+json");
                if (ClientId != null)
                    client.DefaultRequestHeaders.Add("Client-ID", ClientId);

                return await client.GetStringAsync(ApiAddress);
            }
        }
    }
}
