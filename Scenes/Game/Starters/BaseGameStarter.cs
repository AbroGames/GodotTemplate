using System;
using GodotTemplate.Scenes.World.Services;
using GodotTemplate.Scripts.Content.LoadingScreen;
using Humanizer;
using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;

namespace GodotTemplate.Scenes.Game.Starters;

public abstract class BaseGameStarter
{
    protected const string Localhost = "127.0.0.1";
    protected const string DefaultHost = Localhost;
    protected const int DefaultPort = 25566;
    
    private const string SyncRejectedMessage = "Synchronization with the server was rejected: {0}";
    
    [Logger] ILogger _log;

    protected BaseGameStarter()
    {
        Di.Process(this);
    }

    public virtual void Init(Game game)
    {
        
    }

    protected void ConnectToClientSynchronizerEvents(Synchronizer synchronizer)
    {
        if (!Net.IsClient()) throw new InvalidOperationException("Can only be executed on the client");
        
        synchronizer.SyncRejectOnClientEvent += errorMessage => GoToMenuAndShowError(SyncRejectedMessage.FormatWith(errorMessage));
        synchronizer.SyncStartedOnClientEvent += () => Services.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
        synchronizer.SyncEndedOnClientEvent += () => Services.LoadingScreen.Clear();
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
            catch (WorldDataSaveLoadService.LoadException loadException)
            {
                Net.DoClient(() => GoToMenuAndShowError(loadException.Message));
            }
        }
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
