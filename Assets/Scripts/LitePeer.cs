// ILSpyBased#2
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Lite;
using System.Collections;
using System.Collections.Generic;

public class LitePeer : PhotonPeer
{
    public LitePeer(IPhotonPeerListener listener)
        : base(listener, false)
    {
    }

    public LitePeer(IPhotonPeerListener listener, bool useTcp)
        : base(listener, useTcp)
    {
    }

    public bool OpRaiseEvent(byte eventCode, Hashtable evData, bool sendReliable)
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary[245] = evData;
        dictionary[244] = eventCode;
        return this.OpCustom(253, dictionary, sendReliable, 0);
    }

    public virtual bool OpSetPropertiesOfActor(int actorNr, Hashtable properties, bool broadcast, byte channelId)
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add(251, properties);
        dictionary.Add(254, actorNr);
        if (broadcast)
        {
            dictionary.Add(250, broadcast);
        }
        return this.OpCustom(252, dictionary, true, channelId);
    }

    public virtual bool OpSetPropertiesOfGame(Hashtable properties, bool broadcast, byte channelId)
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add(251, properties);
        if (broadcast)
        {
            dictionary.Add(250, broadcast);
        }
        return this.OpCustom(252, dictionary, true, channelId);
    }

    public virtual bool OpGetProperties(byte channelId)
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add(251, (byte)3);
        return this.OpCustom(251, dictionary, true, channelId);
    }

    public virtual bool OpGetPropertiesOfActor(int[] actorNrList, string[] properties, byte channelId)
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add(251, LitePropertyTypes.Actor);
        if (properties != null)
        {
            dictionary.Add(249, properties);
        }
        if (actorNrList != null)
        {
            dictionary.Add(254, actorNrList);
        }
        return this.OpCustom(251, dictionary, true, channelId);
    }

    public virtual bool OpGetPropertiesOfActor(int[] actorNrList, byte[] properties, byte channelId)
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add(251, LitePropertyTypes.Actor);
        if (properties != null)
        {
            dictionary.Add(249, properties);
        }
        if (actorNrList != null)
        {
            dictionary.Add(254, actorNrList);
        }
        return this.OpCustom(251, dictionary, true, channelId);
    }

    public virtual bool OpGetPropertiesOfGame(string[] properties, byte channelId)
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add(251, LitePropertyTypes.Game);
        if (properties != null)
        {
            dictionary.Add(248, properties);
        }
        return this.OpCustom(251, dictionary, true, channelId);
    }

    public virtual bool OpGetPropertiesOfGame(byte[] properties, byte channelId)
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add(251, LitePropertyTypes.Game);
        if (properties != null)
        {
            dictionary.Add(248, properties);
        }
        return this.OpCustom(251, dictionary, true, channelId);
    }

    public virtual bool OpJoin(string gameName)
    {
        return this.OpJoin(gameName, null, null, false);
    }

    public virtual bool OpJoin(string gameName, Hashtable gameProperties, Hashtable actorProperties, bool broadcastActorProperties)
    {
        if ((int)base.DebugOut >= 5)
        {
            base.Listener.DebugReturn(DebugLevel.ALL, "OpJoin(" + gameName + ")");
        }
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary[255] = gameName;
        if (actorProperties != null)
        {
            dictionary[249] = actorProperties;
        }
        if (gameProperties != null)
        {
            dictionary[248] = gameProperties;
        }
        if (broadcastActorProperties)
        {
            dictionary[250] = broadcastActorProperties;
        }
        return this.OpCustom(255, dictionary, true, 0);
    }

    public virtual bool OpLeave(string gameName)
    {
        if ((int)base.DebugOut >= 5)
        {
            base.Listener.DebugReturn(DebugLevel.ALL, "OpLeave()");
        }
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary[255] = gameName;
        return this.OpCustom(254, dictionary, true, 0);
    }
}


