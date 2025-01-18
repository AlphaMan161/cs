// ILSpyBased#2
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanEvent
{
    private int clanId;

    private int eventId;

    private int createrId;

    private ClanEventType type;

    private Duration duration;

    private Hashtable data = new Hashtable();

    public int ClanId
    {
        get
        {
            return this.clanId;
        }
    }

    public int EventId
    {
        get
        {
            return this.eventId;
        }
    }

    public int CreaterId
    {
        get
        {
            return this.createrId;
        }
    }

    public ClanEventType Type
    {
        get
        {
            return this.type;
        }
    }

    public Duration Duration
    {
        get
        {
            return this.duration;
        }
        set
        {
            this.duration = value;
        }
    }

    public Hashtable Data
    {
        get
        {
            return this.data;
        }
    }

    public ClanEvent(JSONNode json)
    {
        this.eventId = json["i"].AsInt;
        this.clanId = json["cid"].AsInt;
        this.type = (ClanEventType)json["et"].AsInt;
        int num = json["exDa"].AsInt - ClanManager.ServerTime;
        this.duration = new Duration(num);
        if (json["creatid"] != (object)null)
        {
            this.createrId = json["creatid"].AsInt;
        }
        DurationManager.Add(this.duration, this);
        if (json["d"] != (object)null)
        {
            UnityEngine.Debug.LogError("CHECK this code: " + json["d"].Value);
            JSONNode jSONNode = JSONNode.Parse(json["d"].Value);
            foreach (string key in jSONNode.Keys)
            {
                JSONNode jSONNode2 = jSONNode[key];
                if (jSONNode2.AsInt != 0)
                {
                    this.data.Add(key, jSONNode2.AsInt);
                }
                else
                {
                    this.data.Add(key, jSONNode2.Value);
                }
            }
        }
    }

    public static List<ClanEvent> FromArray(JSONNode json)
    {
        List<ClanEvent> list = new List<ClanEvent>();
        foreach (JSONNode child in json.Childs)
        {
            list.Add(new ClanEvent(child));
        }
        return list;
    }
}


