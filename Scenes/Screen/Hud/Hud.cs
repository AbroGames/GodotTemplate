using System.Linq;
using Godot;
using GodotTemplate.Scenes.Game;
using GodotTemplate.Scripts.Content.LoadingScreen;
using KludgeBox.DI.Requests.ChildInjection;
using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;

namespace GodotTemplate.Scenes.Screen.Hud;

public partial class Hud : Control
{
    
    [Child] private Button Test1Button { get; set; }
    [Child] private Button Test2Button { get; set; }
    [Child] private Button Test3Button { get; set; }
    [Child] private Button LogButton { get; set; }
    [Child] private Label InfoLabel { get; set; }
    [Child] private Button SaveButton { get; set; }
    [Child] private Button ExitButton { get; set; }
    [Child] private TextEdit SaveTextEdit { get; set; }
    
    private World.World _world;
    [Logger] private ILogger _log;
    
    public Hud InitPreReady(World.World world)
    {
        Di.Process(this);
        
        if (world == null) _log.Error("World must be not null");
        _world = world;
        
        return this;
    }

    public override void _Ready()
    {
        Di.Process(this);
        Test1Button.Pressed += () => { _world.Test1(); };
        Test2Button.Pressed += () => { _world.Test2(); };
        Test3Button.Pressed += () => { _world.Test3(); };
        LogButton.Pressed += () => { Services.NodeTree.LogFullTree(_world); };

        ExitButton.Pressed += () => { Services.MainScene.StartMainMenu(); };
        SaveButton.Pressed += () => { _world.DataSaveLoadService.Save(SaveTextEdit.Text); };
    }

    public override void _Process(double delta)
    {
        //TODO В отдельный PerformanceAnalyzerWorldService, с генерацией строк текста нескольких видов: построчно или в 2 строках сжато. Туда же PingChecker и пересылку с клиентов на сервер инфы о пинге?
        InfoLabel.Text = $"FPS: {Engine.GetFramesPerSecond():N0}";
        InfoLabel.Text += $"\nTPS: {Mathf.Min(1.0/Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess), Engine.PhysicsTicksPerSecond):N0}";
        
        InfoLabel.Text += $"\n\nNodes: {Performance.GetMonitor(Performance.Monitor.ObjectNodeCount)}";
        InfoLabel.Text += $"\nSurfaces 1-level nodes: {_world.Tree.MapSurface?.GetChildCount() + _world.Tree.BattleSurfaces.ToList().Select(surf => surf.GetChildCount()).Sum()}";
        InfoLabel.Text += $"\nFrame time process: {Performance.GetMonitor(Performance.Monitor.TimeProcess)*1000:N1}ms";
        InfoLabel.Text += $"\nPhysics time process: {Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess)*1000:N1}ms ({Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess) * Engine.PhysicsTicksPerSecond * 100:N0} %)";
        InfoLabel.Text += $"\nNavigation time process: {Performance.GetMonitor(Performance.Monitor.TimeNavigationProcess)*1000:N1}ms";
    }
}