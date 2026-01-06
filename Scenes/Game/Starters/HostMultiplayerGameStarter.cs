using Godot;
using GodotTemplate.Scripts.Content.LoadingScreen;
using GodotTemplate.Scripts.Service.Settings;
using Humanizer;
using KludgeBox.Godot.Nodes.Process;

namespace GodotTemplate.Scenes.Game.Starters;

public class HostMultiplayerGameStarter(int? port = null, string saveFileName = null, string adminNickname = null, int? parentPid = null) : BaseGameStarter
{
    
    private const string HostingFailedMessage = "Failed to start server: {0}";
    
    public override void Init(Game game)
    {
        base.Init(game);
        Services.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);

        if (parentPid.HasValue)
        {
            ProcessDeadChecker clientDeadChecker = new ProcessDeadChecker(
                parentPid.Value, 
                () => Services.MainScene.Shutdown(),
                pid => $"Parent process {pid} is dead. Shutdown server.");
            game.AddChild(clientDeadChecker);
        }
        
        PlayerSettings playerSettings = Services.PlayerSettings.GetPlayerSettings();
        World.World world = game.AddWorld();
        Synchronizer synchronizer = game.AddSynchronizer(playerSettings);
        Net.DoClient(() => game.AddHud());
        Network.Network network = game.AddNetwork();
        
        Error error = network.HostServer(port ?? DefaultPort, true);
        if (error != Error.Ok)
        {
            Net.DoClient(() => HostingFailedEventOnClient(error));
            return;
        }

        StartWorld(world, saveFileName, adminNickname);
        network.OpenServer();
        Net.DoClient(synchronizer.StartSyncOnClient);
    }

    private void HostingFailedEventOnClient(Error error) => GoToMenuAndShowError(HostingFailedMessage.FormatWith(error));
}