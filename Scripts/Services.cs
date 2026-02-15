using GodotTemplate.Scripts.Service;
using GodotTemplate.Scripts.Service.Settings;
using KludgeBox.Core;
using KludgeBox.Core.Random;
using KludgeBox.DI;
using KludgeBox.Godot.Services;
using KludgeBox.Reflection.Access;
using CmdArgsService = GodotTemplate.Scripts.Service.CmdArgs.CmdArgsService;
using NetworkService = GodotTemplate.Scripts.Service.NetworkService;

namespace GodotTemplate.Scripts;

public static class Services
{
    // Services from KludgeBox
    public static readonly DependencyInjector Di = new();
    public static readonly ExceptionHandlerService ExceptionHandler = new();
    public static readonly RandomService Rand = new();
    public static readonly MathService Math = new();
    public static readonly StringCompressionService StringCompression = new();
    public static readonly NodeTreeService NodeTree = new();
    public static readonly TypesStorageService TypesStorage = new();
    public static readonly ExecutingAssemblyCacheService ExecutingAssemblyCache = new();
    public static MembersScanner MembersScanner => Di.MembersScanner;
    
    // Services from game, but extended KludgeBox services
    public static readonly CmdArgsService CmdArgs = new();
    public static readonly NetworkService Net = new();
    
    // Services from game
    public static readonly ProcessService Process = new();
    public static readonly LoadingScreenService LoadingScreen = new();
    public static readonly MainSceneService MainScene = new();
    public static readonly PlayerSettingsService PlayerSettings = new();
    
    public static class Global
    {
        public static DependencyInjector Di => Services.Di;
        public static NetworkService Net => Services.Net;
    }
}