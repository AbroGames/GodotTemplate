using GodotTemplate.Scripts.Service.LoadingScreen;
using GodotTemplate.Scripts.Service.MainScene;
using GodotTemplate.Scripts.Service.Process;
using GodotTemplate.Scripts.Service.Settings;
using KludgeBox.Core;
using KludgeBox.Core.Random;
using KludgeBox.DI;
using CmdArgsService = GodotTemplate.Scripts.Service.CmdArgs.CmdArgsService;

namespace GodotTemplate.Scripts;

public static class Services
{
    // Services from KludgeBox
    public static readonly DependencyInjector Di = new DependencyInjector();
    public static readonly RandomService Rand = new RandomService();
    public static readonly MathService Math = new MathService();
    public static readonly StringCompressionService StringCompression = new StringCompressionService();
    //TODO public static readonly ExceptionHandlerService ExceptionHandler = new ExceptionHandlerService();
    
    // Services from game
    public static readonly CmdArgsService CmdArgs = new CmdArgsService();
    public static readonly ProcessService Process = new ProcessService();
    public static readonly LoadingScreenService LoadingScreen = new LoadingScreenService();
    public static readonly MainSceneService MainScene = new MainSceneService();
    public static readonly PlayerSettingsService PlayerSettings = new PlayerSettingsService();
    
    public static class Global
    {
        public static DependencyInjector Di => Services.Di;
    }
}