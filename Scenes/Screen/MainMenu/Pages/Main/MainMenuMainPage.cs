using Godot;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.Screen.MainMenu.Pages.Main;

public partial class MainMenuMainPage : MainMenuPage
{
    
    [Export] [NotNull] public Button StartSingleplayerButton { get; private set; }
    [Export] [NotNull] public Button CreateServerButton { get; private set; }
    [Export] [NotNull] public Button ConnectToServerButton { get; private set; }
    [Export] [NotNull] public Button SettingsButton { get; private set; }
    
    public override void _Ready()
    {
        Di.Process(this);
        
        StartSingleplayerButton.Pressed += () => Services.MainScene.StartSingleplayerGame();
        CreateServerButton.Pressed += () => ChangeMenuPage(PackedScenes.CreateServer);
        ConnectToServerButton.Pressed += () => ChangeMenuPage(PackedScenes.ConnectToServer);
        SettingsButton.Pressed += () => ChangeMenuPage(PackedScenes.Settings);
    }
}