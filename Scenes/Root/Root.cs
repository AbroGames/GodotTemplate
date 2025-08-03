using System;
using Godot;

namespace GodotTemplate.Scenes.Root;

public partial class Root : Node2D
{
    
    public override void _Ready()
    {
        Callable.From(() => { 
            Init(); 
            Start();
        }).CallDeferred();
    }
    
    private void Init()
    {
        
    }
    
    private void Start()
    {
        
    }

    public void Shutdown()
    {
        GetTree().Root.PropagateNotification((int) NotificationWMCloseRequest); // Notify all nodes about game closing
        GetTree().Quit();
    }
}
