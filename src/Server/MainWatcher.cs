using System.Diagnostics;
using System.IO;
using System.Text;

namespace MindustryManagerBot;

public class MindustryManager
{
    public bool ServerEnabled { get; private set; }
    private StreamWriter? _STDIN;
    private Process? _MindustryServer;
    public void StartServer()
    {
        const string _SERVER_PATH = "/home/all/mindustry_server/server-release.jar";
        ProcessStartInfo mindustryInfo = new()
        {
            FileName = "java",
            Arguments = $"-jar {_SERVER_PATH}",
            WorkingDirectory = "/home/all/mindustry_server/",
            StandardInputEncoding = Encoding.UTF8,
            RedirectStandardInput = true,
        };
        _MindustryServer = Process.Start(mindustryInfo);
        _STDIN = _MindustryServer.StandardInput;
    }
    /// <summary>
    /// Returns if the server is currently running.
    /// </summary>
    /// <returns>a NULLABLE BOOL. If null the server has exited.</returns>
    public bool IsServerRunning()
    {
        bool? exited = _MindustryServer?.HasExited;
        if (exited is null) return false;
        else if (exited is true) return false;
        else return true;
    }
    public void SetGameLoaded(bool status) => ServerEnabled = status;
    public async Task WriteToCommandLineAsync(string text) => await _STDIN?.WriteLineAsync(text)!;
    public void WriteToCommandLine(string text) => _STDIN?.WriteLine(text);
    public void StopServer()
    {
        _STDIN?.WriteLine("stop".ReplaceLineEndings("\n"));
        _STDIN?.WriteLine("exit");
        ServerEnabled = false;
        _MindustryServer?.Dispose();
        _MindustryServer = null;
        _STDIN = null;
    }
}