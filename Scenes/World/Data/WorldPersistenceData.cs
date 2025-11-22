using Godot;
using GodotTemplate.Scenes.World.Data.General;
using GodotTemplate.Scenes.World.Data.MapPoint;
using GodotTemplate.Scenes.World.Data.Player;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.World.Data;

public partial class WorldPersistenceData : Node
{
    
    [Export] [NotNull] public GeneralDataStorage General { get; private set; }
    [Export] [NotNull] public PlayerDataStorage Players { get; private set; }
    [Export] [NotNull] public MapPointDataStorage MapPoint { get; private set; }
    
    public WorldDataSaveLoad SaveLoad;
    public WorldDataSerializer Serializer;
    
    public override void _Ready()
    {
        Di.Process(this);

        SaveLoad = new(this);
        Serializer = new(this);
    }
}