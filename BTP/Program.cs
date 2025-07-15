using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using BTP.config;
using BTP.Slash;
using DSharpPlus.SlashCommands;

namespace BTP
{
    internal class Program
    {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }
        static async Task Main(string[] args)
        {
            var jsonReader = new jsonClass();
            await jsonReader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            Client = new DiscordClient(discordConfig);
            Client.Ready += Client_Ready;

            var slashCommands = Client.UseSlashCommands();

            slashCommands.RegisterCommands<Commands>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
