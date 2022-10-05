using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace MindustryManagerBot;

public struct CommandBuilder
{
    /// <summary>
    /// Build all bot commands for a specific Guild (Discord Server)
    /// </summary>
    /// <param name="guild">Socket of the Guild.</param>
    public async Task BuildFor(SocketGuild guild)
        => await guild.BulkOverwriteApplicationCommandAsync(this.BuildCommands().ToArray());
    /// <summary>
    /// Build all bot commands for the whole bot | Takes two hours to apply between iterations.
    /// </summary>
    /// <param name="client">Bot Client.</param>
    public async Task BuildApp(DiscordSocketClient client)
        => await client.BulkOverwriteGlobalApplicationCommandsAsync(BuildCommands().ToArray());

    private List<SlashCommandProperties> BuildCommands()
    {
        List<SlashCommandProperties> commands = new();

        SlashCommandBuilder pingCommand = new()
        {
            Name = "ping",
            Description = "Get the ping from our bot to Discord"
        };
        SlashCommandBuilder startServerCommand = new()
        {
            Name = "start",
            Description = "Start the Mindustry Server."
        };
        SlashCommandBuilder stopServerCommand = new()
        {
            Name = "stop",
            Description = "Start the Mindustry Server."
        };
        SlashCommandBuilder loadSaveCommand = new()
        {
            Name = "load",
            Description = "Load a save in the Mindustry Server."
        };
        loadSaveCommand.AddOption("savename", ApplicationCommandOptionType.String, "The name of the save file", true);
        SlashCommandBuilder pauseCommand = new()
        {
            Name = "pause",
            Description = "Pause or resume the game depending of the value"
        };
        pauseCommand.AddOption("pause", ApplicationCommandOptionType.Boolean, "if true the server will be paused, else it will be resumed", true);

        SlashCommandBuilder srvInfoCommand = new()
        {
            Name = "serverinfo",
            Description = "Show information about the Mindustry server"
        };

        SlashCommandBuilder executeCommand = new()
        {
            Name = "execute",
            Description = "Execute a command on the Mindustry server"
        };
        executeCommand.AddOption("command", ApplicationCommandOptionType.String, "The command to execute in the Mindustry server", true);
        commands.Add(pingCommand.Build());
        commands.Add(startServerCommand.Build());
        commands.Add(stopServerCommand.Build());
        commands.Add(loadSaveCommand.Build());
        commands.Add(pauseCommand.Build());
        commands.Add(srvInfoCommand.Build());
        commands.Add(executeCommand.Build());




        return commands;
    }
}
