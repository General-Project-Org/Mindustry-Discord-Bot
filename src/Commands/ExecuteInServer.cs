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
    internal async Task ExecuteInServer(SocketSlashCommand cmdSocket)
    {
        string cmd = (string)cmdSocket.Data.Options.ElementAt(0).Value;
        await cmdSocket.DeferAsync();
        EmbedBuilder embed;
        if (MainActivity.game_server.IsServerRunning())
        {
            if (!MainActivity.game_server.ServerEnabled)
            {
                embed = new()
                {
                    Title = "The server runtime is running, but there is no game available to run the server command!",
                    Footer = Utils.GetTimeFooter()
                };
            }
            else
            {
                await MainActivity.game_server.WriteToCommandLineAsync(cmd);
                embed = new()
                {
                    Title = $"Running server command -> {cmd}!",
                    Footer = Utils.GetTimeFooter()
                };
            }
        }
        else
        {
            embed = new()
            {
                Title = "There is no server instance, please start one with /start",
                Footer = Utils.GetTimeFooter()
            };
        }
        await cmdSocket.FollowupAsync(embed: embed.Build());
    }
}
