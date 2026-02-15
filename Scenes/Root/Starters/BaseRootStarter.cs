using System.Linq;
using System.Reflection;
using GodotTemplate.Scripts.Content.CmdArgs;
using KludgeBox.Core;
using KludgeBox.DI.Requests.LoggerInjection;
using KludgeBox.Logging;
using Serilog;

namespace GodotTemplate.Scenes.Root.Starters;

public abstract class BaseRootStarter
{
    
    // We don't move it to global Services class,
    // because RootStarter is the only right place for processing and forwarding cmd args
    protected readonly CmdArgsService CmdArgsService = new();
    
    private CommonArgs _commonArgs;
    [Logger] private ILogger _log;

    public virtual void Init(RootData rootData)
    {
        Di.Process(this);
        
        _commonArgs = CommonArgs.GetFromCmd(CmdArgsService);
        
        LogFactory.GodotPushEnable = _commonArgs.GodotLogPush;
        Services.ExceptionHandler.AddExceptionHandlerForUnhandledException();
        CmdArgsService.LogCmdArgs();
        
        _log.Information("Initializing base...");
        Services.ExecutingAssemblyCache.Init(Assembly.GetExecutingAssembly());
        Services.TypesStorage.AddTypes(Services.ExecutingAssemblyCache.Types.ToList());
        Services.LoadingScreen.Init(rootData.LoadingScreenContainer, rootData.PackedScenes.LoadingScreen);
        Services.MainScene.Init(rootData.MainSceneContainer, rootData.PackedScenes.Game, rootData.PackedScenes.MainMenu);
    }

    public virtual void Start(RootData rootData)
    {
        _log.Information("Starting base...");
    }
}