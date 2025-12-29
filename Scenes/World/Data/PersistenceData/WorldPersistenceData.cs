using Godot;
using GodotTemplate.Scenes.World.Data.PersistenceData.General;
using GodotTemplate.Scenes.World.Data.PersistenceData.MapPoint;
using GodotTemplate.Scenes.World.Data.PersistenceData.Player;
using KludgeBox.DI.Requests.ChildInjection;

namespace GodotTemplate.Scenes.World.Data.PersistenceData;

public partial class WorldPersistenceData : Node
{
    
    [Child] public GeneralDataStorage General { get; private set; }
    [Child] public PlayerDataStorage Players { get; private set; }
    [Child] public MapPointDataStorage MapPoint { get; private set; }
    
    public override void _Ready()
    {
        Di.Process(this);
    }
}