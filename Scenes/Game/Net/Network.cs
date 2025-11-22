using Godot;

namespace GodotTemplate.Scenes.Game.Net;

public partial class Network : Node
{

    public static readonly int MaxSyncPacketSize = 1350 * 100;
    
    public MultiplayerApi Api { get; private set; }
    public NetworkStateMachine StateMachine { get; } = new();

    public override void _Ready()
    {
        Api = GetMultiplayer();
        Api.ConnectedToServer += ConnectedToServerEvent;
        Api.PeerConnected += PeerConnectedEvent;
        Api.ConnectionFailed += ConnectionFailedEvent;
        Api.PeerDisconnected += PeerDisconnectedEvent;
        Api.ServerDisconnected += ServerDisconnectedEvent;
        (Api as SceneMultiplayer)?.SetMaxSyncPacketSize(MaxSyncPacketSize);
    }

    /// <summary>Try to connect to the server</summary>
    /// <returns>
    /// Returns Godot.Error.Ok if a client was created
    /// Godot.Error.AlreadyInUse if this ENetMultiplayerPeer instance already has an open connection
    /// Godot.Error.CantCreate if the client could not be created
    /// Godot.Error.AlreadyInUse if the client already connected
    /// </returns>
    public Error ConnectToServer(string host, int port)
    {
        if (!StateMachine.CanInitialize)
        {
            Log.Error($"Can't initialize network in current state: {StateMachine.CurrentState}");
            return Error.AlreadyInUse;
        }
        
        Log.Info($"Connecting to the server at {host}:{port}");

        StateMachine.SetState(NetworkStateMachine.State.Connecting);
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateClient(host, port);
        Api.MultiplayerPeer = peer;
		
        if (error != Error.Ok)
        {
            Log.Error($"Failed to connect to the server: {error}");
        }
        return error; 
    }
    
    /// <summary>
    /// Try to host server.
    /// If server hosted with refuseNewConnections = true, and you must call OpenServer() after hosting process.
    /// </summary>
    /// <returns>
    /// Returns Godot.Error.Ok if a server was created
    /// Godot.Error.AlreadyInUse if this ENetMultiplayerPeer instance already has an open connection
    /// Godot.Error.CantCreate if the server could not be created
    /// Godot.Error.AlreadyInUse if the server already hosted
    /// </returns>
    public Error HostServer(int port, bool refuseNewConnections = false, int maxClients = 32)
    {
        if (!StateMachine.CanInitialize)
        {
            Log.Error($"Can't initialize network in current state: {StateMachine.CurrentState}");
            return Error.AlreadyInUse;
        }
        
        Log.Info($"Starting server on port {port}");
        
        StateMachine.SetState(NetworkStateMachine.State.Hosting);
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(port, maxClients);
        peer.RefuseNewConnections = refuseNewConnections;
        Api.MultiplayerPeer = peer;

        if (error == Error.Ok)
        {
            StateMachine.SetState(NetworkStateMachine.State.Hosted);
            Log.Info("Started server successfully");
        }
        else
        {
            Log.Error($"Failed to start server: {error}");
        }
        
        return error;
    }

    public void OpenServer()
    {
        if (!StateMachine.IsServer)
        {
            Log.Error($"Can't open server in current state: {StateMachine.CurrentState}");
            return;
        }
        
        Api.MultiplayerPeer.RefuseNewConnections = false;
    }
    
    public override void _Notification(int id)
    {
        if (id == NotificationExitTree) Shutdown();
    }
    
    private void Shutdown()
    {
        if (Api.HasMultiplayerPeer() && Api.GetMultiplayerPeer() is not OfflineMultiplayerPeer)
        {
            Log.Info("Shutting down network...");

            Api.MultiplayerPeer.RefuseNewConnections = true;
            foreach (var peer in Api.GetPeers())
            {
                Api.MultiplayerPeer.DisconnectPeer(peer);
            }
            Api.MultiplayerPeer.Close();
            Api.MultiplayerPeer = new OfflineMultiplayerPeer();
            StateMachine.SetState(NetworkStateMachine.State.NotInitialized);
            
            Log.Info("Network shutdown successful");
        }
    }

    private void ConnectedToServerEvent()
    {
        StateMachine.SetState(NetworkStateMachine.State.Connected);
        Log.Info($"Connected to the server successfully. My peer id: {Api.GetUniqueId()}");
    }

    private void ConnectionFailedEvent()
    {
        StateMachine.SetState(NetworkStateMachine.State.Disconnected);
        Log.Error("Connection to the server failed");
        
        Shutdown();
    }

    private void ServerDisconnectedEvent()
    {
        StateMachine.SetState(NetworkStateMachine.State.Disconnected);
        Log.Info("Server disconnected");
        
        Shutdown();
    }
    
    private void PeerConnectedEvent(long id)
    {
        Log.Debug($"Network peer connected: {id}");
    }
    
    private void PeerDisconnectedEvent(long id)
    {
        Log.Debug($"Network peer disconnected: {id}");
    }
}