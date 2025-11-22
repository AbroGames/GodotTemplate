using GodotTemplate.Scripts.Services.CmdArgs;
using Godot;

namespace GodotTemplate.Scripts.Services.Process;

//TODO [Service]
public class ProcessService
{
    
    public int StartNewApplication(string[] arguments)
    {
        return OS.CreateInstance(arguments);
    }
    
    public int StartNewDedicatedServerApplication(int port, string saveFileName, string adminNickname, bool showWindow)
    {
        DedicatedServerArgs dedicatedServerArgs = new DedicatedServerArgs(!showWindow, port, saveFileName, adminNickname, OS.GetProcessId(), false);

        return StartNewApplication(dedicatedServerArgs.GetArrayToStartDedicatedServer());
    }
    
    public int StartNewClientApplicationAndAutoConnect(string ip, int port)
    {
        string[] clientParams = [ClientArgs.AutoConnectIpFlag, ip, ClientArgs.AutoConnectPortFlag, port.ToString()];
        return StartNewApplication(clientParams);
    }
}
