using GodotTemplate.Scenes.Screen.MainMenu.Pages;
using Godot;
using GodotTemplate.Scenes.KludgeBox;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.Screen.MainMenu;

public partial class MainMenu : Node2D
{
    [Export] [NotNull] public NodeContainer BackgroundContainer { get; private set; }
    [Export] [NotNull] public NodeContainer MenuContainer { get; private set; }
    [Export] [NotNull] public MainMenuPackedScenes PackedScenes { get; private set; }
    
    public override void _Ready()
    {
        Di.Process(this);

        ChangeMenuPage(PackedScenes.Main);
    }
    
    public Node ChangeMenuPage(PackedScene newMenuPageScene)
    {
        MainMenuPage newMenuPage = newMenuPageScene.Instantiate<MainMenuPage>();
        newMenuPage.Init(ChangeMenuPage, PackedScenes);
        return MenuContainer.ChangeStoredNode(newMenuPage);
    }
}