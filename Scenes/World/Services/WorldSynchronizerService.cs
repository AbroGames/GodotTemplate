using System;
using Godot;
using GodotTemplate.Scenes.Game;
using GodotTemplate.Scenes.World.Data.PersistenceData;
using GodotTemplate.Scenes.World.Data.PersistenceData.Player;
using GodotTemplate.Scenes.World.Data.TemporaryData;
using GodotTemplate.Scenes.World.Services.DataSerializer;
using GodotTemplate.Scripts.Service.Settings;
using KludgeBox.DI.Requests.LoggerInjection;
using KludgeBox.DI.Requests.SceneServiceInjection;
using Serilog;

namespace GodotTemplate.Scenes.World.Services;

/// <summary>
/// Use for send player data (like nick, color etc.) to server.<br/>
/// Use in multiplayer and singleplayer games for init player info.
/// </summary>
public partial class WorldSynchronizerService : Node
{
    private const int NicknameMinLength = 3;
    private const int NicknameMaxLength = 25;
    private static readonly string LengthOfNicknameErrorMessage = $"Length of nickname must be between {NicknameMinLength} and {NicknameMaxLength} characters";
    private const string NicknameAlreadyUsedErrorMessage = "Nickname is already used";
    
    public event Action SyncStartedOnClientEvent;
    public event Action<int> SyncEndedOnServerEvent;
    public event Action SyncEndedOnClientEvent;
    public event Action<string> SyncRejectOnServerEvent;
    public event Action<string> SyncRejectOnClientEvent;

    [SceneService] private WorldPersistenceData _persistenceData;
    [SceneService] private WorldTemporaryData _temporaryData;
    [SceneService] private WorldDataSerializerService _dataSerializerService;
    [Logger] private ILogger _log;

    public override void _Ready()
    {
        Di.Process(this);
    }

    public void StartSyncOnClient(string nick, Color color)
    {
        _log.Information("Starting sync with server with nick '{nick}'", nick);
        SyncStartedOnClientEvent?.Invoke();
        NewClientInitOnServer(nick, color);
    }

    private void NewClientInitOnServer(string nick, Color color) => RpcId(ServerId, MethodName.NewClientInitOnServerRpc, nick, color);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)] 
    private void NewClientInitOnServerRpc(string nick, Color color)
    {
        int connectedClientId = GetMultiplayer().GetRemoteSenderId();
        _log.Information("Peer {peer} try sync with server with nick '{nick}'", connectedClientId, nick);

        if (_temporaryData.PlayerNickByPeerId.Values.Contains(nick))
        {
            _log.Warning("Syncing peer {peer} was rejected with error: {error}", connectedClientId, NicknameAlreadyUsedErrorMessage);
            SyncRejectOnServerEvent?.Invoke(NicknameAlreadyUsedErrorMessage);
            RejectSyncOnClient(connectedClientId, NicknameAlreadyUsedErrorMessage);
        }
        if (nick.Length < NicknameMinLength || nick.Length > NicknameMaxLength)
        {
            _log.Warning("Syncing peer {peer} was rejected with error: {error}", connectedClientId, LengthOfNicknameErrorMessage);
            SyncRejectOnServerEvent?.Invoke(LengthOfNicknameErrorMessage);
            RejectSyncOnClient(connectedClientId, LengthOfNicknameErrorMessage);
        }
        _temporaryData.PlayerNickByPeerId.Add(connectedClientId, nick);

        if (!_persistenceData.Players.PlayerByNick.ContainsKey(nick))
        {
            _log.Information("Add peer {peer} as new player", connectedClientId);
            _persistenceData.Players.AddPlayer(new PlayerData
            {
                Nick = nick
            });
        }
        PlayerData playerData = _persistenceData.Players.PlayerByNick[nick];
        playerData.Color = color;
        playerData.IsAdmin = nick.Equals(_temporaryData.MainAdminNick);
        _log.Information("Player data about peer {peer} was synced ", connectedClientId);
        
        _log.Information("Send to peer {peer} serialized world data", connectedClientId);
        EndSyncOnClient(connectedClientId, _dataSerializerService.SerializeWorldData());
        SyncEndedOnServerEvent?.Invoke(connectedClientId);
    }

    private void EndSyncOnClient(long peerId, byte[] serializableData) => RpcId(peerId, MethodName.EndSyncOnClientRpc, serializableData);
    [Rpc(CallLocal = true)]
    private void EndSyncOnClientRpc(byte[] serializableData)
    {
        _log.Information("Received serialized world data from server");
        _dataSerializerService.DeserializeWorldData(serializableData);
        SyncEndedOnClientEvent?.Invoke();
    }
    
    private void RejectSyncOnClient(long peerId, string errorMessage) => RpcId(peerId, MethodName.RejectSyncOnClientRpc, errorMessage);
    [Rpc(CallLocal = true)] 
    private void RejectSyncOnClientRpc(string errorMessage)
    {
        _log.Error("Synchronization with the server was rejected: {error}", errorMessage);
        SyncRejectOnClientEvent?.Invoke(errorMessage);
    }
}