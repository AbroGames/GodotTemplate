using Godot;
using KludgeBox.Godot.Nodes.MpSync;

namespace GodotTemplate.Scenes.World.Data.TemporaryData;

public partial class WorldTemporaryData : Node
{
    /// <summary>
    /// Hoster nick, or nick from cmd param in dedicated server.<br/>
    /// <c>Player.IsAdmin</c> in <c>WorldPersistenceData</c> for this player automatically will change to true.<br/>
    /// If next application start will be with <c>MainAdminNick = null</c>, then <c>Player.IsAdmin</c>
    /// in <c>WorldPersistenceData</c> change to false on this player connecting process.<br/>
    /// </summary>
    [Export] [Sync] public string MainAdminNick; 
    
    /// <summary>
    /// List of current connected players.
    /// </summary>
    [Export] [Sync] public Godot.Collections.Dictionary<long, string> PlayerNickByPeerId = new();
    
    public override void _Ready()
    {
        Di.Process(this);
    }
    
    //TODO Вынести этот метод отсюда, т.к. здесь просто хранилище + синк, без логики
    public void InitOnServer(string adminNickname = null)
    {
        MainAdminNick = adminNickname;
        GetMultiplayer().PeerDisconnected += id => PlayerNickByPeerId.Remove((int) id);
    }
}