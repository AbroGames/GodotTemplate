using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Godot;
using KludgeBox.Godot.Nodes;

namespace GodotTemplate.Scenes.World.MpSpawn;

public partial class WorldMultiplayerSpawner : AbstractMultiplayerSpawner
{
    [Export] [NotNull] public PackedScenes.WorldPackedScenes PackedScenes { get; set; }
    [Export] private bool _selfSync = true;
    
    public override IReadOnlyList<PackedScene> GetPackedScenesForSpawn()
    {
        return PackedScenes.GetScenesList();
    }

    public override bool GetSelfSync()
    {
        return _selfSync;
    }

    public override void SetSelfSync(bool selfSync)
    {
        _selfSync = selfSync;
    }
}