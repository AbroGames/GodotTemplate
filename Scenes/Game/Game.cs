using Godot;
using GodotTemplate.Scenes.Game.Starters;
using GodotTemplate.Scenes.KludgeBox;
using GodotTemplate.Scenes.Screen.Hud;
using GodotTemplate.Scripts.Service.Settings;
using KludgeBox.DI.Requests.ChildInjection;

namespace GodotTemplate.Scenes.Game;

public partial class Game : Node2D
{

    [Child] private NodeContainer WorldContainer { get; set; }
    [Child] private NodeContainer HudContainer { get; set; }
    [Child] private GamePackedScenes PackedScenes { get; set; }

    private Network.Network _network;
    private Synchronizer _synchronizer;

    public override void _Ready()
    {
        Di.Process(this);
    }

    public void Init(BaseGameStarter gameStarter)
    {
        gameStarter.Init(this);
    }

    public World.World AddWorld()
    {
        World.World world = PackedScenes.World.Instantiate<World.World>();
        world.SetName("World");
        WorldContainer.ChangeStoredNode(world);
        return world;
    }
    
    public Hud AddHud()
    {
        Hud hud = PackedScenes.Hud.Instantiate<Hud>()
            .InitPreReady(WorldContainer.GetCurrentStoredNode<World.World>());
        hud.SetName("Hud");
        HudContainer.ChangeStoredNode(hud);
        return hud;
    }

    public Synchronizer AddSynchronizer(PlayerSettings playerSettings)
    {
        _synchronizer?.QueueFree();
        _synchronizer = new Synchronizer()
            .InitPreReady(WorldContainer.GetCurrentStoredNode<World.World>(), playerSettings);
        this.AddChildWithName(_synchronizer, "Synchronizer");
        return _synchronizer;
    }

    public Network.Network AddNetwork()
    {
        _network?.QueueFree();
        _network = new Network.Network();
        this.AddChildWithName(_network, "Network");
        return _network;
    }
}