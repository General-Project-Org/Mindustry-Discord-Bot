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

    public static async Task Main()
    {
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
        BotClient.SlashCommandExecuted += handlerMain.HandleSlashCommand;

        // Log into the bot.
        await BotClient.LoginAsync(TokenType.Bot, Token, true);
        await BotClient.StartAsync();
        await Task.Delay(-1); // Don't close.
    }
    private static async Task ListenForCommands()
    {
        CommandBuilder builder = new();
        while (true)
        {
            // Loop listening to console.
            switch (Console.ReadLine()!)
            {
                case "guildslashcmds":
                    const ulong mindustryid = 1026842718984491068;
                    new Thread(
                        async () =>
                        {
                            AnsiConsole.MarkupLine("[[INFO]] Building Guild Level Commands, This might take a while!");
                            try
                            {
                                await builder.BuildFor(BotClient.GetGuild(mindustryid));
                                AnsiConsole.MarkupLine($"[[INFO]] Commands Built for {BotClient.GetGuild(mindustryid).Name}.");
                            }
                            catch (Exception ex)
                            {
                                AnsiConsole.MarkupLine("[[FATAL]] Commands Failed to build with exception -> \r\n\r\n {0}", ex.ToString().RemoveMarkup());
                            }
                        }
                    ).Start();
                    break;
                case "appslashcmds":
                    new Thread(
                        async () =>
                        {
                            AnsiConsole.MarkupLine("[[INFO]] Building Application Level Commands, This might take a while!");
                            try
                            {
                                await builder.BuildApp(BotClient);
                                AnsiConsole.MarkupLine("[[INFO]] Commands Built at an Applcation level.");
                            }
                            catch (Exception ex)
                            {
                                AnsiConsole.MarkupLine("[[FATAL]] Commands Failed to build with exception -> \r\n\r\n {0}", ex.ToString().RemoveMarkup());
                            }
                        }
                    ).Start();
                    break;
                case "exit":
                    AnsiConsole.MarkupLine("[yellow bold][[INFO]] Exiting Mindustry Game Manager Bot...[/]");
                    await BotClient.LogoutAsync(); // Log out of Discord.
                    await BotClient.StopAsync(); // Stop the code of the bot
                    BotClient.Dispose(); // Dispose bot client.
                    client.Dispose(); // Dispose HTTPClient.
                    Environment.Exit(0);
                    break;
                default:
                    AnsiConsole.MarkupLine("[red bold][[WARN]] Invalid Command.[/]");
                    break;
            }
        }
    }
}