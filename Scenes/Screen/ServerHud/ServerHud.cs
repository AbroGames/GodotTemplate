using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using KludgeBox.DI.Requests.ChildInjection;
using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;

namespace GodotTemplate.Scenes.Screen.ServerHud;

public partial class ServerHud : Control
{
    
    [Child] private Button Test1Button { get; set; }
    [Child] private Button Test2Button { get; set; }
    [Child] private Button Test3Button { get; set; }
    [Child] private Button LogButton { get; set; }
    [Child] private Label InfoLabel { get; set; }
    [Child] private Button SaveButton { get; set; }
    [Child] private TextEdit SaveTextEdit { get; set; }
    
    private World.World _world;
    [Logger] private ILogger _log;
    
    public ServerHud InitPreReady(World.World world)
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

        SaveButton.Pressed += () => { _world.DataSaveLoadService.Save(SaveTextEdit.Text); };
    }

    public override void _Process(double delta)
    {
        InfoLabel.Text = _world.PerformanceService.Godot.GetManyLinesString() + "\n" + GetPlayersInfo();
    }
    
    private String GetPlayersInfo()
    {
        IEnumerable<String> playersInfo = _world.TemporaryData.PlayerNickByPeerId.Select(kv => $"{kv.Key}: {kv.Value}");
        return "Players:\n" + string.Join("\n", playersInfo);
    }
}