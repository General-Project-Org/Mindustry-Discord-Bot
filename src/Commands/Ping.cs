using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace MindustryManagerBot;

internal partial class Commands
{
    internal async Task Ping(SocketSlashCommand cmdSocket)
    {
        await cmdSocket.DeferAsync();
        EmbedBuilder embed = new()
        {
            Title = "Pong!",
            Description = new StringBuilder().Append("Bot Latency: ").Append(MainActivity.BotClient.Latency).Append("ms").ToString(),
            Footer = new()
            {
                Text = string.Format("{0} | BOT UTC TIME!", DateTime.UtcNow)
            }
        };
        await cmdSocket.FollowupAsync(embed: embed.Build());
    }
}
