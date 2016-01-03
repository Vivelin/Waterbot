using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Waterbot.Config;

namespace Waterbot
{
    /// <summary>
    /// Represents a Waterbot configuration.
    /// </summary>
    [Serializable]
    public class Configuration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class
        /// using default values.
        /// </summary>
        public Configuration()
        {
            Credentials = new Credentials();
            DefaultChannels = new List<string>();
        }

        /// <summary>
        /// Gets the name of the default configuration file.
        /// </summary>
        public static string DefaultFileName
        {
            get
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appData, "waterbot", "config.json");
            }
        }

        /// <summary>
        /// Gets or sets the credentials used for accessing Twitch chat and the
        /// Twitch API.
        /// </summary>
        public Credentials Credentials { get; set; }

        /// <summary>
        /// Gets a list of channels to connect to on startup.
        /// </summary>
        public IList<string> DefaultChannels { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="Configuration"/> class from
        /// the default configuration file.
        /// </summary>
        /// <returns>A new <see cref="Configuration"/> object.</returns>
        public static Configuration FromFile()
        {
            return FromFile(DefaultFileName);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Configuration"/> class with
        /// the contents of the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to load.</param>
        /// <returns>
        /// A new <see cref="Configuration"/> object, or <c>null</c> if the
        /// specified file does not exist.
        /// </returns>
        public static Configuration FromFile(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Load(stream);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Configuration"/> class with
        /// the contents of the specified stream.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <returns>A new <see cref="Configuration"/> object.</returns>
        public static Configuration Load(Stream stream)
        {
            string contents;
            using (var reader = new StreamReader(stream))
            {
                contents = reader.ReadToEnd();
            }

            return Load(contents);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Configuration"/> class with
        /// the specified contents.
        /// </summary>
        /// <param name="contents">The contents to load.</param>
        /// <returns>A new <see cref="Configuration"/> object.</returns>
        public static Configuration Load(string contents)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            return JsonConvert.DeserializeObject<Configuration>(contents, settings);
        }

        /// <summary>
        /// Saves the configuration to the specified stream.
        /// </summary>
        /// <param name="destination">
        /// The stream to write to. The stream will be closed.
        /// </param>
        public void Save(Stream destination)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Include
            };

            var value = JsonConvert.SerializeObject(this, settings);
            using (var writer = new StreamWriter(destination))
            {
                writer.WriteLine(value);
            }
        }

        /// <summary>
        /// Saves the configuration to the default file.
        /// </summary>
        public void Save()
        {
            Save(DefaultFileName);
        }

        /// <summary>
        /// Saves the configuration to the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to write to.</param>
        public void Save(string fileName)
        {
            var folder = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            Save(stream);
        }
    }
}
