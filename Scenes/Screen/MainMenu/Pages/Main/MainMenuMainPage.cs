using Godot;
using GodotTemplate.Scripts.Service.ResumableGame;
using KludgeBox.DI.Requests.ChildInjection;

namespace GodotTemplate.Scenes.Screen.MainMenu.Pages.Main;

public partial class MainMenuMainPage : MainMenuPage
{
    
    [Child] public Button StartLastGameButton { get; private set; }
    [Child] public Button StartSingleplayerButton { get; private set; }
    [Child] public Button CreateServerButton { get; private set; }
    [Child] public Button ConnectToServerButton { get; private set; }
    [Child] public Button SettingsButton { get; private set; }
    [Child] public Button ExitButton { get; private set; }
    
    public override void _Ready()
    {
        Di.Process(this);

        if (Services.LastGame.GetLastGame().Type == ResumableGame.ResumableType.None)
        {
            StartLastGameButton.Visible = false;
        }
        
        StartLastGameButton.Pressed += () => Services.LastGame.StartLastGame();
        StartSingleplayerButton.Pressed += () => ChangeMenuPage(PackedScenes.StartSingleplayer);
        CreateServerButton.Pressed += () => ChangeMenuPage(PackedScenes.CreateServer);
        ConnectToServerButton.Pressed += () => ChangeMenuPage(PackedScenes.ConnectToServer);
        SettingsButton.Pressed += () => ChangeMenuPage(PackedScenes.Settings);
        ExitButton.Pressed += () => Services.MainScene.Shutdown();
    }
}