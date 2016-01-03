using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kappa;

namespace Waterbot.WaterbotServer
{
    internal class Program
    {
        private static TaskCompletionSource<bool> quitTask;

        private static List<string> Channels { get; set; }
        private static string ConfigFile { get; set; }

        /// <summary>
        /// Loads the configuration file, creating a new one if the default
        /// configuration is not present and no custom configuration file has
        /// been specified.
        /// </summary>
        /// <returns>
        /// A <see cref="Configuration"/> object, or <c>null</c> if no
        /// configuration was loaded.
        /// </returns>
        private static Configuration LoadConfig()
        {
            Configuration config = null;

            if (string.IsNullOrEmpty(ConfigFile))
            {
                config = Configuration.FromFile();
                if (config == null)
                {
                    config = new Configuration();
                    config.Save();

                    Console.WriteLine("A default configuration has been written to {0}. Please check the file, modify it where necessary, then start Waterbot again.", Configuration.DefaultFileName);
                    return null;
                }
            }
            else
            {
                config = Configuration.FromFile(ConfigFile);
                if (config == null)
                {
                    Console.WriteLine("Could not load {0}. Please check whether the file is accessible and valid and try again.", ConfigFile);
                    return null;
                }
            }

            return config;
        }

        private static void Main(string[] args)
        {
            var showHelp = false;

            var options = new Mono.Options.OptionSet();
            options.Add("config=", "The file name of the configuration to load.",
                value => ConfigFile = value);
            options.Add("h|help|?", "Prints this message and exits.",
                value => showHelp = (value != null));

            Channels = options.Parse(args);
            if (Channels == null || Channels.Count == 0)
            {
                Channels = new List<string>();
            }

            if (showHelp)
            {
                Console.WriteLine("Runs the Waterbot server.");
                Console.WriteLine();
                Console.WriteLine("WaterbotServer [--config=VALUE] [channel]");
                Console.WriteLine();
                options.WriteOptionDescriptions(Console.Out);
            }
            else
            {
                Task.WaitAll(MainAsync());
            }
        }

        private static async Task MainAsync()
        {
            Console.Title = "Waterbot server";

            // Prepare a task which completes when Ctrl+C is pressed
            quitTask = new TaskCompletionSource<bool>();
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                quitTask.SetResult(true);
            };

            var config = LoadConfig();
            if (config == null)
                return;

            Channels.AddRange(config.DefaultChannels);
            if (!Channels.Contains(config.Credentials.UserName))
                Channels.Add(config.Credentials.UserName);

            using (var waterbot = new Waterbot(config))
            {
                waterbot.MessageReceived += Waterbot_MessageReceived;
                waterbot.MessageSent += Waterbot_MessageSent;

                await waterbot.StartAsync(Channels.Distinct());
                Console.WriteLine("Press Ctrl+C to stop Waterbot");

                // Wait until Ctrl+C is pressed, then exit gracefully
                await quitTask.Task;
                await waterbot.StopAsync();
            }

            config.Save();
        }

        private static void Waterbot_MessageReceived(object sender, ChatMessageEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[");
            Console.Write(e.Message.Channel);
            Console.Write("] ");

            if (e.Message.IsBroadcaster)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (e.Message.IsAdmin)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (e.Message.IsMod)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (e.Message.IsSub)
                Console.ForegroundColor = ConsoleColor.Blue;
            else
                Console.ResetColor();

            Console.Write(e.Message.DisplayName);
            Console.Write(": ");

            Console.ResetColor();
            Console.WriteLine(e.Message.Contents);
        }

        private static void Waterbot_MessageSent(object sender, ChatMessageEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[");
            Console.Write(e.Message.Channel);
            Console.Write("] ");

            Console.WriteLine(e.Message.Contents);
            Console.ResetColor();
        }
    }
}
