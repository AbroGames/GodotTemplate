using GodotTemplate.Scripts.Services.Settings;
using Godot;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.Screen.MainMenu.Pages.Settings;

public partial class MainMenuSettingsPage : MainMenuPage
{
    
    [Export] [NotNull] public TextEdit NickTextEdit { get; private set; }
    [Export] [NotNull] public TextEdit ColorTextEdit { get; private set; }
    [Export] [NotNull] public Button SaveReturnButton { get; private set; }
    
    public override void _Ready()
    {
        Di.Process(this);

        PlayerSettings playerSettings = Service.PlayerSettings.GetPlayerSettings();
        NickTextEdit.Text = playerSettings.Nick;
        ColorTextEdit.Text = playerSettings.Color.ToHtml(false);

        SaveReturnButton.Pressed += ParseAndSaveSettings;
    }

    private void ParseAndSaveSettings()
    {
        string nick = NickTextEdit.Text;
        Color color = Color.FromHtml(ColorTextEdit.Text);
        
        Service.PlayerSettings.SetPlayerSettings(new PlayerSettings(nick, color));
        ChangeMenuPage(PackedScenes.Main);
    }
}