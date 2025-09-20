using System;
using System.Linq;
using Godot;

namespace KludgeBox;

public class CmdArgsService
{
    public bool ContainsInCmdArgs(string paramName)
    {
        return OS.GetCmdlineArgs().Contains(paramName);
    }
    
    public string GetStringFromCmdArgs(string paramName, string defaultValue = null)
    {
        string arg = defaultValue;
        try
        {
            int argPos = OS.GetCmdlineArgs().ToList().IndexOf(paramName);
            if (argPos == -1)
            {
                return arg;
            }

            arg = OS.GetCmdlineArgs()[argPos + 1];
        }
        catch
        {
            // do nothing
        }
        
        return arg;
    }

    public int? GetIntFromCmdArgs(string paramName)
    {
        string argAsString = GetStringFromCmdArgs(paramName);
        int? arg = null;

        try
        {
            if (argAsString != null)
            {
                arg = Convert.ToInt32(argAsString);
            }
        }
        catch 
        {
            // do nothing
        }

        return arg;
    }
    
    public int GetIntFromCmdArgs(string paramName, int defaultValue)
    {
        string argAsString = GetStringFromCmdArgs(paramName, defaultValue.ToString());
        int arg = defaultValue;

        try
        {
            if (argAsString != null)
            {
                arg = Convert.ToInt32(argAsString);
            }
        }
        catch
        {
            // do nothing
        }

        return arg;
    }
}