using GodotTemplate.Scenes.Game.Net;
using GodotTemplate.Scripts.Content.LoadingScreen;
using GodotTemplate.Scripts.Services.Settings;
using Godot;

namespace GodotTemplate.Scenes.Game.Starters;

public class ConnectToMultiplayerGameStarter(string host = null, int? port = null) : BaseGameStarter
{
    
    public override void Init(Game game)
    {
        base.Init(game);
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Connecting);
        
        PlayerSettings playerSettings = Service.PlayerSettings.GetPlayerSettings();
        game.AddWorld();
        Synchronizer synchronizer = game.AddSynchronizer(playerSettings);
        game.AddHud();
        Network network = game.AddNetwork();
        
        game.GetMultiplayer().ConnectedToServer += synchronizer.StartSyncOnClient;
        game.GetMultiplayer().ConnectionFailed += ConnectionFailedEvent;
        game.GetMultiplayer().ServerDisconnected += ServerDisconnectedEvent;
        
        Error error = network.ConnectToServer(host ?? DefaultHost, port ?? DefaultPort);
        if (error != Error.Ok)
        {
            ConnectionFailedEvent();
        }
    }

    // Failed attempt to connect to the server (did not receive a response from the server within the timeout).
    private void ConnectionFailedEvent()
    {
        Service.MainScene.StartMainMenu();
        //TODO Show message in menu (it is client). Log already has message.
        Service.LoadingScreen.Clear();
    }
    
    // Server disconnected (the connection was successful, but the server disconnected us). This may also happen several hours after the connection.
    private void ServerDisconnectedEvent()
    {
        Service.MainScene.StartMainMenu();
        //TODO Show message in menu (it is client). Log already has message.
        Service.LoadingScreen.Clear();
    }
}