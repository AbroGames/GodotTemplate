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

    public bool Save(string saveFileName)
    {
        _worldData.General.GeneralData.SaveFileName = saveFileName;
        return SaveToDisk(_serializerService.SerializeWorldData(), saveFileName);
    }
    
    public bool AutoSave()
    {
        return SaveToDisk(_serializerService.SerializeWorldData(), AutoSaveName);
    }

    public bool Load(string saveFileName)
    {
        byte[] data = LoadFromDisk(saveFileName);
        if (data == null) return false;
        
        try
        {
            _serializerService.DeserializeWorldData(data);
        }
        catch (Exception e)
        {
            //TODO Вместо возвращаемого bool мб выбрасывать ошибку выше, чтобы обрабатывать потом в Host MpGame и в Host Single Game?
            string fullPath = GetFullPath(saveFileName);
            _log.Error("Failed to deserialize world data from save file '{fullPath}': {error}", fullPath, e.Message);
            return false;
        }

        return true;
    }

    private bool SaveToDisk(byte[] data, string saveFileName)
    {
        DirAccess.MakeDirRecursiveAbsolute(SaveDirPath);
        string fullPath = GetFullPath(saveFileName);
        using var file = FileAccess.Open(fullPath, FileAccess.ModeFlags.Write);
        if (file == null)
        {
            _log.Error("Failed to save file '{fullPath}': {error}", fullPath, FileAccess.GetOpenError());
            return false;
        }
        
        file.StoreBuffer(data);
        file.Close();
        return true;
    }
    
    private byte[] LoadFromDisk(string saveFileName)
    {
        string fullPath = GetFullPath(saveFileName);
        using var file = FileAccess.Open(fullPath, FileAccess.ModeFlags.Read);
        if (file == null)
        {
            _log.Error("Failed to load file '{fullPath}': {error}", fullPath, FileAccess.GetOpenError());
            return null;
        }
        
        byte[] data = file.GetBuffer((long) file.GetLength());
        file.Close();
        
        return data;
    }
    
    private string GetFullPath(string saveFileName) => SaveDirPath + saveFileName + SaveExtension;
}