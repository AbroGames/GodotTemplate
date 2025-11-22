using Godot;
using GodotTemplate.Scenes.Root.Starters;
using KludgeBox.DI.Requests.NotNullCheck;
using NodeContainer = GodotTemplate.Scenes.KludgeBox.NodeContainer;

namespace GodotTemplate.Scenes.Root;

public partial class Root : Node2D
{
    
    [Export] [NotNull] public NodeContainer MainSceneContainer { get; set; }
    [Export] [NotNull] public NodeContainer LoadingScreenContainer { get; set; }
    [Export] [NotNull] public RootPackedScenes PackedScenes { get; set; }

    private RootStarterManager _rootStarterManager;
    
    public override void _Ready()
    {
        Di.Process(this);
        
        Callable.From(() => {
            Init();
            Start();
        }).CallDeferred();
    }

    private void Init()
    {
        _rootStarterManager = new RootStarterManager(this);
        _rootStarterManager.Init();
    }

    private void Start()
    {
        _rootStarterManager.Start();
    }
}
