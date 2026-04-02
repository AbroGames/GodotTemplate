using Godot;
using GodotTemplate.Scenes.KludgeBox;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.Root;

public partial class RootPackedScenes : CheckedAbstractStorage
{
    
    [Export] [NotNull] public PackedScene Game { get; private set; }
    [Export] [NotNull] public PackedScene MainMenu { get; private set; }
    [Export] [NotNull] public PackedScene LoadingScreen { get; private set; }
}