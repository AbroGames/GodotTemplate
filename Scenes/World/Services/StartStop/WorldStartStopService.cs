using Godot;

namespace GodotTemplate.Scenes.World.Services.StartStop;


public partial class WorldStartStopService : Node
{
    
    private World _world;
    private readonly WorldTreeLoadService _worldTreeLoadService = new();

    /// <summary>
    /// In shutdown process (TreeExit signal) we can't use Node.GetMultiplayer().IsServer() for checking, because after TreeExit Node.GetMultiplayer() returns null.
    /// Also, Game.Network may have been disabled earlier, in which case it replaces the Peer with an OfflinePeer during the shutdown process.
    /// </summary>
    private bool _isServer;

    public WorldStartStopService InitPostReady(World world)
    {
        _world = world;
        return this;
    }
    
    public void StartNewGame(string adminNickname = null)
    {
        ServerInit(adminNickname);
    }
    
    public void LoadGame(string saveFileName, string adminNickname = null)
    {
        ServerInit(adminNickname);
        _world.Data.SaveLoad.Load(saveFileName);
        _worldTreeLoadService.RunAllLoaders(_world);
    }

    private void ServerInit(string adminNickname = null)
    {
        _isServer = true;
        _world.TemporaryDataService.InitOnServer(adminNickname);
    }
    
    public override void _Notification(int id)
    {
        if (id == NotificationExitTree && _isServer) Shutdown();
    }
    
    private void Shutdown()
    {
        _world.Data.SaveLoad.AutoSave();
    }
}