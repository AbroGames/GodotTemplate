using GodotTemplate.Scripts.Content.LoadingScreen;
using GodotTemplate.Scripts.Services.Settings;

namespace GodotTemplate.Scenes.Game.Starters;

public class SingleplayerGameStarter : BaseGameStarter
{
    
    public override void Init(Game game)
    {
        base.Init(game);
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);

        PlayerSettings playerSettings = Service.PlayerSettings.GetPlayerSettings();
        World.World world = game.AddWorld();
        Synchronizer synchronizer = game.AddSynchronizer(playerSettings);
        game.AddHud();
        
        world.StartStopService.StartNewGame(playerSettings.Nick);
        synchronizer.StartSyncOnClient();
    }
}