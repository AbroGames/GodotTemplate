using System;
using Godot;
using GodotTemplate.Scenes.World.Data.PersistenceData;
using GodotTemplate.Scenes.World.Data.PersistenceData.Player;
using GodotTemplate.Scenes.World.Data.TemporaryData;
using GodotTemplate.Scenes.World.Services.DataSerializer;
using GodotTemplate.Scripts.Service;
using KludgeBox.DI.Requests.LoggerInjection;
using KludgeBox.DI.Requests.SceneServiceInjection;
using Serilog;

namespace GodotTemplate.Scenes.World.Services;

public partial class WorldDataSaveLoadService : Node
{
    
    private const string NotRightsForSaveMessage = "You don't have the rights for saving";
    
    [SceneService] private WorldPersistenceData _persistenceData;
    [SceneService] private WorldTemporaryData _temporaryData;
    [SceneService] private WorldDataSerializerService _serializerService;
    [Logger] private ILogger _log;

    public override void _Ready()
    {
        Di.Process(this);
    }
    
    public void Save(string saveFileName) => RpcId(ServerId, MethodName.SaveRpc, saveFileName);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void SaveRpc(string saveFileName)
    {
        //TODO Фасад для этого? Прям в World? Но лучше в Facade. Причем два разных метода: получить PlayerData и проверка на Admin (т.к. peer = 1 может не иметь PlayerData, но он admin)
        int peerId = GetMultiplayer().GetRemoteSenderId();
        String playerNick = _temporaryData.PlayerNickByPeerId[peerId];
        PlayerData playerData = _persistenceData.Players.PlayerByNick[playerNick];
        if (peerId != 1 && !playerData.IsAdmin)
        {
            SaveReject(peerId, NotRightsForSaveMessage);
            return;
        }

        String currentSaveFileName = _persistenceData.General.GeneralData.SaveFileName;
        try
        {
            _persistenceData.General.GeneralData.SaveFileName = saveFileName;
            byte[] data = TrySerializeWorldData();
            Scripts.Services.SaveLoad.SaveToDisk(data, saveFileName);
        }
        catch (SaveLoadService.SaveException saveException)
        {
            // Return old SaveFileName, because we couldn't save the game with new filename.
            _persistenceData.General.GeneralData.SaveFileName = currentSaveFileName;
            SaveReject(peerId, saveException.Message);
        }
    }

    public void SaveReject(long peerId, string errorMessage) => RpcId(peerId, MethodName.SaveRejectRpc, errorMessage);
    [Rpc(CallLocal = true)]
    private void SaveRejectRpc(string errorMessage)
    {
        //TODO NotificationService.ShowWindowLocal? Но зачем? Лучше сразу Hud дергать. Через events, видимо.
    }
    
    public void AutoSave()
    {
        byte[] data = TrySerializeWorldData();
        Scripts.Services.SaveLoad.SaveToDisk(data, Scripts.Services.SaveLoad.AutoSaveName);
    }

    public void Load(string saveFileName)
    {
        byte[] data = Scripts.Services.SaveLoad.LoadFromDisk(saveFileName);
        TryDeserializeWorldData(data);
    }
    
    private byte[] TrySerializeWorldData()
    {
        try
        {
            return _serializerService.SerializeWorldData();
        }
        catch (Exception e)
        {
            _log.Error("Failed to serialize world data: {error}", e.Message);
            throw new SaveLoadService.SaveException($"Failed to serialize world data: {e.Message}", e);
        }
    }

    private void TryDeserializeWorldData(byte[] worldDataBytes)
    {
        try
        {
            _serializerService.DeserializeWorldData(worldDataBytes);
        }
        catch (Exception e)
        {
            _log.Error("Failed to deserialize world data: {error}", e.Message);
            throw new SaveLoadService.LoadException($"Failed to deserialize world data: {e.Message}", e);
        }
    }
}