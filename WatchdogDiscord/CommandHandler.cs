using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace WatchdogDiscord
{
    class CommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commandService;

        public CommandHandler(DiscordSocketClient client, CommandService commandService)
        {
            this.client = client;
            this.commandService = commandService;
        }

        public async Task InstallCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            var modules = await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            
            foreach (var module in modules)
            {
                Console.WriteLine(module.Name);
            }
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            if (msg == null) return;

            int argPos = 0;

            if (msg.HasCharPrefix('!', ref argPos))
            {
                var context = new SocketCommandContext(client, msg);
                await commandService.ExecuteAsync(context, argPos, null);
            }
        }
    }
}
