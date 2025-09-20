using System;
using Godot;

namespace KludgeBox.Godot.Nodes;

public enum CallType
{
    EnterTree,
    Ready,
    ReadyDeferred,
    ExitTree
}

/// <summary>
/// Выполняет действие в подходящий момент. Полезен, когда нужно подписаться на какой-то встроенный вызов движка снаружи.
/// Потенциально полезно для настроек владения нодами в сетевой игре.
/// </summary>
public partial class Deferrer : Node
{
    public CallType CallType { get; }
    public bool IsPersistent { get; set; }
    private readonly Action _action;

    public Deferrer(CallType callType, Action action, bool persistent = false)
    {
        CallType = callType;
        IsPersistent = persistent;
        _action = () =>
        {
            action();
            if (!IsPersistent)
            {
                QueueFree();
            }
        };
    }
    
    private Deferrer()
    {
    }

    public override void _Ready()
    {
        if (CallType is CallType.Ready)
        {
            _action();
        }

        if (CallType is CallType.ReadyDeferred)
        {
            Callable.From(_action).CallDeferred();
        }
    }

    public override void _EnterTree()
    {
        if (CallType is CallType.EnterTree)
        {
            _action();
        }
    }

    public override void _ExitTree()
    {
        if (CallType is CallType.ExitTree)
        {
            _action();
        }
    }
}

public static class DeferrerExtensions
{
    public static Deferrer Defer(this Node node, Action action, CallType callType = CallType.ReadyDeferred,
        bool persistent = false)
    {
        var deferrer = new Deferrer(callType, action, persistent);
        node.AddChild(deferrer);
        return deferrer;
    }
}