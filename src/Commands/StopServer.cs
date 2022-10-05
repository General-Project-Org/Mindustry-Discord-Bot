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
    internal async Task StopServer(SocketSlashCommand cmdSocket)
    {
        await cmdSocket.DeferAsync();
        EmbedBuilder embed;
        if (MainActivity.game_server.IsServerRunning())
        {
            MainActivity.game_server.StopServer();
            embed = new()
            {
                Title = "Server Stopped!",
                Footer = Utils.GetTimeFooter()
            };
        }
        else
        {
            embed = new()
            {
                Title = "The server has been ALREADY stopped!",
                Footer = Utils.GetTimeFooter()
            };
        }
        await cmdSocket.FollowupAsync(embed: embed.Build());
    }
}
