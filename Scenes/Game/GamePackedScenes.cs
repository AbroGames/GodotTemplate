using Godot;
using GodotTemplate.Scenes.KludgeBox;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.Game;

public partial class GamePackedScenes : CheckedAbstractStorage
{
    
    [Export] [NotNull] public PackedScene World { get; private set; }
    [Export] [NotNull] public PackedScene Hud { get; private set; }
    [Export] [NotNull] public PackedScene ServerHud { get; private set; }
}