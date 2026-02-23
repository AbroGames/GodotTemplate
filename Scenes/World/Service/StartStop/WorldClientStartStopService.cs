using System;
using Godot;
using GodotTemplate.Scenes.World.Services.Performance;
using GodotTemplate.Scripts.Content.LoadingScreen;
using GodotTemplate.Scripts.Service.Settings;
using Humanizer;
using KludgeBox.DI.Requests.LoggerInjection;
using KludgeBox.DI.Requests.SceneServiceInjection;
using Serilog;

namespace GodotTemplate.Scenes.World.Services.StartStop;

public partial class WorldClientStartStopService : Node
{
    
    private const string SyncRejectedMessage = "Synchronization with the server was rejected: {0}";
    
    [SceneService] private WorldSynchronizerService _synchronizerService;
    [SceneService] private WorldPerformanceService _performanceService;
    [Logger] private ILogger _log;

    public override void _Ready()
    {
        Di.Process(this);
    }

    public void StartSyncWithServer(Action<string> goToMenuAndShowErrorAction)
    {
        if (!Net.IsClient()) throw new InvalidOperationException("Can only be executed on the client");
        _log.Information("World starting...");
        
        _synchronizerService.SyncRejectOnClientEvent += errorMessage => goToMenuAndShowErrorAction.Invoke(SyncRejectedMessage.FormatWith(errorMessage));
        _synchronizerService.SyncStartedOnClientEvent += OnSyncStarted;
        _synchronizerService.SyncEndedOnClientEvent += OnSyncEnded; //TODO Проверить утечки памяти и двойные синхронизации
        
        PlayerSettings playerSettings = Scripts.Services.PlayerSettings.GetPlayerSettings();
        _synchronizerService.StartSyncOnClient(playerSettings.Nick, playerSettings.Color);
    }
    
    private void OnSyncStarted()
    {
        Scripts.Services.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
    }

    private void OnSyncEnded()
    {
        _performanceService.Ping.Start();
        Scripts.Services.LoadingScreen.Clear();
    }
}