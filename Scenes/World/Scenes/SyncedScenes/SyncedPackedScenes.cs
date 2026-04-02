using Godot;
using GodotTemplate.Scenes.KludgeBox;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.World.Scenes.SyncedScenes;

public partial class SyncedPackedScenes : CheckedAbstractStorage
{
    
    [ExportGroup("Surfaces")]
    [Export] [NotNull] public PackedScene MapSurface { get; private set; }
    [Export] [NotNull] public PackedScene BattleSurface { get; private set; }
    
    [ExportGroup("Map")]
    [Export] [NotNull] public PackedScene MapPoint { get; private set; }
}