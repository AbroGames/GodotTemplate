using System;
using System.Linq;
using System.Text;
using Godot;
using GodotTemplate.Scenes.World.Tree;
using KludgeBox.DI.Requests.SceneServiceInjection;
using GPerf = Godot.Performance;

namespace GodotTemplate.Scenes.World.Services.Performance;

public partial class WorldGodotPerformance : Node
{
    
    public double FramePerSecond => Engine.GetFramesPerSecond();
    public double FrameTime => GPerf.GetMonitor(GPerf.Monitor.TimeProcess) * 1000;
    
    public double TickPerSecond => Mathf.Min(1.0 / GPerf.GetMonitor(GPerf.Monitor.TimePhysicsProcess), Engine.PhysicsTicksPerSecond);
    public double TickTime => GPerf.GetMonitor(GPerf.Monitor.TimePhysicsProcess) * 1000;
    public double TickTimePercent => GPerf.GetMonitor(GPerf.Monitor.TimePhysicsProcess) * Engine.PhysicsTicksPerSecond * 100;

    public double NavigationTime => GPerf.GetMonitor(GPerf.Monitor.TimeNavigationProcess) * 1000;
    
    public int NodeCount => (int) GPerf.GetMonitor(GPerf.Monitor.ObjectNodeCount);
    public int SurfacesChildCount => _tree.BattleSurfaces.ToList().Cast<Node>().Prepend(_tree.MapSurface).Sum(surf => surf.GetChildCount());
    
    [SceneService] private WorldTree _tree;

    public override void _Ready()
    {
        Di.Process(this);
    }

    public String GetManyLinesString()
    {
        StringBuilder sb = new();
        
        sb.Append($"FPS: {FramePerSecond:N0}\n");
        sb.Append($"TPS: {TickPerSecond:N0}\n");
        sb.Append("\n");
        sb.Append($"Nodes: {NodeCount}\n");
        sb.Append($"Surfaces 1-level nodes: {SurfacesChildCount}\n");
        sb.Append($"Frame time process: {FrameTime:N1}ms\n");
        sb.Append($"Physics time process: {TickTime:N1}ms ({TickTimePercent:N0} %)\n");
        sb.Append($"Navigation time process: {NavigationTime:N1}ms\n");

        return sb.ToString();
    }
    
    public String GetTwoLinesString()
    {
        StringBuilder sb = new();
        
        sb.Append($"FPS/TPS: {FramePerSecond:N0}/{TickPerSecond:N0}    ");
        sb.Append($"Nodes (1-level/all): {SurfacesChildCount}/{NodeCount}\n");
        sb.Append($"Time (frame/physics/navi): {FrameTime:N1}/{TickTime:N1}({TickTimePercent:N0}%)/{NavigationTime:N1}ms");

        return sb.ToString();
    }
}