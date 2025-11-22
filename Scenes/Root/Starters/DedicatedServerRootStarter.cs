namespace GodotTemplate.Scenes.Root.Starters;

public class DedicatedServerRootStarter : BaseRootStarter
{
    
    public override void Init(Root root)
    {
        base.Init(root);
        Log.Info("Initializing DedicatedServer...");
        
        root.GetTree().Root.Title = $"[SERVER] {root.GetTree().Root.Title}";
    }

    public override void Start(Root root)
    {
        base.Start(root);
        Log.Info("Starting DedicatedServer...");

        Service.MainScene.HostMultiplayerGameAsDedicatedServer(
            Service.CmdArgs.DedicatedServer.Port,
            Service.CmdArgs.DedicatedServer.SaveFileName,
            Service.CmdArgs.DedicatedServer.Admin,
            Service.CmdArgs.DedicatedServer.ParentPid,
            Service.CmdArgs.DedicatedServer.IsRender);
    }
}