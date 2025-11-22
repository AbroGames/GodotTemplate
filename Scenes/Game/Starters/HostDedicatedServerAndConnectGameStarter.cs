using KludgeBox.Godot.Nodes.Process;

namespace GodotTemplate.Scenes.Game.Starters;

public class HostDedicatedServerAndConnectGameStarter(int? port = null, string saveFileName = null, string adminNickname = null, bool? showWindow = null) : ConnectToMultiplayerGameStarter(Localhost, port)
{
    private readonly int? _port = port;

    public override void Init(Game game)
    {
        int dedicatedServerPid = Service.Process.StartNewDedicatedServerApplication(
            _port ?? DefaultPort, 
            saveFileName,
            adminNickname, 
            showWindow ?? true);
        
        ProcessShutdowner dedicatedServerShutdowner = new ProcessShutdowner(
            dedicatedServerPid,
            pid => $"Kill server process: {pid}."); 
        game.AddChild(dedicatedServerShutdowner); 
        
        base.Init(game); // Try to connect to new hosted server
    }
}