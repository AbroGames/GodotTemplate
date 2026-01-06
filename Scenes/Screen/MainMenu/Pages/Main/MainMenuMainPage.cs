using Godot;
using KludgeBox.DI.Requests.ChildInjection;

namespace GodotTemplate.Scenes.Screen.MainMenu.Pages.Main;

public partial class MainMenuMainPage : MainMenuPage
{
    
    [Child] public Button StartSingleplayerButton { get; private set; }
    [Child] public Button CreateServerButton { get; private set; }
    [Child] public Button ConnectToServerButton { get; private set; }
    [Child] public Button SettingsButton { get; private set; }
    
    public override void _Ready()
    {
        Di.Process(this);

        StartSingleplayerButton.Pressed += () => ChangeMenuPage(PackedScenes.StartSingleplayer);
        CreateServerButton.Pressed += () => ChangeMenuPage(PackedScenes.CreateServer);
        ConnectToServerButton.Pressed += () => ChangeMenuPage(PackedScenes.ConnectToServer);
        SettingsButton.Pressed += () => ChangeMenuPage(PackedScenes.Settings);
    }
}