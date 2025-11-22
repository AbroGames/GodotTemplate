using Godot;

namespace GodotTemplate.Scripts.Service;

public class NetworkService() : KludgeBox.Godot.Services.NetworkService(
    () => ((Engine.GetMainLoop() as SceneTree)?.GetMultiplayer().IsServer()).GetValueOrDefault(false),
    () => !Services.CmdArgs.IsDedicatedServer);