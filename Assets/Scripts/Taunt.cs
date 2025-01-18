// ILSpyBased#2
using SimpleJSON;
using System;
using UnityEngine;

public class Taunt : CCItem
{
    private short t_id;

    private Duration duration;

    private static Texture2D emptyTexture = new Texture2D(1, 1);

    public short TauntID
    {
        get
        {
            return this.t_id;
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

    public Taunt(JSONNode json)
    {
        if (json["t_id"] != (object)null)
        {
            this.t_id = Convert.ToInt16(json["t_id"].AsInt);
            base.itemID = Convert.ToUInt32(this.t_id);
        }
        if (json["sname"] != (object)null)
        {
            base.sname = json["sname"];
        }
        if (json["sn"] != (object)null)
        {
            base.sname = json["sn"];
        }
        if (json["nlvl"] != (object)null)
        {
            base.nlvl = Convert.ToUInt32(json["nlvl"].AsInt);
        }
        if (json["iS"] != (object)null)
        {
            base.isSale = ((byte)((json["iS"].AsInt != 0) ? 1 : 0) != 0);
        }
        base.name = "taunt_" + this.TauntID + "_name";
        base.desc = "taunt_" + this.TauntID + "_desc";
        base.descAdditional = "taunt_" + this.TauntID + "_desca";
        base.icoFileString = string.Format("GUI/Icons/Taunt/{0}", this.TauntID);
        if (json["sc"] != (object)null)
        {
            base.shop_cost = new ShopCost(json["sc"]);
        }
        if (json["eD"] != (object)null)
        {
            long num = Convert.ToInt64(json["eD"].AsInt);
            if (num != 0L)
            {
                this.duration = new Duration(num - Inventory.Instance.ServerTime);
            }
        }
        if (this.duration != null)
        {
            DurationManager.Add(this.duration, this);
        }
    }

    public bool Equals(Taunt p)
    {
        if (p == null)
        {
            return false;
        }
        return this.t_id == p.t_id;
    }

    public static Texture2D GetEmptyTexture()
    {
        return Taunt.emptyTexture;
    }
}


