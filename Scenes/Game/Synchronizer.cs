using System;
using Godot;
using GodotTemplate.Scenes.World.Data.PersistenceData.Player;
using GodotTemplate.Scripts.Service.Settings;
using KludgeBox.DI.Requests.LoggerInjection;
using Serilog;

namespace GodotTemplate.Scenes.Game;

/// <summary>
/// Use for send player data (like nick, color etc.) to server.<br/>
/// We must use synchronizer out of <c>World</c>, because in connecting process <c>World</c> children nodes don't exist.<br/>
/// But we need in synchronizer at connecting process.
/// </summary>
public partial class Synchronizer : Node
{

    private const int NicknameMinLength = 3;
    private const int NicknameMaxLength = 25;
    private static readonly string LengthOfNicknameErrorMessage = $"Length of nickname must be between {NicknameMinLength} and {NicknameMaxLength} characters";
    private const string NicknameAlreadyUsedErrorMessage = "Nickname is already used";
    
    public Action SyncStartedOnClientEvent;
    public Action<int> SyncEndedOnServerEvent;
    public Action SyncEndedOnClientEvent;
    public Action<string> SyncRejectOnServerEvent;
    public Action<string> SyncRejectOnClientEvent;
    
    private World.World _world;
    private PlayerSettings _playerSettings;
    [Logger] private ILogger _log;
    
    public Synchronizer InitPreReady(World.World world, PlayerSettings playerSettings)
    {
        Di.Process(this);
        
        if (world == null) _log.Error("World must be not null");
        _world = world;
        
        if (playerSettings == null) _log.Error("PlayerSettings must be not null");
        _playerSettings = playerSettings;

        return this;
    }

    public void StartSyncOnClient()
    {
        SyncStartedOnClientEvent.Invoke();
        NewClientInitOnServer(_playerSettings.Nick, _playerSettings.Color);
    }

    private void NewClientInitOnServer(string nick, Color color) => RpcId(ServerId, MethodName.NewClientInitOnServerRpc, nick, color);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)] 
    private void NewClientInitOnServerRpc(string nick, Color color)
    {
        int connectedClientId = GetMultiplayer().GetRemoteSenderId();

        if (_world.TemporaryData.PlayerNickByPeerId.Values.Contains(nick))
        {
            SyncRejectOnServerEvent.Invoke(NicknameAlreadyUsedErrorMessage);
            RejectSyncOnClient(connectedClientId, NicknameAlreadyUsedErrorMessage);
        }
        if (nick.Length < NicknameMinLength || nick.Length > NicknameMaxLength)
        {
            SyncRejectOnServerEvent.Invoke(LengthOfNicknameErrorMessage);
            RejectSyncOnClient(connectedClientId, LengthOfNicknameErrorMessage);
        }
        _world.TemporaryData.PlayerNickByPeerId.Add(connectedClientId, nick);

        if (!_world.PersistenceData.Players.PlayerByNick.ContainsKey(nick))
        {
            _world.PersistenceData.Players.AddPlayer(new PlayerData
            {
                Nick = nick
            });
        }
        PlayerData playerData = _world.PersistenceData.Players.PlayerByNick[nick];
        playerData.Color = color;
        playerData.IsAdmin = nick.Equals(_world.TemporaryData.MainAdminNick);

        EndSyncOnClient(connectedClientId, _world.DataSerializerService.SerializeWorldData());
        SyncEndedOnServerEvent.Invoke(connectedClientId);
    }

    private void EndSyncOnClient(long peerId, byte[] serializableData) => RpcId(peerId, MethodName.EndSyncOnClientRpc, serializableData);
    [Rpc(CallLocal = true)]
    private void EndSyncOnClientRpc(byte[] serializableData)
    {
        _world.DataSerializerService.DeserializeWorldData(serializableData);
        SyncEndedOnClientEvent.Invoke();
    }
    
    private void RejectSyncOnClient(long peerId, string errorMessage) => RpcId(peerId, MethodName.RejectSyncOnClientRpc, errorMessage);
    [Rpc(CallLocal = true)] 
    private void RejectSyncOnClientRpc(string errorMessage)
    {
        SyncRejectOnClientEvent.Invoke(errorMessage);
    }
}