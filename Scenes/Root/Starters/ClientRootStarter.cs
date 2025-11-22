using GodotTemplate.Scripts.Content.LoadingScreen;

namespace GodotTemplate.Scenes.Root.Starters;

public class ClientRootStarter : BaseRootStarter
{

    public override void Init(Root root)
    {
	    base.Init(root);
        Log.Info("Initializing Client...");
        
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
        Log.Info("Starting Client...");
        
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