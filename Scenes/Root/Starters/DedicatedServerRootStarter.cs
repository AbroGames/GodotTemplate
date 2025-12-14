using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;

namespace GodotTemplate.Scenes.Root.Starters;

public class DedicatedServerRootStarter : BaseRootStarter
{
    
    [Logger] private ILogger _log;
    
    public override void Init(RootData rootData)
    {
        base.Init(rootData);
        _log.Information("Initializing DedicatedServer...");
        
        rootData.SceneTree.Root.Title = $"[SERVER] {rootData.SceneTree.Root.Title}";
    }

    public override void Start(RootData rootData)
    {
        base.Start(rootData);
        _log.Information("Starting DedicatedServer...");

        Services.MainScene.HostMultiplayerGameAsDedicatedServer(
            Services.CmdArgs.DedicatedServer.Port,
            Services.CmdArgs.DedicatedServer.SaveFileName,
            Services.CmdArgs.DedicatedServer.Admin,
            Services.CmdArgs.DedicatedServer.ParentPid,
            Services.CmdArgs.DedicatedServer.IsRender);
    }
}