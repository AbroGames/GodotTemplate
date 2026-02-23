namespace GodotTemplate.Scenes.World.Services.DataSerializer;

public interface ISerializableStorage
{
    
    public byte[] SerializeStorage();
    public void DeserializeStorage(byte[] storageBytes);
    public void SetAllPropertyListeners();
}