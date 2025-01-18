// ILSpyBased#2
using SimpleJSON;
using System;
using UnityEngine;

public class Enhancer : CCItem
{
    private uint e_id;

    private Duration duration;

    private bool isClan;

    private static Texture2D emptyTexture = new Texture2D(1, 1);

    public uint EnhancerID
    {
        get
        {
            return this.e_id;
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

    public bool IsClan
    {
        get
        {
            return this.isClan;
        }
        set
        {
            this.isClan = value;
        }
    }

    public Enhancer(JSONNode json)
    {
        if (json["e_id"] != (object)null)
        {
            this.e_id = Convert.ToUInt32(json["e_id"].AsInt);
            base.itemID = this.e_id;
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
        if (json["iC"] != (object)null)
        {
            this.isClan = ((byte)((json["iC"].AsInt != 0) ? 1 : 0) != 0);
        }
        base.name = "enhancer_" + this.EnhancerID + "_name";
        base.desc = "enhancer_" + this.EnhancerID + "_desc";
        base.descAdditional = "enhancer_" + this.EnhancerID + "_desca";
        base.icoFileString = string.Format("GUI/Icons/Enhancer/{0:000}", this.EnhancerID);
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

    public bool Equals(Enhancer p)
    {
        if (p == null)
        {
            return false;
        }
        return this.e_id == p.e_id;
    }

    public static Texture2D GetEmptyTexture()
    {
        return Enhancer.emptyTexture;
    }
}


