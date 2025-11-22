using GodotTemplate.Scripts.Content.LoadingScreen;
using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;

namespace GodotTemplate.Scenes.Root.Starters;

public class ClientRootStarter : BaseRootStarter
{

	[Logger] private ILogger _log; //TODO Тест, т.к. здесь нет DI
	
    public override void Init(Root root)
    {
	    base.Init(root);
        _log.Information("Initializing Client...");
        
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
        
        Service.PlayerSettings.Init();
        if (Service.CmdArgs.Client.Nick != null)
        {
	        Service.PlayerSettings.SetNickTemporarily(Service.CmdArgs.Client.Nick);
        }
    }

    public override void Start(Root root)
    {
	    base.Start(root);
        _log.Information("Starting Client...");
        
        if (Service.CmdArgs.Client.AutoConnect)
        {
	        Service.MainScene.ConnectToMultiplayerGame(Service.CmdArgs.Client.AutoConnectIp, Service.CmdArgs.Client.AutoConnectPort);
        }
        else
        {
	        Service.MainScene.StartMainMenu();
	        Service.LoadingScreen.Clear();
        }
    }
}