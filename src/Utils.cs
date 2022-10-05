using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace MindustryManagerBot;

public static class Utils
{
    public static SocketGuild GetGuild<T>(T channelSocket) where T : ISocketMessageChannel
    {
        try
        {
#pragma warning disable CS8600

            if (channelSocket is not SocketGuildChannel gChan)
            {
                throw new InvalidOperationException("Channel does not point to a valid guild!");
            }

            return gChan.Guild;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An exception occured while attempting to get the guild of channel with id -> {channelSocket.Id}. \r\n\r\nException -> {ex}");
            throw;
        }
    }
    public static SocketGuildUser GetGuildUser<T>(T userSocket) where T : IUser
        => (userSocket as SocketGuildUser)!;
    public static async Task<List<String>> GetRoleMentions(SocketRole[] roles)
    {
        List<String> names = new();
        await Task.Run(
        () =>
        {
            for (int i = 0; i < roles.Length; i++)
            {
                names.Add(roles[i].Mention);
            }
        });
        return names;
    }
    public static Task<bool> HasRole(SocketGuildUser user, SocketRole role)
    {
        return Task.Run(() =>
        {
            for (int i = 0; i < user.Roles.Count; i++)
            {
                if (user.Roles.ElementAt(i).Id == role.Id)
                    return true;
            }
            return false;
        });
    }
    /// <summary>
    /// Deletes a message in an asynchrownous manner
    /// </summary>
    /// <param name="msg">The message socket</param>
    /// <param name="delay">The delay in milliseconds in which the message should be deleted.</param>
    /// <returns>a Task representing the operation.</returns>
    internal static Task MessageDeleter<T>(T msg, int delay = 5000) where T : IMessage =>
        Task.Run(async () =>
        {
            await Task.Delay(delay);
            await msg.DeleteAsync();
        });
    internal static EmbedFooterBuilder GetTimeFooter() => new() { Text = string.Format("Bot Time. | {0}", DateTime.UtcNow) };

    private static readonly Dictionary<string, string> urlText = new()
    {
        ["%3A"] = ":",
        ["%2F"] = "/",
        ["%3F"] = "?",
        ["%2D"] = "-",
        ["%40"] = "@",
        ["%3D"] = "=",
        ["%2B"] = "+"
    };
    private static readonly Dictionary<string, string> fixUpChars = new()
    {
        ["&#xE1;"] = "á",
        ["&#xE9;"] = "é",
        ["&#xF3;"] = "ó",
        ["&#x2013;"] = "–",
        ["&#x2014;"] = "—",
        ["&quot;"] = "\"",
        ["&#xFA;"] = "ú",
        ["&#xED;"] = "í",
        ["&#xF1;"] = "ñ",
        ["&#xBF;"] = "¿",
        ["&#xA1;"] = "¡",
        ["&apos;"] = "\'",
        ["&amp;"] = "&",
        ["&#xA0;"] = " ",
        ["&lt;"] = "<",
        ["&gt;"] = ">",
        ["&copy;"] = "©",
        ["&reg;"] = "®",
        ["&euro;"] = "€",
        ["&yen;"] = "¥",
        ["&pound;"] = "£",
        ["&cent;"] = "¢"
    };
    public static void FixURL(ref string dirty, bool reverse)
    {
        if (!reverse)
        {
            for (int i = 0; i < urlText.Count; i++)
            {
                dirty = dirty.Replace(urlText.ElementAt(i).Key, urlText.ElementAt(i).Value);
            }
        }
        else
        {
            for (int i = 0; i < urlText.Count; i++)
            {
                dirty = dirty.Replace(urlText.ElementAt(i).Value, urlText.ElementAt(i).Key);
            }
        }
    }
    public static void FixString(ref string dirty, bool reverse)
    {
        if (!reverse)
        {
            for (int i = 0; i < fixUpChars.Count; i++)
            {
                dirty = dirty.Replace(fixUpChars.ElementAt(i).Key, fixUpChars.ElementAt(i).Value);
            }
        }
        else
        {
            for (int i = 0; i < fixUpChars.Count; i++)
            {
                dirty = dirty.Replace(fixUpChars.ElementAt(i).Value, fixUpChars.ElementAt(i).Key);
            }
        }
    }
}