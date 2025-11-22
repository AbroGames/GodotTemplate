using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;

namespace GodotTemplate.Scenes.Root.Starters;

public abstract class BaseRootStarter
{
    
    [Logger] private ILogger _log;

    public virtual void Init(Root root)
    {
        Di.Process(this);
        
        // We can't log anything before Lib initialized
        /* TODO
         new LibInitializer()
            .SetNodeNetworkExtensionsIsClientChecker(_ => !Service.CmdArgs.IsDedicatedServer) // IsDedicatedServer is null now, but will be set before the lambda is called
            .Init();*/
        
        _log.Information("Initializing base...");
        
        root.PackedScenes.Init();
        Service.LoadingScreen.Init(root.LoadingScreenContainer, root.PackedScenes.LoadingScreen);
        Service.MainScene.Init(root.MainSceneContainer, root.PackedScenes.Game, root.PackedScenes.MainMenu);
    }

    public virtual void Start(Root root)
    {
        _log.Information("Starting base...");
    }
}