using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using GodotTemplate.Scenes.World.SyncedScenes;
using KludgeBox.DI.Requests.ChildInjection;
using KludgeBox.DI.Requests.SceneServiceInjection;
using KludgeBox.Godot.Nodes.MpSync;
using BattleSurface = GodotTemplate.Scenes.World.Tree.Surface.Battle.BattleSurface;
using MapSurface = GodotTemplate.Scenes.World.Tree.Surface.Map.MapSurface;

namespace GodotTemplate.Scenes.World.Tree;

public partial class WorldTree : Node2D
{

    [Child] public MapSurface MapSurface  { get; private set; }
    
    public List<BattleSurface> BattleSurfaces => _battleSurfacesNames.Select(name => GetNodeOrNull<BattleSurface>(name)).ToList();
    [Export] [Sync] private Array<string> _battleSurfacesNames = new();
    
    [SceneService] private SyncedPackedScenes _worldPackedScenes;
    [SceneService] private WorldMultiplayerSpawnerService _worldMultiplayerSpawner;

    public override void _Ready()
    {
        Di.Process(this);
    }
    
    public BattleSurface AddBattleSurface()
    {
        BattleSurface battleSurface = _worldPackedScenes.BattleSurface.Instantiate<BattleSurface>();
        this.AddChildWithUniqueName(battleSurface, "BattleSurface");
        _battleSurfacesNames.Add(battleSurface.Name);
        _worldMultiplayerSpawner.AddSpawnerToNode(battleSurface);
        return battleSurface;
    }
}