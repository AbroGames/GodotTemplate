using System;
using GodotTemplate.Scenes.World.Services;

namespace GodotTemplate.Scenes.Game.Starters;

public abstract class BaseGameStarter
{
    protected const string Localhost = "127.0.0.1";
    protected const string DefaultHost = Localhost;
    protected const int DefaultPort = 25566;

    public virtual void Init(Game game)
    {
        
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
        
        Services.MainScene.StartMainMenu();
        //TODO Show message in menu
        Services.LoadingScreen.Clear();
    }
}
