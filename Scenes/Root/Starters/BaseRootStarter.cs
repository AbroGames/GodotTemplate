namespace GodotTemplate.Scenes.Root.Starters;

public abstract class BaseRootStarter
{

    public virtual void Init(Root root)
    {
        // We can't log anything before Lib initialized
        new LibInitializer()
            .SetNodeNetworkExtensionsIsClientChecker(_ => !Service.CmdArgs.IsDedicatedServer) // IsDedicatedServer is null now, but will be set before the lambda is called
            .Init();
        
        Log.Info("Initializing base...");
        
        root.PackedScenes.Init();
        Service.LoadingScreen.Init(root.LoadingScreenContainer, root.PackedScenes.LoadingScreen);
        Service.MainScene.Init(root.MainSceneContainer, root.PackedScenes.Game, root.PackedScenes.MainMenu);
    }

    public virtual void Start(Root root)
    {
        Log.Info("Starting base...");
    }
}