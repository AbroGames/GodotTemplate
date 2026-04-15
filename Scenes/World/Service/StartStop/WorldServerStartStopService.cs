using System;
using Godot;
using GodotTemplate.Scenes.World.Data.PersistenceData;
using GodotTemplate.Scenes.World.Data.TemporaryData;
using GodotTemplate.Scenes.World.Service.Command;
using KludgeBox.DI.Requests.LoggerInjection;
using KludgeBox.DI.Requests.ParentInjection;
using KludgeBox.DI.Requests.SceneServiceInjection;
using Serilog;

namespace GodotTemplate.Scenes.World.Service.StartStop;


public partial class WorldServerStartStopService : Node
{
    
    [Parent] private World _world;
    [SceneService] private WorldPersistenceData _persistenceData;
    [SceneService] private WorldTemporaryData _temporaryData;
    
    [SceneService] private WorldSynchronizerService _synchronizerService;
    [SceneService] private WorldDataSaveLoadService _dataSaveLoadService;
    [SceneService] private WorldCommandService _commandService;
    [Logger] private ILogger _log;

    public override void _Ready()
    {
        Di.Process(this);
    }

    public void StartNewGame(string saveFileName, string adminUid)
    {
        if (!Net.IsServer()) throw new InvalidOperationException("Can only be executed on the server");
        
        CommonServerInit(adminUid);
        NewGameServerInit(saveFileName);
    }
    
    public void LoadGame(string saveFileName, string adminUid)
    {
        if (!Net.IsServer()) throw new InvalidOperationException("Can only be executed on the server");
        
        CommonServerInit(adminUid);
        LoadServerInit(saveFileName);
    }

    private void CommonServerInit(string adminUid)
    {
        _log.Information("World starting...");
        
        // Init WorldTemporaryData
        // Use inner function for detach this function after server shutdown,
        // otherwise we can have memory leak for this function
        void PeerDisconnectedEvent(long id)
        {
            _temporaryData.PlayerUidByPeerId.Remove((int) id);
        }
        GetMultiplayer().PeerDisconnected += PeerDisconnectedEvent;
        
        // Init WorldSynchronizerService
        _synchronizerService.InitOnServer(adminUid);
        
        // Init command system
        _commandService.InitOnServer();
        
        // Init node for server shutdown process in the future
        WorldServerShutdowner worldServerShutdowner = new WorldServerShutdowner();
        worldServerShutdowner.AddCustomShutdownAction(() => GetMultiplayer().PeerDisconnected -= PeerDisconnectedEvent);
        AddChild(worldServerShutdowner);
    }

    private void NewGameServerInit(string saveFileName)
    {
        // Set savaFileName for future saving or auto-saving
        _persistenceData.General.GeneralData.SaveFileName = saveFileName;
    }

    private void LoadServerInit(string saveFileName)
    {
        _dataSaveLoadService.Load(saveFileName);
        
        // We use this class very rarely, so we don't need to save it as class field
        new WorldTreeLoader().RunAllLoaders(_world);
    }
}