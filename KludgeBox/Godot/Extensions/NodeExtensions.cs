using Godot;

namespace KludgeBox.Godot.Extensions;

public static class NodeExtensions
{
    private static ulong _lastId = 0;

    public static TNode WithUniqueName<TNode>(this TNode node, string name = null) where TNode : Node
    {
        if (name is null)
            name = typeof(TNode).Name;
        
        node.SetName($"{name}_{_lastId++}");
        
        return node;
    }
}