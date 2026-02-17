using System;
using Godot;
using GodotTemplate.Scenes.World.Data.PersistenceData;
using GodotTemplate.Scenes.World.Services.DataSerializer;
using GodotTemplate.Scripts.Service;
using KludgeBox.DI.Requests.LoggerInjection;
using KludgeBox.DI.Requests.SceneServiceInjection;
using Serilog;

namespace GodotTemplate.Scenes.World.Services;

public partial class WorldDataSaveLoadService : Node
{
    
    [SceneService] private WorldPersistenceData _worldData;
    [SceneService] private WorldDataSerializerService _serializerService;
    [Logger] private ILogger _log;

    public override void _Ready()
    {
        Di.Process(this);
    }

    public void Save(string saveFileName)
    {
        _worldData.General.GeneralData.SaveFileName = saveFileName;
        byte[] data = TrySerializeWorldData();
        Scripts.Services.SaveLoad.SaveToDisk(data, saveFileName);
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