using System.Linq;
using System.Reflection;
using KludgeBox.DI.Requests.LoggerInjection;
using KludgeBox.Logging;
using Serilog;

namespace GodotTemplate.Scenes.Root.Starters;

public abstract class BaseRootStarter
{
    
    [Logger] private ILogger _log;

    public virtual void Init(RootData rootData)
    {
        Di.Process(this);
        
        LogFactory.GodotPushEnable = Services.CmdArgs.GodotLogPush;
        Services.ExceptionHandler.AddExceptionHandlerForUnhandledException();
        Services.CmdArgs.LogCmdArgs();
        
        _log.Information("Initializing base...");
        Services.TypesStorage.AddTypes(Assembly.GetExecutingAssembly().GetTypes().ToList());
        Services.Net.Init(rootData.SceneTree, Services.CmdArgs.IsDedicatedServer);
        Services.LoadingScreen.Init(rootData.LoadingScreenContainer, rootData.PackedScenes.LoadingScreen);
        Services.MainScene.Init(rootData.MainSceneContainer, rootData.PackedScenes.Game, rootData.PackedScenes.MainMenu);
    }

    public virtual void Start(RootData rootData)
    {
        _log.Information("Starting base...");
    }
}