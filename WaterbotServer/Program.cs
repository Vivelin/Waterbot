using System;
using System.Collections.Generic;
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
            try
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

                using (var waterbot = new Waterbot(config))
                {
                    waterbot.MessageReceived += Waterbot_MessageReceived;
                    waterbot.MessageSent += Waterbot_MessageSent;
                    waterbot.MessageMuted += Waterbot_MessageMuted;
                    waterbot.NoticeReceived += Waterbot_NoticeReceived;

                    await waterbot.StartAsync();
                    Console.WriteLine("Press Ctrl+C to stop Waterbot");

                    await waterbot.JoinAsync(Channels);

                    // Wait until Ctrl+C is pressed, then exit gracefully
                    await quitTask.Task;
                    await waterbot.StopAsync();
                }

                config.Save();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
            }
        }

        private static void Waterbot_MessageMuted(object sender, ChatMessageEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[");
            Console.Write(e.Message.Channel);
            Console.Write("] ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[MUTED] ");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(e.Message.Contents);

            Console.ResetColor();
        }

        private static void Waterbot_MessageReceived(object sender, ChatMessageEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[");
            Console.Write(e.Message.Channel);
            Console.Write("] ");

            if (e.Message.IsTwitchNotify)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(e.Message.Contents);

                Console.ResetColor();
            }
            else
            {
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

                Console.Write(e.Message.User);
                Console.Write(": ");

                Console.ResetColor();
                Console.WriteLine(e.Message.Contents);
            }
        }

        private static void Waterbot_MessageSent(object sender, ChatMessageEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[");
            Console.Write(e.Message.Channel);
            Console.Write("] ");

            Console.ResetColor();
            Console.WriteLine(e.Message.Contents);
        }

        private static void Waterbot_NoticeReceived(object sender, MessageEventArgs e)
        {
            var message = e.Message as NoticeMessage;

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[");
            Console.Write(message.Channel);
            Console.Write("] ");

            Console.Write(message.Text);

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
