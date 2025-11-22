using System;
using Godot;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.Screen.LoadingScreen;

public partial class LoadingScreen : CanvasLayer
{
    [Export] [NotNull] public LoadingAnimHandle LoadingAnimHandle { get; private set; }
    [Export] [NotNull] public Label LoadingLabel { get; private set; }

    public override void _Ready()
    {
        Di.Process(this);
        SetLayer(Int32.MaxValue);
    }

    public void SetText(string loadingText)
    {
        LoadingLabel.Text = loadingText;
    }
}
