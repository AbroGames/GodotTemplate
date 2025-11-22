using System;
using Godot;

namespace GodotTemplate.Scenes.Screen.MainMenu.Pages;

public partial class MainMenuPage : Control
{
    protected Func<PackedScene, Node> ChangeMenuPage;
    protected MainMenuPackedScenes PackedScenes;
    
    public void Init(Func<PackedScene, Node> changeMenuPage, MainMenuPackedScenes packedScenes)
    {
        ChangeMenuPage = changeMenuPage;
        PackedScenes = packedScenes;
    }
}