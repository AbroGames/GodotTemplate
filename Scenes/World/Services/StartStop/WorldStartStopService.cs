using System;
using Godot;
using GodotTemplate.Scenes.World.Data.PersistenceData;
using KludgeBox.DI.Requests.LoggerInjection;
using KludgeBox.DI.Requests.ParentInjection;
using KludgeBox.DI.Requests.SceneServiceInjection;
using Serilog;

namespace GodotTemplate.Scenes.World.Services.StartStop;


public partial class WorldStartStopService : Node
{
    
    [Parent] private World _world;
    [SceneService] private WorldDataSaveLoadService _dataSaveLoadService;
    [SceneService] private WorldPersistenceData _worldPersistenceData;
    [SceneService] private Data.TemporaryData.WorldTemporaryData _worldTemporaryData;
    [Logger] private ILogger _log;

    public override void _Ready()
    {
        Di.Process(this);
    }

    public void StartNewGame(string adminNickname = null)
    {
        if (!Net.IsServer()) throw new InvalidOperationException("Can only be executed on the server");
        
        CommonServerInit(adminNickname);
        NewGameServerInit();
    }
    
    public void LoadGame(string saveFileName, string adminNickname = null)
    {
        if (!Net.IsServer()) throw new InvalidOperationException("Can only be executed on the server");
        
        CommonServerInit(adminNickname);
        LoadServerInit(saveFileName);
    }

    private void CommonServerInit(string adminNickname = null)
    {
        _log.Information("World starting...");
        
        //Init WorldTemporaryData
        _worldTemporaryData.MainAdminNick = adminNickname;
        // Use inner function for detach this function after server shutdown,
        // otherwise we can have memory leak for this function
        void PeerDisconnectedEvent(long id)
        {
            _worldTemporaryData.PlayerNickByPeerId.Remove((int) id);
        }
        GetMultiplayer().PeerDisconnected += PeerDisconnectedEvent;
        
        //Init node for server shutdown process in the future
        WorldServerShutdowner worldServerShutdowner = new WorldServerShutdowner();
        worldServerShutdowner.AddCustomShutdownAction(() => GetMultiplayer().PeerDisconnected -= PeerDisconnectedEvent);
        AddChild(worldServerShutdowner);
    }

    private void NewGameServerInit()
    {
        
    }

    private void LoadServerInit(string saveFileName)
    {
        _dataSaveLoadService.Load(saveFileName);
        
        // We use this class very rarely, so we don't need to save it as class field
        new WorldTreeLoader().RunAllLoaders(_world);
    }
}