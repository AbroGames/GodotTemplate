using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;

namespace GodotTemplate.Scenes.Root.Starters;

public class DedicatedServerRootStarter : BaseRootStarter
{
    
    [Logger] private ILogger _log; //TODO Тест, т.к. здесь нет DI
    
    public override void Init(Root root)
    {
        base.Init(root);
        _log.Information("Initializing DedicatedServer...");
        
        root.GetTree().Root.Title = $"[SERVER] {root.GetTree().Root.Title}";
    }

    public override void Start(Root root)
    {
        base.Start(root);
        _log.Information("Starting DedicatedServer...");

        Service.MainScene.HostMultiplayerGameAsDedicatedServer(
            Service.CmdArgs.DedicatedServer.Port,
            Service.CmdArgs.DedicatedServer.SaveFileName,
            Service.CmdArgs.DedicatedServer.Admin,
            Service.CmdArgs.DedicatedServer.ParentPid,
            Service.CmdArgs.DedicatedServer.IsRender);
    }
}