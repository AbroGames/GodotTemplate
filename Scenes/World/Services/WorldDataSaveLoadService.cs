using System;
using Godot;
using GodotTemplate.Scenes.World.Data.PersistenceData;
using GodotTemplate.Scenes.World.Services.DataSerializer;
using KludgeBox.DI.Requests.LoggerInjection;
using KludgeBox.DI.Requests.SceneServiceInjection;
using Serilog;

namespace GodotTemplate.Scenes.World.Services;

public partial class WorldDataSaveLoadService : Node
{

    public class SaveException(string message, Exception innerException = null) : Exception(message, innerException);
    public class LoadException(string message, Exception innerException = null) : Exception(message, innerException);
    
    private const string SaveDirPath = "user://saves/";
    private const string SaveExtension = ".bin";
    private const string AutoSaveName = "auto";
    
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
        SaveToDisk(data, saveFileName);
    }
    
    public void AutoSave()
    {
        byte[] data = TrySerializeWorldData();
        SaveToDisk(data, AutoSaveName);
    }

    public void Load(string saveFileName)
    {
        byte[] data = LoadFromDisk(saveFileName);
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
            throw new LoadException($"Failed to serialize world data: {e.Message}", e);
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
            throw new LoadException($"Failed to deserialize world data: {e.Message}", e);
        }
    }

    private void SaveToDisk(byte[] data, string saveFileName)
    {
        DirAccess.MakeDirRecursiveAbsolute(SaveDirPath);
        string fullPath = GetFullPath(saveFileName);
        using var file = FileAccess.Open(fullPath, FileAccess.ModeFlags.Write);
        if (file == null)
        {
            _log.Error("Failed to save file '{fullPath}': {error}", fullPath, FileAccess.GetOpenError());
            throw new SaveException($"Failed to save file '{fullPath}': {FileAccess.GetOpenError()}");
        }
        
        file.StoreBuffer(data);
        file.Close();
        _log.Information("Successfully save file '{fullPath}'", fullPath);
    }
    
    private byte[] LoadFromDisk(string saveFileName)
    {
        string fullPath = GetFullPath(saveFileName);
        using var file = FileAccess.Open(fullPath, FileAccess.ModeFlags.Read);
        if (file == null)
        {
            _log.Error("Failed to load file '{fullPath}': {error}", fullPath, FileAccess.GetOpenError());
            throw new LoadException($"Failed to load file '{fullPath}': {FileAccess.GetOpenError()}");
        }

        byte[] data = file.GetBuffer((long) file.GetLength());
        file.Close();
        if (data == null)
        {
            _log.Error("Failed to load data from file '{fullPath}'", fullPath);
            throw new LoadException($"Failed to load data from file '{fullPath}'");
        }
        _log.Information("Successfully load file '{fullPath}'", fullPath);
        
        return data;
    }

    private string GetFullPath(string saveFileName)
    {
        return SaveDirPath + saveFileName + SaveExtension;
    }
}