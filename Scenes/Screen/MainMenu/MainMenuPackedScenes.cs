using Godot;
using GodotTemplate.Scenes.KludgeBox;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.Screen.MainMenu;

public partial class MainMenuPackedScenes : CheckedAbstractStorage
{
    
    [Export] [NotNull] public PackedScene Main { get; private set; }
    [Export] [NotNull] public PackedScene StartSingleplayer { get; private set; }
    [Export] [NotNull] public PackedScene CreateServer { get; private set; }
    [Export] [NotNull] public PackedScene ConnectToServer { get; private set; }
    [Export] [NotNull] public PackedScene Settings { get; private set; }
    [Export] [NotNull] public PackedScene Message { get; private set; }
    
}