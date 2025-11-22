using KludgeBox.Godot.Nodes.Process;
using GodotTemplate.Scenes.Game.Net;
using GodotTemplate.Scripts.Content.LoadingScreen;
using GodotTemplate.Scripts.Services.Settings;
using Godot;

namespace GodotTemplate.Scenes.Game.Starters;

public class HostMultiplayerGameStarter(int? port = null, string saveFileName = null, string adminNickname = null, int? parentPid = null) : BaseGameStarter
{
    public override void Init(Game game)
    {
        base.Init(game);
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);

        if (parentPid.HasValue)
        {
            ProcessDeadChecker clientDeadChecker = new ProcessDeadChecker(
                parentPid.Value, 
                () => Service.MainScene.Shutdown(),
                pid => $"Parent process {pid} is dead. Shutdown server.");
            game.AddChild(clientDeadChecker);
        }
        
        PlayerSettings playerSettings = Service.PlayerSettings.GetPlayerSettings();
        World.World world = game.AddWorld();
        Synchronizer synchronizer = game.AddSynchronizer(playerSettings);
        game.DoClient(() => game.AddHud());
        Network network = game.AddNetwork();
        
        Error error = network.HostServer(port ?? DefaultPort, true);
        if (error != Error.Ok)
        {
            game.DoClient(HostFailedEventOnClient);
            return;
        }

        if (saveFileName == null)
        {
            world.StartStopService.StartNewGame(adminNickname);
        }
        else
        {
            world.StartStopService.LoadGame(saveFileName, adminNickname);
        }
        network.OpenServer();
        game.DoClient(synchronizer.StartSyncOnClient);
    }
    
    private void HostFailedEventOnClient()
    {
        Service.MainScene.StartMainMenu();
        //TODO Show error in menu (it is client). Log already has error.
        Service.LoadingScreen.Clear();
    }
}