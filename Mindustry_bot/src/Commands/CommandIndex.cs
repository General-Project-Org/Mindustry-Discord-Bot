using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

namespace MindustryManagerBot;

internal class CommandIndex
{
    private static Commands cmdCode = new();

    /// <summary>
    /// Returns a tuple of lists that contain all the properties of all the available commands and their logic.
    /// </summary>
    /// <returns>Tuple of Lists containing the commands data and their logic.</returns>
    internal static (List<SlashCommandProperties>, List<Func<SocketSlashCommand, Task>>) GetCommands()
    {
        List<SlashCommandProperties> commands = new();
        List<Func<SocketSlashCommand, Task>> commandLogic = new();

        SlashCommandBuilder pingCommand = new()
        {
            Name = "ping",
            Description = "Get the ping from our bot to Discord"
        };
        commandLogic.Add(sock => cmdCode.Ping(sock));

        SlashCommandBuilder startServerCommand = new()
        {
            Name = "start",
            Description = "Start the Mindustry Server."
        };
        commandLogic.Add(sock => cmdCode.StartServer(sock));

        SlashCommandBuilder stopServerCommand = new()
        {
            Name = "stop",
            Description = "Start the Mindustry Server."
        };
        commandLogic.Add(sock => cmdCode.StopServer(sock));

        SlashCommandBuilder loadSaveCommand = new()
        {
            Name = "load",
            Description = "Load a save in the Mindustry Server."
        };
        loadSaveCommand.AddOption("savename", ApplicationCommandOptionType.String, "The name of the save file", true);
        commandLogic.Add(sock => cmdCode.LoadSave(sock));

        SlashCommandBuilder pauseCommand = new()
        {
            Name = "pause",
            Description = "Pause or resume the game depending of the value"
        };
        pauseCommand.AddOption("pause", ApplicationCommandOptionType.Boolean, "if true the server will be paused, else it will be resumed", true);
        commandLogic.Add(sock => cmdCode.PauseGame(sock));

        SlashCommandBuilder srvInfoCommand = new()
        {
            Name = "serverinfo",
            Description = "Show information about the Mindustry server"
        };
        commandLogic.Add(sock => cmdCode.ServerInformation(sock));

        SlashCommandBuilder executeCommand = new()
        {
            Name = "execute",
            Description = "Execute a command on the Mindustry server"
        };
        commandLogic.Add(sock => cmdCode.ExecuteInServer(sock));

        executeCommand.AddOption("command", ApplicationCommandOptionType.String, "The command to execute in the Mindustry server", true);
        commands.Add(pingCommand.Build());
        commands.Add(startServerCommand.Build());
        commands.Add(stopServerCommand.Build());
        commands.Add(loadSaveCommand.Build());
        commands.Add(pauseCommand.Build());
        commands.Add(srvInfoCommand.Build());
        commands.Add(executeCommand.Build());

        return (commands, commandLogic);
    }
}