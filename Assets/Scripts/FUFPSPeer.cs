// ILSpyBased#2
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Lite;
using System.Collections;
using System.Collections.Generic;

public class FUFPSPeer : LiteLobbyPeer
{
    public FUFPSPeer(IPhotonPeerListener listener)
        : base(listener)
    {
    }

    public override bool OpJoin(string gameName)
    {
        return this.OpJoin(gameName, null, null, false);
    }

    public virtual bool OpJoinFromLobby(string gameName, string lobbyName, Hashtable roomProperties, Hashtable actorProperties, bool broadcastActorProperties)
    {
        if ((int)base.DebugOut >= 5)
        {
            base.Listener.DebugReturn(DebugLevel.ALL, string.Format("OpJoin({0}/{1})", gameName, lobbyName));
        }
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary[255] = gameName;
        dictionary[242] = lobbyName;
        dictionary[248] = roomProperties;
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

    public virtual bool OpGetPropertiesOfGame(byte[] properties, string gameName, byte channelId)
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add(251, LitePropertyTypes.Game);
        if (properties != null)
        {
            dictionary.Add(248, properties);
            dictionary.Add(255, gameName);
        }
        return this.OpCustom(251, dictionary, true, channelId);
    }
}


