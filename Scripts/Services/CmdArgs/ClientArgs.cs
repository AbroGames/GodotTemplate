namespace GodotTemplate.Scripts.Services.CmdArgs;

public readonly record struct ClientArgs(bool AutoConnect, string AutoConnectIp, int? AutoConnectPort, string Nick)
{
    public static readonly string AutoConnectFlag = "--auto-connect";
    public static readonly string AutoConnectIpFlag = "--auto-connect-ip";
    public static readonly string AutoConnectPortFlag = "--auto-connect-port";
    public static readonly string NickFlag = "--nick";
    
    public static ClientArgs GetFromCmd(KludgeBox.Core.CmdArgsService  argsService)
    {
        return new ClientArgs(
            argsService.ContainsInCmdArgs(AutoConnectFlag),
            argsService.GetStringFromCmdArgs(AutoConnectIpFlag),
            argsService.GetIntFromCmdArgs(AutoConnectPortFlag),
            argsService.GetStringFromCmdArgs(NickFlag)
        );
    }
}
