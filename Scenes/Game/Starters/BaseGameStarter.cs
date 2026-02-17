using System;
using Godot;
using GodotTemplate.Scenes.World.Services;
using GodotTemplate.Scripts.Content.LoadingScreen;
using GodotTemplate.Scripts.Service;
using GodotTemplate.Scripts.Service.Settings;
using Humanizer;

namespace GodotTemplate.Scenes.Game.Starters;

public abstract class BaseGameStarter
{
    protected const string Localhost = "127.0.0.1";
    protected const string DefaultHost = Localhost;
    protected const int DefaultPort = 25566;
    
    private const string SyncRejectedMessage = "Synchronization with the server was rejected: {0}";

    public virtual void Init(Game game) { }

    protected void ConnectToClientSynchronizerEvents(WorldSynchronizerService synchronizerService)
    {
        if (!Net.IsClient()) throw new InvalidOperationException("Can only be executed on the client");

        synchronizerService.SyncRejectOnClientEvent += errorMessage => GoToMenuAndShowError(SyncRejectedMessage.FormatWith(errorMessage));
        synchronizerService.SyncStartedOnClientEvent += () => Services.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
        synchronizerService.SyncEndedOnClientEvent += () => Services.LoadingScreen.Clear();
    }

    protected void StartWorld(World.World world, string saveFileName, string adminNickname)
    {
        if (saveFileName == null)
        {
            world.StartStopService.StartNewGame(adminNickname);
        }
        else
        {
            try
            {
                world.StartStopService.LoadGame(saveFileName, adminNickname);
            }
            catch (SaveLoadService.LoadException loadException)
            {
                Net.DoClient(() => GoToMenuAndShowError(loadException.Message));
            }
        }
    }

    protected void StartSyncOnClient(WorldSynchronizerService synchronizerService)
    {
        PlayerSettings playerSettings = Services.PlayerSettings.GetPlayerSettings();
        synchronizerService.StartSyncOnClient(playerSettings.Nick, playerSettings.Color);
    }

    /// <summary>
    /// This method calls only on client.<br/>
    /// Log error message to logger must be early, because this method calls only on client,
    /// but we want log error message on client and server.
    /// </summary>
    protected void GoToMenuAndShowError(string message)
    {
        if (!Net.IsClient()) throw new InvalidOperationException("Can only be executed on the client");
        
        Services.MainScene.StartMainMenu(message);
        Services.LoadingScreen.Clear();
    }
}
