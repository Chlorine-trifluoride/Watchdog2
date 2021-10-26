using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WatchdogDiscord
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandService commandService;
        private LogService logService;
        private CommandHandler commandHandler;

        public async Task MainAsync()
        {
            try
            {
                client = new DiscordSocketClient();
                commandService = new CommandService();
                logService = new LogService(client, commandService);
                commandHandler = new CommandHandler(client, commandService);
                await commandHandler.InstallCommandsAsync();

                client.Ready += Client_Ready;

                await client.LoginAsync(TokenType.Bot, BotTokenHelper.Token);
                await client.StartAsync();

                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                Log.Error("Discord", e.Message);
            }
        }

        private async Task Client_Ready()
        {
            await new LogTailer(client).Run();
        }
    }
}
