using System.Security.Cryptography;
using System.Text;
using GodotTemplate.Scenes.World.Data;
using GodotTemplate.Scenes.World.Tree;
using Godot;
using KludgeBox.Core.Cooldown;

namespace GodotTemplate.Scenes.World.Services;

public partial class WorldStateCheckerService : Node
{

    private WorldTree _worldTree;
    private WorldPersistenceData _worldData;
    private AutoCooldown _checkCooldown;

    public void Init(WorldTree worldTree, WorldPersistenceData worldData)
    {
        _worldTree = worldTree;
        _worldData = worldData;
    }

    public void InitOnServer()
    {
        _checkCooldown = new AutoCooldown(5, true, StateCheckOnClients);
    }

    public override void _PhysicsProcess(double delta)
    {
        _checkCooldown?.Update(delta);
    }

    public void StateCheckRequest() => RpcId(ServerId, MethodName.StateCheckRequestRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void StateCheckRequestRpc()
    {
        StateCheckOnClients();
    }
    
    public void StateCheckOnClients() => StateCheckOnClients(GetWorldTreeHash(), GetWorldDataHash());
    private void StateCheckOnClients(string serverWorldTreeHash, string serverWorldDataHash) => 
        Rpc(MethodName.StateCheckOnClientRpc, serverWorldTreeHash, serverWorldDataHash);
    [Rpc(CallLocal = false)]
    private void StateCheckOnClientRpc(string serverWorldTreeHash, string serverWorldDataHash)
    {
        if (GetWorldTreeHash() != serverWorldTreeHash || GetWorldDataHash() != serverWorldDataHash)
        {
            NotifyServerAboutInconsistentState(GetWorldTreeHash(), GetWorldDataHash());
        }
    }

    private void NotifyServerAboutInconsistentState(string clientWorldTree, string clientWorldDataHash) => 
        RpcId(ServerId, MethodName.NotifyServerAboutInconsistentStateRpc, clientWorldTree, clientWorldDataHash);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false)]
    private void NotifyServerAboutInconsistentStateRpc(string clientWorldTree, string clientWorldDataHash)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Client has inconsistent state (peer id = {GetMultiplayer().GetRemoteSenderId()})");

        if (!GetWorldTreeHash().Equals(clientWorldTree))
        {
            sb.AppendLine("Server world tree: " + GetWorldTreeHash());
            sb.AppendLine("Client world tree: " + clientWorldTree);
        }
        
        if (!GetWorldDataHash().Equals(clientWorldDataHash))
        {
            sb.AppendLine("Server world data: " + GetWorldDataHash());
            sb.AppendLine("Client world data: " + clientWorldDataHash);
        }
        
        Log.Warning(sb.ToString());
    }
    
    /// <summary>
    /// Log full Tree and Data infos.
    /// For Debug only.
    /// </summary>
    public void LogState() => Rpc(MethodName.LogStateRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void LogStateRpc()
    {
        Log.Debug("World tree: " + GetWorldTree());
        Log.Debug("World tree hash: " + GetWorldTreeHash());
        Log.Debug("World data hash: " + GetWorldDataHash());
    }
    
    private string GetWorldTree()
    {
        return GetFullTree(_worldTree);
    }

    private string GetWorldTreeHash()
    {
        return Hash(GetWorldTree());
    }

    private string GetWorldDataHash()
    {
        return Hash(_worldData.Serializer.SerializeWorldData());
    }
    
    /// <summary>
    /// Get full path for all children of this node
    /// It works only for server nodes!
    /// It means nodes with MultiplayerAuthority != ServerId (and their children) will not be display in tree
    /// Can be used for compare Client/Server trees in debug
    /// </summary>
    private string GetFullTree(Node node)
    {
        if (node.GetMultiplayerAuthority() != ServerId) return "";
        
        StringBuilder sb = new();
        sb.AppendLine(); 
        sb.Append(node.GetPath());
        foreach (var child in node.GetChildren()) 
        {
            sb.Append(GetFullTree(child));
        }
        return sb.ToString();
    }
    
    private string Hash(string inputString)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
        return Hash(inputBytes);
    }

    private string Hash(byte[] inputBytes)
    {
        byte[] hashBytes = MD5.HashData(inputBytes);

        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}