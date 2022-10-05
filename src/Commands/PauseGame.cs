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
    internal async Task PauseGame(SocketSlashCommand cmdSocket)
    {
        bool pause = (bool)cmdSocket.Data.Options.ElementAt(0).Value;
        string pauseText = pause ? "on " : "off";
        await cmdSocket.DeferAsync();
        EmbedBuilder embed;
        if (MainActivity.game_server.IsServerRunning())
        {
            await MainActivity.game_server.WriteToCommandLineAsync($"pause {pauseText}");
            MainActivity.game_server.SetGameLoaded(true);
            if (pause)
            {
                embed = new()
                {
                    Title = "Pausing Game!",
                    Footer = Utils.GetTimeFooter()
                };
            }
            else
            {
                embed = new()
                {
                    Title = "Resuming Game!",
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
