using GodotTemplate.Scenes.Root.Starters;
using Godot;
using GodotTemplate.Scenes.KludgeBox;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.Root;

public partial class Root : Node2D
{
    
    [Export] [NotNull] public NodeContainer MainSceneContainer { get; set; }
    [Export] [NotNull] public NodeContainer LoadingScreenContainer { get; set; }
    [Export] [NotNull] public RootPackedScenes PackedScenes { get; set; }

    private RootStarterManager _rootStarterManager;
    
    public override void _Ready()
    {
        Callable.From(() => {
            Init();
            Start();
        }).CallDeferred();
    }

    private void Init()
    {
        _rootStarterManager = new RootStarterManager(this);
        _rootStarterManager.Init();
        Di.Process(this); // We call NotNullChecker here, because it has not been created earlier
    }

    private void Start()
    {
        _rootStarterManager.Start();
    }
}
