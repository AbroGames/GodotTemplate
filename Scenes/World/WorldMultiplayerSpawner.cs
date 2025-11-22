using Godot;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.World;

//TODO This node and scene in Lib utils nodes (as SelfMultiplayerSpawner) + AbstractPackedScenes (with Scenes field) to Lib utils nodes, change here WorldPackedScenes to AbstractPackedScenes
//TODO Для SelfMultiplayerSpawner: Необходимо создать наслденика сцены, наследника класса, добавить туда PackedScenes со всеми требуемыми нодами. Важно, чтобы в них не было самой SelfMultiplayerSpawner.
//TODO [GlobalClass] ? Но тогда с PackedScenes проблемы. Или всегда строго из кода создавать и сделать коммент об этом? Чтобы случайно не сломалось как Synchronizer
public partial class WorldMultiplayerSpawner : MultiplayerSpawner
{
    [Export] [NotNull] public WorldPackedScenes PackedScenes { get; set; }
    
    [Export] private bool _selfSync = true;
    private Node _observableNode;
    
    public WorldMultiplayerSpawner Init(Node observableNode, bool selfSync = true)
    {
        _observableNode = observableNode;
        _selfSync = selfSync;
        return this;
    }

    public override void _Ready()
    {
        foreach (var packedScene in PackedScenes.Scenes)
        {
            AddSpawnableScene(packedScene.ResourcePath);
        }
        if (_selfSync) 
        {
            AddSpawnableScene(SceneFilePath); // Reference by self
        }
        
        // _observableNode can be null if the Spawner is synced over the network by another Spawner or created in the Editor.
        // In these cases, SpawnPath must not be null.
        if (_observableNode != null) 
        {
            SetSpawnPath(GetPathTo(_observableNode));
        }
        else if (string.IsNullOrEmpty(GetSpawnPath())) 
        {
            Log.Error("SelfMultiplayerSpawner must have not null _observableNode or SpawnPath. Spawner path: " + GetPath());
        }
    }
}