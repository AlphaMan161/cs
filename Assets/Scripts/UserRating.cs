// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserRating
{
    private string place = string.Empty;

    private int league = -1;

    private int user_id;

    private string name = string.Empty;

    private string filteredName = string.Empty;

    private int lvl;

    private int clan_id;

    private string clan_name = string.Empty;

    private int clan_arm_id;

    private uint exp;

    private int kill;

    private int death;

    private float kd = 1f;

    private int ach;

    private bool show = true;

    private int ptime;

    private int domination;

    private int flag;

    private int control_point;

    private int nuts;

    private int head;

    private int assist;

    public string Place
    {
        get
        {
            return this.place;
        }
        set
        {
            this.place = value;
        }
    }

    public int League
    {
        get
        {
            return this.league;
        }
        set
        {
            this.league = value;
        }
    }

    public int UserID
    {
        get
        {
            return this.user_id;
        }
    }

    public string Name
    {
        get
        {
            return this.name;
        }
    }

    public string FilteredName
    {
        get
        {
            return this.filteredName;
        }
    }

    public int Level
    {
        get
        {
            return this.lvl;
        }
    }

    public int ClanID
    {
        get
        {
            return this.clan_id;
        }
    }

    public string ClanName
    {
        get
        {
            return this.clan_name;
        }
    }

    public int ClanArmId
    {
        get
        {
            return this.clan_arm_id;
        }
    }

    public uint Exp
    {
        get
        {
            return this.exp;
        }
        set
        {
            this.exp = value;
        }
    }

    public int Kill
    {
        get
        {
            return this.kill;
        }
    }

    public int Death
    {
        get
        {
            return this.death;
        }
    }

    public float KD
    {
        get
        {
            return this.kd;
        }
    }

    public int Achievemnt
    {
        get
        {
            return this.ach;
        }
    }

    public bool Show
    {
        get
        {
            return this.show;
        }
        set
        {
            this.show = value;
        }
    }

    public int PlayedTime
    {
        get
        {
            return this.ptime;
        }
    }

    public int Domination
    {
        get
        {
            return this.domination;
        }
    }

    public int Flag
    {
        get
        {
            return this.flag;
        }
    }

    public int ControlPoint
    {
        get
        {
            return this.control_point;
        }
    }

    public int Nuts
    {
        get
        {
            return this.nuts;
        }
    }

    public int Head
    {
        get
        {
            return this.head;
        }
    }

    public int Assist
    {
        get
        {
            return this.assist;
        }
    }

    public UserRating(JSONNode obj)
    {
        if (obj["pos"] != (object)null)
        {
            this.place = obj["pos"].Value;
        }
        if (obj["name"] != (object)null)
        {
            this.name = BadWorldFilter.CheckLite(obj["name"].Value);
        }
        else if (obj["n"] != (object)null)
        {
            this.name = BadWorldFilter.CheckLite(obj["n"].Value);
        }
        else if (obj["un"] != (object)null)
        {
            this.name = BadWorldFilter.CheckLite(obj["un"].Value);
        }
        if (obj["lvl"] != (object)null)
        {
            this.lvl = obj["lvl"].AsInt;
        }
        else if (obj["l"] != (object)null)
        {
            this.lvl = obj["l"].AsInt;
        }
        if (obj["clan_id"] != (object)null)
        {
            this.clan_id = obj["clan_id"].AsInt;
        }
        else if (obj["ci"] != (object)null)
        {
            this.clan_id = obj["ci"].AsInt;
        }
        if (obj["clan_name"] != (object)null)
        {
            this.clan_name = obj["clan_name"].Value;
        }
        else if (obj["ca"] != (object)null)
        {
            this.clan_name = obj["ca"].Value;
        }
        else if (obj["ctag"] != (object)null)
        {
            this.clan_name = obj["ctag"].Value;
        }
        if (obj["caid"] != (object)null)
        {
            this.clan_arm_id = obj["caid"].AsInt;
        }
        if (obj["exp"] != (object)null)
        {
            this.exp = Convert.ToUInt32(obj["exp"].Value);
        }
        else if (obj["e"] != (object)null)
        {
            this.exp = Convert.ToUInt32(obj["e"].Value);
        }
        if (obj["kill"] != (object)null)
        {
            this.kill = obj["kill"].AsInt;
        }
        else if (obj["k"] != (object)null)
        {
            this.kill = obj["k"].AsInt;
        }
        if (obj["f"] != (object)null)
        {
            this.flag = obj["f"].AsInt;
        }
        if (obj["p"] != (object)null)
        {
            this.control_point = obj["p"].AsInt;
        }
        if (obj["do"] != (object)null)
        {
            this.domination = obj["do"].AsInt;
        }
        if (obj["nu"] != (object)null)
        {
            this.nuts = obj["nu"].AsInt;
        }
        if (obj["h"] != (object)null)
        {
            this.head = obj["h"].AsInt;
        }
        if (obj["a"] != (object)null)
        {
            this.assist = obj["a"].AsInt;
        }
        if (obj["death"] != (object)null)
        {
            this.death = obj["death"].AsInt;
        }
        else if (obj["d"] != (object)null)
        {
            this.death = obj["d"].AsInt;
        }
        if (obj["kd"] != (object)null)
        {
            this.kd = (float)obj["kd"].AsInt / 1000f;
        }
        if (obj["ach"] != (object)null)
        {
            this.ach = obj["ach"].AsInt;
        }
        if (obj["ptime"] != (object)null)
        {
            this.ptime = obj["ptime"].AsInt;
        }
        if (obj["id"] != (object)null)
        {
            this.user_id = obj["id"].AsInt;
        }
        else if (obj["i"] != (object)null)
        {
            this.user_id = obj["i"].AsInt;
        }
        else if (obj["uid"] != (object)null)
        {
            this.user_id = obj["uid"].AsInt;
        }
        this.filteredName = BadWorldFilter.CheckLite(this.name);
    }

    public static List<UserRating> UserRaringFromList(JSONNode list)
    {
        List<UserRating> list2 = new List<UserRating>();
        UserRating userRating = null;
        int num = 0;
        foreach (JSONNode child in list.Childs)
        {
            userRating = new UserRating(child);
            userRating.Place = (num + 1).ToString();
            list2.Add(userRating);
            num++;
        }
        return list2;
    }

    public void AddFromDictionary(Dictionary<byte, object> uStatRating)
    {
        if (uStatRating.ContainsKey(31))
        {
            UnityEngine.Debug.Log(string.Format("uStatRating[FUFPSUserSaveKeys.uStatRating_ExpBef] = {0} type:{1}", uStatRating[31], uStatRating[31].GetType()));
        }
        if (uStatRating.ContainsKey(32))
        {
            UnityEngine.Debug.Log(string.Format("uStatRating[FUFPSUserSaveKeys.uStatRating_Lvl] = {0} type:{1}", uStatRating[32], uStatRating[32].GetType()));
            this.lvl += Convert.ToInt32(uStatRating[32]);
        }
        if (uStatRating.ContainsKey(33))
        {
            UnityEngine.Debug.Log(string.Format("uStatRating[FUFPSUserSaveKeys.uStatRating_Exp] = {0} type:{1}", uStatRating[33], uStatRating[33].GetType()));
            this.exp += Convert.ToUInt32(uStatRating[33]);
        }
        if (uStatRating.ContainsKey(34))
        {
            UnityEngine.Debug.Log(string.Format("uStatRating[FUFPSUserSaveKeys.uStatRating_Kill] = {0} type:{1}", uStatRating[34], uStatRating[34].GetType()));
            this.kill += Convert.ToInt32(uStatRating[34]);
        }
        if (uStatRating.ContainsKey(35))
        {
            UnityEngine.Debug.Log(string.Format("uStatRating[FUFPSUserSaveKeys.uStatRating_Death] = {0} type:{1}", uStatRating[35], uStatRating[35].GetType()));
            this.death += Convert.ToInt32(uStatRating[35]);
        }
        if (uStatRating.ContainsKey(36))
        {
            UnityEngine.Debug.Log(string.Format("uStatRating[FUFPSUserSaveKeys.uStatRatinh_PTime] = {0} type:{1}", uStatRating[36], uStatRating[36].GetType()));
            this.ptime += Convert.ToInt32(uStatRating[36]);
        }
        if (this.death > 0)
        {
            this.kd = Convert.ToSingle(this.kill) / Convert.ToSingle(this.death);
        }
        this.kd = ((this.death >= 1) ? (Convert.ToSingle(this.kill) / (float)this.death) : ((float)this.kill));
    }
}


