using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using KludgeBox.DI.Requests.NotNullCheck;
using KludgeBox.Godot.Nodes.MpSync;
using BattleSurface = GodotTemplate.Scenes.World.Tree.Surface.Battle.BattleSurface;
using MapSurface = GodotTemplate.Scenes.World.Tree.Surface.Map.MapSurface;

namespace GodotTemplate.Scenes.World.Tree;

public partial class WorldTree : Node2D
{

    private World _world;
    
    [Export] [NotNull] public MapSurface MapSurface  { get; private set; }
    
    public List<BattleSurface> BattleSurfaces => _battleSurfacesNames.Select(name => GetNodeOrNull<BattleSurface>(name)).ToList();
    [Export] [Sync] private Array<string> _battleSurfacesNames = new();

    public void Init(World world)
    {
        _world = world;
    }

    public override void _Ready()
    {
        Di.Process(this);
        this.AddChildWithName(new AttributeMultiplayerSynchronizer(this), "MultiplayerSynchronizer");
    }
    
    public BattleSurface AddBattleSurface()
    {
        BattleSurface battleSurface = _world.PackedScenes.BattleSurface.Instantiate<BattleSurface>();
        this.AddChildWithUniqueName(battleSurface, "BattleSurface");
        _battleSurfacesNames.Add(battleSurface.Name);
        _world.MultiplayerSpawnerService.AddSpawnerToNode(battleSurface);
        return battleSurface;
    }
}