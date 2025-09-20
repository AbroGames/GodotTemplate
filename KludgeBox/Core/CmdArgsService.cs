using Godot;
using KludgeBox.DI.Requests.NotNullCheck;
using KludgeBox.Logging;
using Serilog;

namespace KludgeBox.Core;

public class CmdArgsService
{

    protected readonly string[] CmdArgs = OS.GetCmdlineArgs();
    private readonly ILogger _logger = LogFactory.GetForStatic<CmdArgsService>();

    public void LogCmdArgs()
    {
        if (!CmdArgs.IsEmpty())
        {
            _logger.Information("Cmd args: " + CmdArgs.Join());
        }
        else
        {
            _logger.Information("Cmd args is empty");
        }
    }

    public bool ContainsInCmdArgs(string paramName)
    {
        return CmdArgs.Contains(paramName);
    }
    
    public string GetStringFromCmdArgs(string paramName, string defaultValue = null, bool logIfEmpty = false)
    {
        string arg = defaultValue;
        try
        {
            int argPos = CmdArgs.ToList().IndexOf(paramName);
            if (argPos == -1)
            {
                if (logIfEmpty) _logger.Information($"Arg {paramName} not setup.");
                return arg;
            }

            arg = CmdArgs[argPos + 1];
        }
        catch
        {
            _logger.Warning($"Error while arg {paramName} setup."); //TODO Добавить возможность настроить сервис, чтобы ничего не логировалось (ни в сatch, ни просто) 
        }
        
        _logger.Information($"{paramName}: {arg}");
        return arg;
    }

    public int? GetIntFromCmdArgs(string paramName, bool logIfEmpty = false)
    {
        string argAsString = GetStringFromCmdArgs(paramName, null, logIfEmpty);
        int? arg = null;

        try
        {
            if (argAsString != null)
            {
                arg = Convert.ToInt32(argAsString);
            }
        }
        catch (Exception ex) when (ex is FormatException || ex is OverflowException)
        {
            _logger.Warning($"Arg {paramName} can't convert to Int32");
        }

        return arg;
    }
    
    public int GetIntFromCmdArgs(string paramName, int defaultValue, bool logIfEmpty = false)
    {
        string argAsString = GetStringFromCmdArgs(paramName, defaultValue.ToString(), logIfEmpty);
        int arg = defaultValue;

        try
        {
            if (argAsString != null)
            {
                arg = Convert.ToInt32(argAsString);
            }
        }
        catch (Exception ex) when (ex is FormatException || ex is OverflowException)
        {
            _logger.Warning($"Arg {paramName} can't convert to Int32");
        }

        return arg;
    }
}
