using GodotTemplate.Scripts.Services.CmdArgs;
using GodotTemplate.Scripts.Services.LoadingScreen;
using GodotTemplate.Scripts.Services.MainScene;
using GodotTemplate.Scripts.Services.Process;
using GodotTemplate.Scripts.Services.Settings;
using KludgeBox.Core.Random;
using KludgeBox.DI;

namespace GodotTemplate.Scripts.Services;

public static class Service
{
    public static DependencyInjector Di = new DependencyInjector();
    public static RandomService Rand = new RandomService();
    
    public static CmdArgsService CmdArgs = new CmdArgsService();
    public static ProcessService Process = new ProcessService();
    public static LoadingScreenService LoadingScreen = new LoadingScreenService();
    public static MainSceneService MainScene = new MainSceneService();
    public static PlayerSettingsService PlayerSettings = new PlayerSettingsService();
    
    public static class Global
    {
        public static DependencyInjector Di => Service.Di;
    }
}