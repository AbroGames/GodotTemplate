using GodotTemplate.Scripts.Content.LoadingScreen;
using GodotTemplate.Scripts.Service.Settings;

namespace GodotTemplate.Scenes.Game.Starters;

public class SingleplayerGameStarter(string saveFileName = null) : BaseGameStarter
{
    
    public override void Init(Game game)
    {
        Services.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);

        GameSettings gameSettings = Services.GameSettings.GetSettings();
        World.World world = game.AddWorld();
        game.AddHud();
        
        var lastGame = ResumableGame.GetSingleplayer(saveFileName);
        SetLastGame(lastGame);
        AddLastGameUpdaterToSaveEvent(world, lastGame);
        
        ServerStartWorld(world, saveFileName, gameSettings.PlayerNick);
        ClientStartWorld(world);
    }
}