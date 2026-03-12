using System;
using GodotTemplate.Scenes.World.Service.Chat;

namespace GodotTemplate.Scenes.World.Service.Command;

public class ChatMessageCommandInterceptor : IChatMessageInterceptor
{
    private readonly Action<int, string> _onCommand;

    public ChatMessageCommandInterceptor(Action<int, string> onCommand)
    {
        _onCommand = onCommand;
    }

    public bool IsPass(int senderId, string text)
    {
        if (text.StartsWith('/'))
        {
            _onCommand.Invoke(senderId, text.Substring(1));
            return false;
        }
        return true;
    }
}