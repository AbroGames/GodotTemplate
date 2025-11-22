using Godot;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.World.Services;

public partial class WorldMultiplayerSpawnerService : Node
{
    
    [Export] [NotNull] public PackedScene WorldMultiplayerSpawnerPackedScene { get; private set; }
    
    /// <summary>
    /// You can use this method, if observableNode in scene tree.
    /// If observableNode not in scene tree yet, you must use AddSpawnerToNode(Node observableNode, Node parentNode)
    /// </summary>
    /// <param name="observableNode">
    /// MultiplayerSpawner will observe this node and sync children of observableNode by network
    /// MultiplayerSpawner will be added as sibling of this node
    /// </param>
    /// <returns>Created spawner</returns>
    public WorldMultiplayerSpawner AddSpawnerToNode(Node observableNode)
    {
        return AddSpawnerToNode(observableNode, observableNode.GetParent());
    }
    
    /// <summary>
    /// You must use this method, if observableNode not in scene tree yet.
    /// If observableNode in scene tree, you can use AddSpawnerToNode(Node observableNode)
    /// </summary>
    /// <param name="observableNode">MultiplayerSpawner will observe this node and sync children of observableNode by network</param>
    /// <param name="parentNode">MultiplayerSpawner will be added as child of this node</param>
    /// <returns>Created spawner</returns>
    public WorldMultiplayerSpawner AddSpawnerToNode(Node observableNode, Node parentNode)
    {
        WorldMultiplayerSpawner worldMultiplayerSpawner = WorldMultiplayerSpawnerPackedScene.Instantiate<WorldMultiplayerSpawner>() 
            .InitPreReady(observableNode);
        parentNode.AddChildWithName(worldMultiplayerSpawner, observableNode.GetName() + "-MultiplayerSpawner");
        observableNode.TreeExiting += worldMultiplayerSpawner.QueueFree;
        return worldMultiplayerSpawner;
    }
}