using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotTemplate.Scenes.World.Data.PersistenceData;
using GodotTemplate.Scenes.World.Data.PersistenceData.Player;
using GodotTemplate.Scenes.World.Data.TemporaryData;
using GodotTemplate.Scenes.World.Scenes.ClientScenes;
using GodotTemplate.Scenes.World.Scenes.SyncedScenes;
using GodotTemplate.Scenes.World.Service.Chat;
using GodotTemplate.Scenes.World.Service.Command;
using GodotTemplate.Scenes.World.Service.DataSerializer;
using GodotTemplate.Scenes.World.Service.Performance;
using GodotTemplate.Scenes.World.Service.PersistenceFactory;
using GodotTemplate.Scenes.World.Service.StartStop;
using GodotTemplate.Scenes.World.Tree;
using KludgeBox.DI.Requests.SceneServiceInjection;

namespace GodotTemplate.Scenes.World.Service;

public partial class WorldFacadeService : Node
{
    
    [SceneService] private WorldTree _tree;
    [SceneService] private WorldPersistenceData _persistenceData;
    [SceneService] private WorldTemporaryData _temporaryData;
    
    [SceneService] private PersistenceNodesFactoryService _factoryService;
    [SceneService] private WorldMultiplayerSpawnerService _multiplayerSpawnerService;
    [SceneService] private WorldServerStartStopService _serverStartStopService;
    [SceneService] private WorldClientStartStopService _clientStartStopService;
    [SceneService] private WorldSynchronizerService _synchronizerService;
    [SceneService] private WorldDataSaveLoadService _dataSaveLoadService;
    [SceneService] private WorldDataSerializerService _dataSerializerService;
    [SceneService] private WorldPerformanceService _performanceService;
    [SceneService] private WorldChatService _chatService;
    [SceneService] private WorldCommandService _commandService;
    
    [SceneService] private SyncedPackedScenes _syncedPackedScenes;
    [SceneService] private ClientPackedScenes _clientPackedScenes;
    
    public override void _Ready()
    {
        Di.Process(this);
    }

    public PlayerData GetClientPlayerData()
    {
        return GetPlayerData(GetMultiplayer().GetUniqueId());
    }

    public PlayerData GetPlayerData(long peerId)
    {
        String playerUid = _temporaryData.PlayerUidByPeerId.GetValueOrDefault(peerId, null);
        if (playerUid == null) return null;
        
        return _persistenceData.Players.PlayerByUid.GetValueOrDefault(playerUid, null);
    }

    public List<PlayerData> GetOnlinePlayers()
    {
        return _persistenceData.Players.PlayerByUid.Values
            .Where(playerData => _temporaryData.PlayerUidByPeerId.Values.Contains(playerData.Uid))
            .ToList();
    }
    
    public List<PlayerData> GetOfflinePlayers()
    {
        return _persistenceData.Players.PlayerByUid.Values
            .Where(playerData => !_temporaryData.PlayerUidByPeerId.Values.Contains(playerData.Uid))
            .ToList();
    }

    public bool IsAdmin(long peerId)
    {
        if (peerId == ServerId) return true;
        
        PlayerData playerData = GetPlayerData(peerId);
        if (playerData == null) return false;
        return playerData.IsAdmin;
    }
}