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
    internal async Task StartServer(SocketSlashCommand cmdSocket)
    {
        await cmdSocket.DeferAsync();
        EmbedBuilder embed;
        if (!MainActivity.game_server.IsServerRunning())
        {
            MainActivity.game_server.StartServer();
            MainActivity.game_server.SetGameLoaded(false);
            embed = new()
            {
                Title = "Server Started!",
                Footer = Utils.GetTimeFooter()
            };
        }
        else
        {
            embed = new()
            {
                Title = "The server has been ALREADY started!",
                Footer = Utils.GetTimeFooter()
            };
        }
        await cmdSocket.FollowupAsync(embed: embed.Build());
    }
}
