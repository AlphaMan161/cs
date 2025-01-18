// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections;

public class ClanMember : Player
{
    private short memberLevel;

    private uint money;

    private uint clanExp;

    private ushort clanExpKoef;

    private uint exp;

    private string date = string.Empty;

    public short MemberLevel
    {
        get
        {
            return this.memberLevel;
        }
    }

    public uint Money
    {
        get
        {
            return this.money;
        }
    }

    public uint ClanExp
    {
        get
        {
            return this.clanExp;
        }
    }

    public ushort ClanExpKoef
    {
        get
        {
            return this.clanExpKoef;
        }
        set
        {
            this.clanExpKoef = value;
        }
    }

    public uint Exp
    {
        get
        {
            return this.exp;
        }
    }

    public string Date
    {
        get
        {
            return this.date;
        }
    }

    public ClanMember(JSONNode json)
    {
        base.user_id = 0;
        base.level = 0;
        base.name = string.Empty;
        base.isPremium = false;
        if (json["uid"] != (object)null)
        {
            base.user_id = json["uid"].AsInt;
        }
        if (json["ul"] != (object)null)
        {
            base.level = Convert.ToInt16(json["ul"].AsInt);
        }
        if (json["n"] != (object)null)
        {
            base.name = json["n"].Value;
        }
        if (json["mlvl"] != (object)null)
        {
            this.memberLevel = Convert.ToInt16(json["mlvl"].AsInt);
        }
        if (json["m"] != (object)null)
        {
            this.money = Convert.ToUInt32(json["m"].AsInt);
        }
        if (json["e"] != (object)null)
        {
            this.clanExp = Convert.ToUInt32(json["e"].AsInt);
        }
        if (json["ek"] != (object)null)
        {
            this.clanExpKoef = Convert.ToUInt16(json["ek"].AsInt);
        }
        if (json["ue"] != (object)null)
        {
            this.exp = Convert.ToUInt32(json["ue"].AsInt);
        }
        if (json["date"] != (object)null)
        {
            this.date = json["date"].Value;
        }
    }

    public ClanMember(Hashtable obj)
    {
        base.user_id = 0;
        base.level = 0;
        base.name = string.Empty;
        base.isPremium = false;
        base.user_id = (int)obj["uid"];
        base.level = (short)obj["ul"];
        base.name = obj["n"].ToString();
        this.memberLevel = (short)obj["mlvl"];
        this.money = Convert.ToUInt32(obj["m"]);
        this.clanExp = Convert.ToUInt32(obj["e"]);
        this.clanExpKoef = Convert.ToUInt16(obj["ek"]);
        this.exp = Convert.ToUInt32(obj["ue"]);
        this.date = obj["date"].ToString();
    }

    public Hashtable ToHashtable()
    {
        Hashtable hashtable = new Hashtable();
        hashtable["uid"] = base.user_id;
        hashtable["ul"] = base.level;
        hashtable["n"] = base.name;
        hashtable["mlvl"] = this.memberLevel;
        hashtable["m"] = (int)this.money;
        hashtable["e"] = (int)this.clanExp;
        hashtable["ek"] = (short)this.clanExpKoef;
        hashtable["ue"] = (int)this.exp;
        hashtable["date"] = this.date;
        return hashtable;
    }
}


