using System;
using System.Threading.Tasks;

namespace WaterbotServer
{
    internal class Program
    {
        private static TaskCompletionSource<bool> quitTask;

        private static string OAuthKey { get; set; }
        private static string UserName { get; set; }

        private static void Main(string[] args)
        {
            var showHelp = false;

            var options = new Mono.Options.OptionSet();
            options.Add("user=", "The user name that Waterbot should use.",
                value => UserName = value);
            options.Add("key=", "The OAuth key to connect to Twitch chat with.",
                value => OAuthKey = value);
            options.Add("h|help|?", "Prints this message and exits.",
                value => showHelp = (value != null));
            options.Parse(args);

            if (showHelp || UserName == null || OAuthKey == null)
            {
                Console.WriteLine("Runs the Waterbot server.");
                Console.WriteLine();
                Console.WriteLine("WaterbotServer --user=VALUE --key=VALUE");
                Console.WriteLine();
                options.WriteOptionDescriptions(Console.Out);
            }
            else
            {
                Task.WaitAll(MainAsync(args));
            }
        }

        private static async Task MainAsync(string[] args)
        {
            // Prepare a task which completes when Ctrl+C is pressed
            quitTask = new TaskCompletionSource<bool>();
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                quitTask.SetResult(true);
            };

            using (var waterbot = new Waterbot.Server())
            {
                waterbot.UserName = UserName;
                waterbot.OAuthKey = OAuthKey;
                waterbot.IrcMessageReceived += (sender, e) => Console.WriteLine(e);

                await waterbot.StartAsync(waterbot.UserName);
                Console.WriteLine("Press Ctrl+C to stop Waterbot");

                // Wait until Ctrl+C is pressed, then exit gracefully
                await quitTask.Task;
                await waterbot.StopAsync();
            }
        }
    }
}
