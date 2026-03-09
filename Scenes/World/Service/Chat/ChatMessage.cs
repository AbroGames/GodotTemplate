using MessagePack;

namespace GodotTemplate.Scenes.World.Service.Chat;

[MessagePackObject]
public record ChatMessage
{
    [Key(0)] public int PeerId;
    [Key(1)] public string Nick;
    [Key(2)] public string Text;
}