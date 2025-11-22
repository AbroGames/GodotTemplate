using Godot;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.Root;

public partial class RootPackedScenes : Node
{
    
    [Export] [NotNull] public PackedScene Game { get; private set; }
    [Export] [NotNull] public PackedScene MainMenu { get; private set; }
    [Export] [NotNull] public PackedScene LoadingScreen { get; private set; }

    public void Init()
    {
        Di.Process(this); // We call NotNullChecker here, because it has not been created in _Ready
    }
}