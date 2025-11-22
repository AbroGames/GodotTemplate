using GodotTemplate.Scripts;
using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;

namespace GodotTemplate.Scenes.Root.Starters;

public abstract class BaseRootStarter
{
    
    [Logger] private ILogger _log;

    public virtual void Init(Root root)
    {
        //TODO Services.ExceptionHandler.AddExceptionHandlerForUnhandledException();
        
        Di.Process(this);
        _log.Information("Initializing base...");
        
        /* TODO
         new LibInitializer()
            .SetNodeNetworkExtensionsIsClientChecker(_ => !Service.CmdArgs.IsDedicatedServer) // IsDedicatedServer is null now, but will be set before the lambda is called
            .Init();*/
        
        root.PackedScenes.Init();
        Services.LoadingScreen.Init(root.LoadingScreenContainer, root.PackedScenes.LoadingScreen);
        Services.MainScene.Init(root.MainSceneContainer, root.PackedScenes.Game, root.PackedScenes.MainMenu);
    }

    public virtual void Start(Root root)
    {
        _log.Information("Starting base...");
    }
}