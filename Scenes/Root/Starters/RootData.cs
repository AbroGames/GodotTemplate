using Godot;
using GodotTemplate.Scenes.KludgeBox;

namespace GodotTemplate.Scenes.Root.Starters;

public record RootData(
    NodeContainer MainSceneContainer,
    NodeContainer LoadingScreenContainer,
    RootPackedScenes PackedScenes,
    SceneTree SceneTree);