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
    internal async Task ServerInformation(SocketSlashCommand cmdSocket)
    {
        await cmdSocket.DeferAsync();

        string mindustryRTStatus = MainActivity.game_server.IsServerRunning() ? "Running" : "Not Running";
        string status = MainActivity.game_server.ServerEnabled ? "Running" : "Not Running";
        EmbedBuilder embed = new()
        {
            Title = "Server Information:",
            Description = $"Address -> 139.162.194.103\r\nPort -> 25405\r\nMindustry Server Runtime -> {mindustryRTStatus}\r\nServer Hosted/Game Loaded -> {status}",
            Footer = Utils.GetTimeFooter()
        };

        await cmdSocket.FollowupAsync(embed: embed.Build());
    }
}
