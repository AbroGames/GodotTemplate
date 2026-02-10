using GodotTemplate.Scripts.Content.LoadingScreen;
using GodotTemplate.Scripts.Service.Settings;

namespace GodotTemplate.Scenes.Game.Starters;

public class SingleplayerGameStarter(string saveFileName = null) : BaseGameStarter
{
    
    public override void Init(Game game)
    {
        base.Init(game);
        Services.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);

        PlayerSettings playerSettings = Services.PlayerSettings.GetPlayerSettings();
        World.World world = game.AddWorld();
        game.AddHud();
        ConnectToClientSynchronizerEvents(world.SynchronizerService);
        
        StartWorld(world, saveFileName, playerSettings.Nick);
        StartSyncOnClient(world.SynchronizerService);
    }
}