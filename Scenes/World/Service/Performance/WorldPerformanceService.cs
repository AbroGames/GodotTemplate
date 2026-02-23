using Godot;
using KludgeBox.DI.Requests.ChildInjection;

namespace GodotTemplate.Scenes.World.Services.Performance;

public partial class WorldPerformanceService : Node
{
    [Child] public WorldGodotPerformance Godot;
    [Child] public WorldPingPerformance Ping;

    public override void _Ready()
    {
        Di.Process(this);
    }
}