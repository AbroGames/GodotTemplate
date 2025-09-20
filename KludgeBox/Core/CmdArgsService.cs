using Godot;

namespace KludgeBox.Core;

public class CmdArgsService
{

    protected string[] CmdArgs { get; } = OS.GetCmdlineArgs();

    public void LogCmdArgs()
    {
        if (!CmdArgs.IsEmpty())
        {
            Log.Info("Cmd args: " + CmdArgs.Join());
        }
        else
        {
            Log.Info("Cmd args is empty");
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
                if (logIfEmpty) Log.Info($"Arg {paramName} not setup.");
                return arg;
            }

            arg = CmdArgs[argPos + 1];
        }
        catch
        {
            Log.Warning($"Error while arg {paramName} setup."); //TODO Добавить возможность настроить сервис, чтобы ничего не логировалось (ни в сatch, ни просто) 
        }
        
        Log.Info($"{paramName}: {arg}");
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
            Log.Warning($"Arg {paramName} can't convert to Int32");
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
            Log.Warning($"Arg {paramName} can't convert to Int32");
        }

        return arg;
    }
}
