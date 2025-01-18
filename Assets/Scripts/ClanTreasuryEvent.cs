// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections.Generic;

public class ClanTreasuryEvent
{
    private int clanId;

    private int eventId;

    private ClanTreasuryEventType type;

    private int userId;

    private string userName = string.Empty;

    private int money;

    private string date = string.Empty;

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

    public ClanTreasuryEventType Type
    {
        get
        {
            return this.type;
        }
    }

    public int UserId
    {
        get
        {
            return this.userId;
        }
    }

    public string UserName
    {
        get
        {
            return this.userName;
        }
    }

    public int Money
    {
        get
        {
            return this.money;
        }
    }

    public string Date
    {
        get
        {
            return this.date;
        }
    }

    public ClanTreasuryEvent(JSONNode json)
    {
        this.eventId = json["i"].AsInt;
        this.clanId = json["cid"].AsInt;
        this.userId = json["uid"].AsInt;
        this.money = json["vcur"].AsInt;
        this.userName = json["un"].Value;
        this.type = (ClanTreasuryEventType)json["t"].AsInt;
        this.date = json["d"].Value;
        try
        {
            this.date = DateTime.Parse(this.date).ToString("d.MM.yyyy H:mm");
        }
        catch (Exception)
        {
            this.date = json["d"].Value;
        }
    }

    public ClanTreasuryEvent(int id, int clanId, int userId, string userName, int money, ClanTreasuryEventType type, string date)
    {
        this.eventId = id;
        this.clanId = clanId;
        this.userId = userId;
        this.userName = userName;
        this.money = money;
        this.type = type;
        this.date = date;
    }

    public static List<ClanTreasuryEvent> FromArray(JSONNode json)
    {
        List<ClanTreasuryEvent> list = new List<ClanTreasuryEvent>();
        foreach (JSONNode child in json.Childs)
        {
            list.Add(new ClanTreasuryEvent(child));
        }
        return list;
    }
}


