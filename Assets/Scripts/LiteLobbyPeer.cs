// ILSpyBased#2
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;

public class LiteLobbyPeer : LitePeer
{
    public enum LiteLobbyOpCode : byte
    {
        Join = 0xFF
    }

    public enum LiteLobbyOpKey : byte
    {
        RoomName = 0xFF,
        LobbyName = 242,
        ActorProperties = 249
    }

    public enum LiteLobbyEventKey : byte
    {
        RoomsArray = 245
    }

    public enum LiteLobbyEventCode : byte
    {
        RoomList = 252,
        RoomListUpdate = 251
    }

    public LiteLobbyPeer(IPhotonPeerListener listener)
        : base(listener)
    {
    }

    public virtual bool OpJoinFromLobby(string gameName, string lobbyName, Hashtable actorProperties, bool broadcastActorProperties)
    {
        if ((int)base.DebugOut >= 5)
        {
            base.Listener.DebugReturn(DebugLevel.ALL, string.Format("OpJoin({0}/{1})", gameName, lobbyName));
        }
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary[255] = gameName;
        dictionary[242] = lobbyName;
        if (actorProperties != null)
        {
            dictionary[249] = actorProperties;
            if (broadcastActorProperties)
            {
                dictionary[250] = broadcastActorProperties;
            }
        }
        return this.OpCustom(255, dictionary, true);
    }
}


