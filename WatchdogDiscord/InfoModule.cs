using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Sockets;

namespace WatchdogDiscord
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Echoes back a message")]
        public async Task SayAsync()
            => await ReplyAsync("pong");
    }
}
