using Godot;
using GodotTemplate.Scenes.World.Services;
using GodotTemplate.Scripts.Content.LoadingScreen;

namespace GodotTemplate.Scenes.Game.Starters;

public class ConnectToMultiplayerGameStarter(string host = null, int? port = null) : BaseGameStarter
{
    
    private const string ConnectionFailedMessage = "Connection to the server failed";
    private const string DisconnectedFromServerMessage = "Server disconnected";
    
    public override void Init(Game game)
    {
        base.Init(game);
        Services.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Connecting);
        
        World.World world = game.AddWorld();
        game.AddHud();
        Network.Network network = game.AddNetwork();
        ConnectToClientSynchronizerEvents(world.SynchronizerService);
        
        game.GetMultiplayer().ConnectedToServer += () => StartSyncOnClient(world.SynchronizerService);
        game.GetMultiplayer().ConnectionFailed += ConnectionFailedEvent;
        game.GetMultiplayer().ServerDisconnected += ServerDisconnectedEvent;
        
        //AddMultiplayerEventListeners(game, world.SynchronizerService); TODO
        
        Error error = network.ConnectToServer(host ?? DefaultHost, port ?? DefaultPort);
        if (error != Error.Ok)
        {
            ConnectionFailedEvent();
        }
    }
    
    // Failed attempt to connect to the server (did not receive a response from the server within the timeout).
    private void ConnectionFailedEvent() => GoToMenuAndShowError(ConnectionFailedMessage);
    
    // Server disconnected (the connection was successful, but the server disconnected us). This may also happen several hours after the connection.
    private void ServerDisconnectedEvent() => GoToMenuAndShowError(DisconnectedFromServerMessage);
    
    private void AddMultiplayerEventListeners(Game game, WorldSynchronizerService synchronizerService)
    {
        void ConnectionSuccessEvent() => StartSyncOnClient(synchronizerService);
        
        // Failed attempt to connect to the server (did not receive a response from the server within the timeout).
        void ConnectionFailedEvent()
        {
            GD.Print("Connection failed"); //TODO
            GoToMenuAndShowError(ConnectionFailedMessage);
            RemoveMultiplayerEventListeners();
        }
        
        // Server disconnected (the connection was successful, but the server disconnected us). This may also happen several hours after the connection.
        void ServerDisconnectedEvent()
        {
            GoToMenuAndShowError(DisconnectedFromServerMessage);
            RemoveMultiplayerEventListeners();
        }
        
        void RemoveMultiplayerEventListeners()
        {
            game.GetMultiplayer().ConnectedToServer -= ConnectionSuccessEvent;
            game.GetMultiplayer().ConnectionFailed -= ConnectionFailedEvent;
            game.GetMultiplayer().ServerDisconnected -= ServerDisconnectedEvent;
        }
        
        game.GetMultiplayer().ConnectedToServer += ConnectionSuccessEvent;
        game.GetMultiplayer().ConnectionFailed += ConnectionFailedEvent;
        game.GetMultiplayer().ServerDisconnected += ServerDisconnectedEvent;
    }
}