// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class Wear : CCItem
{
    private uint w_id;

    private CCWearType wt = CCWearType.Others;

    private bool showOnCamera = true;

    private string modelSystemName;

    private Assemblage assemblage;

    public uint WearID
    {
        get
        {
            return this.w_id;
        }
    }

    public CCWearType WearType
    {
        get
        {
            return this.wt;
        }
    }

    public bool ShowOnCamera
    {
        get
        {
            return this.showOnCamera;
        }
        set
        {
            this.showOnCamera = value;
        }
    }

    public string ModelSystemName
    {
        get
        {
            return this.modelSystemName;
        }
    }

    public Assemblage AssemblageRel
    {
        get
        {
            return this.assemblage;
        }
    }

    public Wear(JSONObject obj)
    {
        if (obj.type != JSONObject.Type.OBJECT)
        {
            throw new Exception("[Wear] Input object not a valid data");
        }
        if (obj.GetField("w_id") != null)
        {
            this.w_id = Convert.ToUInt32(obj.GetField("w_id").n);
        }
        else if ((bool)obj.GetField("id"))
        {
            if (obj.GetField("id").type == JSONObject.Type.NUMBER)
            {
                this.w_id = Convert.ToUInt32(obj.GetField("id").n);
            }
            else
            {
                this.w_id = Convert.ToUInt32(obj.GetField("id").str);
            }
        }
        base.itemID = this.w_id;
        if (obj.GetField("wt") != null)
        {
            if (obj.GetField("wt").type == JSONObject.Type.NUMBER)
            {
                this.wt = (CCWearType)Convert.ToInt32(obj.GetField("wt").n);
            }
            else
            {
                this.wt = (CCWearType)Convert.ToInt32(obj.GetField("wt").str);
            }
        }
        if (obj.GetField("sname") != null)
        {
            base.sname = obj.GetField("sname").str;
        }
        else if (obj.GetField("sn") != null)
        {
            base.sname = obj.GetField("sn").str;
        }
        if (obj.GetField("nlvl") != null)
        {
            base.nlvl = Convert.ToUInt32(obj.GetField("nlvl").n);
        }
        if (obj.GetField("iS") != null)
        {
            if (obj.GetField("iS").type == JSONObject.Type.NUMBER)
            {
                base.isSale = ((byte)((Convert.ToUInt32(obj.GetField("iS").n) == 1) ? 1 : 0) != 0);
            }
            else
            {
                base.isSale = ((byte)((Convert.ToUInt32(obj.GetField("iS").str) == 1) ? 1 : 0) != 0);
            }
        }
        base.name = "wear_" + this.WearType.ToString() + "_" + base.SystemName + "_name";
        base.desc = "wear_" + this.WearType.ToString() + "_" + base.SystemName + "_desc";
        base.descAdditional = "wear_" + this.WearType.ToString() + "_" + base.SystemName + "_desca";
        this.modelSystemName = Wear.GetModelName(this.WearType, base.SystemName);
        base.icoFileString = string.Format("GUI/Icons/Items/{0}/{1}", this.WearType.ToString(), base.SystemName);
        if (obj.GetField("sc") != null)
        {
            base.shop_cost = new ShopCost(obj.GetField("sc"));
        }
        this.assemblage = AssemblageManager.GetAssemblage(this);
    }

    public static string GetModelName(CCWearType type, string system_name)
    {
        return string.Format("{0}_{1}", type.ToString(), system_name);
    }

    public static List<Wear> WearFromList(JSONObject list)
    {
        if (list.type != JSONObject.Type.ARRAY)
        {
            throw new Exception("[Wear] WeaponFromList: Input object not a array");
        }
        List<Wear> list2 = new List<Wear>();
        for (int i = 0; i < list.Count; i++)
        {
            list2.Add(new Wear(list[i]));
        }
        return list2;
    }

    public static Texture2D GetEmptyTexture(CCWearType type)
    {
        switch (type)
        {
            case CCWearType.Masks:
                return (Texture2D)Resources.Load("GUI/Icons/Items/Empty/empty_slot_mask");
            case CCWearType.Backpacks:
                return (Texture2D)Resources.Load("GUI/Icons/Items/Empty/empty_slot_backpack");
            case CCWearType.Others:
                return (Texture2D)Resources.Load("GUI/Icons/Items/Empty/empty_slot_other");
            case CCWearType.Boots:
                return (Texture2D)Resources.Load("GUI/Icons/Items/Empty/empty_slot_boots");
            case CCWearType.Pants:
                return (Texture2D)Resources.Load("GUI/Icons/Items/Empty/empty_slot_pants");
            case CCWearType.Gloves:
                return (Texture2D)Resources.Load("GUI/Icons/Items/Empty/empty_slot_gloves");
            case CCWearType.Shirts:
                return (Texture2D)Resources.Load("GUI/Icons/Items/Empty/empty_slot_shirt");
            case CCWearType.Hats:
                return (Texture2D)Resources.Load("GUI/Icons/Items/Empty/empty_slot_hat");
            case CCWearType.Heads:
                return (Texture2D)Resources.Load("GUI/Icons/Items/Empty/empty_slot_head");
            default:
                return null;
        }
    }

    public bool Equals(Wear p)
    {
        if (p == null)
        {
            return false;
        }
        return this.w_id == p.w_id;
    }
}


