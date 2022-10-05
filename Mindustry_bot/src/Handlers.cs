using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Spectre.Console;

namespace MindustryManagerBot;

internal class Handlers
{
    private readonly Commands _commandsInstance;
    internal async Task HandleSlashCommand(SocketSlashCommand socket)
    {
        AnsiConsole.MarkupLine("[[INFO]] Command with name -> [red]{0}[/] Executed!", socket.Data.Name);
        switch (socket.Data.Name)
        {
            case "ping":
                await _commandsInstance.Ping(socket);
                break;
            case "start":
                await _commandsInstance.StartServer(socket);
                break;
            case "stop":
                await _commandsInstance.StopServer(socket);
                break;
            case "load":
                await _commandsInstance.LoadSave(socket);
                break;
            case "pause":
                await _commandsInstance.PauseGame(socket);
                break;
            case "serverinfo":
                await _commandsInstance.ServerInformation(socket);
                break;
            case "execute":
                await _commandsInstance.ExecuteInServer(socket);
                break;
        }
    }
    internal Handlers(Commands commands)
    {
        _commandsInstance = commands;
    }
}
