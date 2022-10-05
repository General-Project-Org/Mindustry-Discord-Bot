using System;
using System.Net;
using System.Security.Authentication;

using Discord;
using Discord.WebSocket;

using Spectre.Console;

namespace MindustryManagerBot;

public static class MainActivity
{
    public static MindustryManager game_server = new();
    private static string Token { get; } = Environment.GetEnvironmentVariable("managertoken")!;

    public static DiscordSocketClient BotClient { get; } = new(new()
    {
        GatewayIntents = GatewayIntents.Guilds,
        LogLevel = LogSeverity.Debug,
        LogGatewayIntentWarnings = true,
        AlwaysDownloadUsers = true
    });

    internal static readonly HttpClient client = new(
    new HttpClientHandler()
    {
        SslProtocols = SslProtocols.Tls12,
        AutomaticDecompression = DecompressionMethods.All,
        UseCookies = false,
        UseProxy = false,
        AllowAutoRedirect = true
    });

    private static Masked.DiscordNet.CommandHelper _cmdHelper = new();

    public static async Task Main()
    {
        (List<SlashCommandProperties> commands, List<Func<SocketSlashCommand, Task>> commandsCode) = CommandIndex.GetCommands();
        for (int i = 0; i < commands.Count; i++)
        {
            _cmdHelper.AddToCommandList(commands[i], commandsCode[i]);
        }

        Thread cmdListener = new(async () => await ListenForCommands())
        {
            Name = "Console Listener",
            IsBackground = true
        };
        cmdListener.Start();
        Handlers handlerMain = new(new());

        if (Token is null)
        {
            AnsiConsole.MarkupLine("[[FATAL]] Can not log-in when the token does not exist!");
            Environment.Exit(-1);
        }

        BotClient.Log += (x) =>
        {
            AnsiConsole.MarkupLine("[cyan][[Discord.NET]] {0}[/]", x.ToString().RemoveMarkup());
            return Task.CompletedTask;
        };
        BotClient.SlashCommandExecuted += _cmdHelper.GetSlashCommandHandler();

        // Log into the bot.
        await BotClient.LoginAsync(TokenType.Bot, Token, true);
        await BotClient.StartAsync();
        await Task.Delay(-1); // Don't close.
    }

    private static async Task ListenForCommands()
    {
        const ulong mindustryid = 1026842718984491068;
        while (true)
        {
            // Loop listening to console.
            switch (Console.ReadLine()!)
            {
                case "DEV_commandtobuildcommands":
                    AnsiConsole.MarkupLine("[[INFO]] Obtaining Guild...");
                    SocketGuild guild = BotClient.GetGuild(mindustryid);
                    AnsiConsole.MarkupLine("[[INFO]] Guild Obtained.");

                    AnsiConsole.MarkupLine("[[INFO]] Attempting to submit the Remote Command Builder command...");
                    Task tsk =  _cmdHelper.SubmitCommandBuilder(guild);
                    await tsk;
                    if (tsk.IsCompletedSuccessfully)
                        AnsiConsole.MarkupLine("[[INFO]] Command Submitted Successfully.");
                    if (tsk.IsFaulted)
                        AnsiConsole.MarkupLine($"[[INFO]] Command Submit Failed, Further Error Information: {tsk.Exception}");

                    break;

                case "guildslashcmds":
                    _ = Task.Run(async () =>
                        {
                            AnsiConsole.MarkupLine("[[INFO]] Building Guild Level Commands, This might take a while!");
                            try
                            {
                                await _cmdHelper.BuildFor(BotClient.GetGuild(mindustryid));
                                AnsiConsole.MarkupLine($"[[INFO]] Commands Built for {BotClient.GetGuild(mindustryid).Name}.");
                            }
                            catch (Exception ex)
                            {
                                AnsiConsole.MarkupLine("[[FATAL]] Commands Failed to build with exception -> \r\n\r\n {0}", ex.ToString().RemoveMarkup());
                            }
                        }
                    );
                    break;

                case "appslashcmds":
                    _ = Task.Run(async () =>
                    {
                        AnsiConsole.MarkupLine("[[INFO]] Building Application Level Commands, This might take a while!");
                        try
                        {
                            await _cmdHelper.BuildApp(BotClient);
                            AnsiConsole.MarkupLine("[[INFO]] Commands Built at an Applcation level.");
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.MarkupLine("[[FATAL]] Commands Failed to build with exception -> \r\n\r\n {0}", ex.ToString().RemoveMarkup());
                        }
                    });
                    break;

                case "exit":
                    AnsiConsole.MarkupLine("[yellow bold][[INFO]] Exiting Mindustry Game Manager Bot...[/]");
                    await BotClient.LogoutAsync(); // Log out of Discord.
                    await BotClient.StopAsync(); // Stop the code of the bot
                    BotClient.Dispose(); // Dispose of the bot client.
                    client.Dispose(); // Dispose of the HTTPClient.
                    Environment.Exit(0);
                    break;

                default:
                    AnsiConsole.MarkupLine("[red bold][[WARN]] Invalid Command.[/]\n" +
                        "\tAvailable Commands:\n" +
                        "\t [green]exit[/] -> Exits the Bot, and correctly logs out.\n" +
                        "\t [green]guildslashcmds[/] -> Builds the commands for the bot, this mode builds the commands for the server that is [bold yellow]HARDCODED[/] into the Main.cs source file.\n" +
                        "\t [yellow]appslashcmds[/] -> Builds the commands for the bot, this mode allows for [red bold]ANY[/] server in which the bot is in to access them.\n" +
                        "\t [red]DEV_commandtobuildcommands[/] -> Builds a command that allows ANY member of the server the bot is in to build the commands of the bot remotely.");

                    break;
            }
        }
    }
}