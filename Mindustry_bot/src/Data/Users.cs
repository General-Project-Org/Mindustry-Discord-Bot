using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace MindustryManagerBot;

/// <summary>
/// A Structure containing data of a user in a guild.
/// </summary>
public struct GuildUserData
{
    /// <summary>
    /// The day of creation of the account.
    /// </summary>
    public DateTimeOffset CreationDate { get; }
    /// <summary>
    /// The user's socket in a Guild context.
    /// </summary>
    public SocketGuildUser UserGuildSocket { get; }
    /// <summary>
    /// The user's UserName in the Guild.
    /// </summary>
    public string UserName { get; }
    /// <summary>
    /// The ID of the user.
    /// </summary>
    public ulong UserId { get; }
    /// <summary>
    /// The SocketGuild that the users participate in.
    /// </summary>
    public SocketGuild UserGuild { get; }
    /// <summary>
    /// All the roles of the user in the Guild. <see cref="UserGuild"/>
    /// </summary>
    public SocketRole[] UserRoles { get; }
    /// <summary>
    /// Is the user an administrator at the Guild he is in?
    /// </summary>
    public bool IsAdministrator { get; }
    public GuildUserData(SocketGuildUser UserGuildSocket)
    {
        this.UserGuildSocket = UserGuildSocket;

        UserName = UserGuildSocket.Username;
        UserId = UserGuildSocket.Id;
        UserGuild = UserGuildSocket.Guild;
        UserRoles = UserGuildSocket.Roles.ToArray();
        CreationDate = UserGuildSocket.CreatedAt;

        IsAdministrator = this.UserGuildSocket.GuildPermissions.Administrator;
    }
}