using System;
using System.Collections.Generic;
using Godot;
using GodotTemplate.Scenes.World.Data.PersistenceData;
using GodotTemplate.Scenes.World.Data.PersistenceData.Player;
using GodotTemplate.Scenes.World.Data.TemporaryData;
using GodotTemplate.Scenes.World.Scenes.ClientScenes;
using GodotTemplate.Scenes.World.Scenes.SyncedScenes;
using GodotTemplate.Scenes.World.Services.DataSerializer;
using GodotTemplate.Scenes.World.Services.PersistenceFactory;
using GodotTemplate.Scenes.World.Services.StartStop;
using GodotTemplate.Scenes.World.Tree;
using KludgeBox.DI.Requests.SceneServiceInjection;

namespace GodotTemplate.Scenes.World.Services;

public partial class WorldFacadeService : Node
{
    
    [SceneService] private WorldTree _tree;
    [SceneService] private WorldPersistenceData _persistenceData;
    [SceneService] private WorldTemporaryData _temporaryData;
    
    [SceneService] private PersistenceNodesFactoryService _factoryService;
    [SceneService] private WorldMultiplayerSpawnerService _multiplayerSpawnerService;
    [SceneService] private WorldStartStopService _startStopService;
    [SceneService] private WorldSynchronizerService _synchronizerService;
    [SceneService] private WorldDataSaveLoadService _dataSaveLoadService;
    [SceneService] private WorldDataSerializerService _dataSerializerService;
    
    [SceneService] private SyncedPackedScenes _syncedPackedScenes;
    [SceneService] private ClientPackedScenes _clientPackedScenes;
    
    public override void _Ready()
    {
        Di.Process(this);
    }

    public PlayerData GetPlayerData(long peerId)
    {
        String playerNick = _temporaryData.PlayerNickByPeerId.GetValueOrDefault(peerId, null);
        if (playerNick == null) return null;
        
        return _persistenceData.Players.PlayerByNick.GetValueOrDefault(playerNick, null);
    }

    public bool IsAdmin(long peerId)
    {
        if (peerId == ServerId) return true;
        
        PlayerData playerData = GetPlayerData(peerId);
        if (playerData == null) return false;
        return playerData.IsAdmin;
    }
}