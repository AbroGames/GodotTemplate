using Godot;
using GodotTemplate.Scripts.Content.LoadingScreen;
using LoadingScreenNode = GodotTemplate.Scenes.Screen.LoadingScreen.LoadingScreen;

namespace GodotTemplate.Scripts.Service.LoadingScreen;

public class LoadingScreenService
{
    
    private Scenes.KludgeBox.NodeContainer _loadingScreenContainer;
    private PackedScene _loadingScreenPackedScene;

    public void Init(Scenes.KludgeBox.NodeContainer loadingScreenContainer, PackedScene loadingScreenPackedScene)
    {
        _loadingScreenContainer = loadingScreenContainer;
        _loadingScreenPackedScene = loadingScreenPackedScene;
    }
    
    public LoadingScreenNode SetLoadingScreen(string text)
    {
        LoadingScreenNode loadingScreen = _loadingScreenPackedScene.Instantiate<LoadingScreenNode>();
        if (text != null)
        {
            loadingScreen.SetText(text);
        }

        _loadingScreenContainer.ChangeStoredNode(loadingScreen);
        return loadingScreen;
    }
    
    public LoadingScreenNode SetLoadingScreen(LoadingScreenTypes.Type loadingScreenType)
    {
        return SetLoadingScreen(LoadingScreenTypes.GetLoadingScreenText(loadingScreenType));
    }
	
    public void Clear()
    {
        _loadingScreenContainer.ClearStoredNode();
    }
}