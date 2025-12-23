using GodotTemplate.Scripts.Content.LoadingScreen;
using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;

namespace GodotTemplate.Scenes.Root.Starters;

public class ClientRootStarter : BaseRootStarter
{

	[Logger] private ILogger _log;
	
    public override void Init(RootData rootData)
    {
	    base.Init(rootData);
        _log.Information("Initializing Client...");
        
        Services.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
        
        Services.PlayerSettings.Init();
        if (Services.CmdArgs.Client.Nick != null)
        {
	        Services.PlayerSettings.SetNickTemporarily(Services.CmdArgs.Client.Nick);
        }
    }

    public override void Start(RootData rootData)
    {
	    base.Start(rootData);
        _log.Information("Starting Client...");


        if (Services.CmdArgs.Client.AutoStart)
        {
	        Services.MainScene.StartSingleplayerGame();
        } 
        else if (Services.CmdArgs.Client.AutoConnect)
        {
	        Services.MainScene.ConnectToMultiplayerGame(Services.CmdArgs.Client.AutoConnectIp, Services.CmdArgs.Client.AutoConnectPort);
        }
        else
        {
	        Services.MainScene.StartMainMenu();
	        Services.LoadingScreen.Clear();
        }
    }
}