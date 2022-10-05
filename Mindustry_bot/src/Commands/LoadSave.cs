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
    internal async Task LoadSave(SocketSlashCommand cmdSocket)
    {
        string savename = (string)cmdSocket.Data.Options.ElementAt(0).Value;
        await cmdSocket.DeferAsync();
        EmbedBuilder embed;
        if (MainActivity.game_server.IsServerRunning())
        {
            await MainActivity.game_server.WriteToCommandLineAsync("stop");
            await MainActivity.game_server.WriteToCommandLineAsync($"load {savename}");
            MainActivity.game_server.SetGameLoaded(true);
            embed = new()
            {
                Title = $"Attempting to load save {savename}!",
                Footer = Utils.GetTimeFooter()
            };
        }
        else
        {
            MainActivity.game_server.SetGameLoaded(false);
            embed = new()
            {
                Title = "There is no server instance, please start one with /start",
                Footer = Utils.GetTimeFooter()
            };
        }
        await cmdSocket.FollowupAsync(embed: embed.Build());
    }
}
