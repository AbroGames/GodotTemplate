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
    public static CmdArgsService CmdArgs => ServiceLocator.Get<CmdArgsService>();
    public static ProcessService Process => ServiceLocator.Get<ProcessService>();
    public static LoadingScreenService LoadingScreen => ServiceLocator.Get<LoadingScreenService>();
    public static MainSceneService MainScene => ServiceLocator.Get<MainSceneService>();
    public static PlayerSettingsService PlayerSettings => ServiceLocator.Get<PlayerSettingsService>();
    
    public static DependencyInjector Di = new DependencyInjector();
    public static RandomService Rand = new RandomService();
    
    public static class Global
    {
        public static DependencyInjector Di => Service.Di;
    }
}