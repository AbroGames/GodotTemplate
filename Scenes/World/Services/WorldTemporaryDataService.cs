using Godot;
using KludgeBox.Godot.Nodes.MpSync;

namespace GodotTemplate.Scenes.World.Services;

public partial class WorldTemporaryDataService : Node
{
    /// <summary>
    /// Hoster nick, or nick from cmd param in dedicated server
    /// Player.IsAdmin in WorldPersistenceData for this player automatically will change to true
    /// If next application start will be with MainAdminNick = null, then Player.IsAdmin in WorldPersistenceData stay true anyway
    /// </summary>
    [Export] [Sync] public string MainAdminNick; 
    
    /// <summary>
    /// List of current connected players
    /// </summary>
    [Export] [Sync] public Godot.Collections.Dictionary<int, string> PlayerNickByPeerId = new();
    
    public override void _Ready()
    {
        this.AddChildWithName(new AttributeMultiplayerSynchronizer(this), "MultiplayerSynchronizer");
    }
    
    public void InitOnServer(string adminNickname = null)
    {
        MainAdminNick = adminNickname;
        GetMultiplayer().PeerDisconnected += id => PlayerNickByPeerId.Remove((int) id);
    }
}