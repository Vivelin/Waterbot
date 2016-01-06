using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kappa
{
    /// <summary>
    /// Represents an object whose properties can be loaded with an API call.
    /// </summary>
    public abstract class ApiObject
    {
        /// <summary>
        /// Gets a value that indicates when the object has last been loaded
        /// using live data; otherwise, returns <c>null</c>.
        /// </summary>
        public DateTime? Loaded { get; internal set; }

        /// <summary>
        /// When overridden in a derived class, gets the address at which the
        /// API is located.
        /// </summary>
        protected abstract Uri ApiAddress { get; }

        /// <summary>
        /// Populates the current object using live data.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        public virtual async Task Load()
        {
            var data = await Get();
            Populate(data);

            Loaded = DateTime.Now;
        }

        /// <summary>
        /// Makes a GET request to the API and returns the result.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> object representing the result of the
        /// asynchronous operation.
        /// </returns>
        protected virtual async Task<string> Get()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                return await client.GetStringAsync(ApiAddress);
            }
        }

        /// <summary>
        /// Populates the current object with the specified data.
        /// </summary>
        /// <param name="data">The data to populate the object with.</param>
        protected virtual void Populate(string data)
        {
            JsonConvert.PopulateObject(data, this);
        }
    }
}
