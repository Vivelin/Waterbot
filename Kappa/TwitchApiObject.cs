using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kappa
{
    /// <summary>
    /// Represents an object whose properties can be loaded with a Twitch API
    /// call.
    /// </summary>
    public abstract class TwitchApiObject : ApiObject
    {
        private readonly Uri apiBase = new Uri("https://api.twitch.tv/kraken/");

        /// <summary>
        /// Gets the address at which the API is located.
        /// </summary>
        protected override Uri ApiAddress => new Uri(apiBase, Endpoint);

        /// <summary>
        /// When overridden in a derived object, gets the Twitch API endpoint to
        /// the object to load.
        /// </summary>
        protected abstract string Endpoint { get; }

        /// <summary>
        /// When overridden in a derived object, gets the name of the JSON
        /// property that contains the actual data.
        /// </summary>
        protected virtual string PropertyName { get; }

        /// <summary>
        /// Makes a GET request to the API and returns the result.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        protected override async Task<string> Get()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v3+json");
                // client.DefaultRequestHeaders.Add("Client-ID", "");
                return await client.GetStringAsync(ApiAddress);
            }
        }

        /// <summary>
        /// Populates the current stream with the specified data.
        /// </summary>
        /// <param name="data">The data to populate the stream with.</param>
        protected override void Populate(string data)
        {
            if (!string.IsNullOrEmpty(PropertyName))
            {
                var obj = JObject.Parse(data);
                var token = obj[PropertyName].ToString();
                if (!string.IsNullOrEmpty(token))
                    JsonConvert.PopulateObject(token, this);
            }
            else
            {
                base.Populate(data);
            }
        }
    }
}
