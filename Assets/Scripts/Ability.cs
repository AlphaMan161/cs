// ILSpyBased#2
using System;
using UnityEngine;

public class Ability
{
    private uint ability_id;

    private short level;

    private string name = string.Empty;

    private string desc = string.Empty;

    private string icoFileString = string.Empty;

    private Texture2D ico;

    private ShopCost cost;

    private bool isBuyed;

    private string values = string.Empty;

    public Ability NextLevel;

    public uint AbilityID
    {
        get
        {
            return this.ability_id;
        }
    }

    public short Level
    {
        get
        {
            return this.level;
        }
    }

    public string Name
    {
        get
        {
            return LanguageManager.GetText(this.name);
        }
    }

    public string Description
    {
        get
        {
            return LanguageManager.GetText(this.desc);
        }
    }

    public Texture2D Ico
    {
        get
        {
            if ((UnityEngine.Object)this.ico == (UnityEngine.Object)null && this.icoFileString != string.Empty)
            {
                this.ico = (Texture2D)Resources.Load(this.icoFileString);
            }
            return this.ico;
        }
    }

    public ShopCost Cost
    {
        get
        {
            return this.cost;
        }
    }

    public bool IsBuyed
    {
        get
        {
            return this.isBuyed;
        }
        set
        {
            this.isBuyed = value;
        }
    }

    public string Values
    {
        get
        {
            return this.values;
        }
    }

    public Ability(JSONObject obj)
    {
        if (obj.type != JSONObject.Type.OBJECT)
        {
            throw new Exception("[Ability] Input object not a valid data");
        }
        this.ability_id = Convert.ToUInt32(obj.GetField("i").n);
        this.level = Convert.ToInt16(obj.GetField("l").n);
        if (obj["v"] != null)
        {
            this.values = obj["v"].str;
        }
        this.name = "ability_name_" + this.ability_id;
        this.desc = "ability_desc_" + this.ability_id + "_" + this.Level;
        this.icoFileString = "GUI/Icons/Ability/ability_" + this.ability_id;
        if (obj.GetField("sc") != null)
        {
            this.cost = new ShopCost(obj.GetField("sc"));
        }
    }

    public void UnloadIco()
    {
        if ((UnityEngine.Object)this.ico != (UnityEngine.Object)null)
        {
            Resources.UnloadAsset(this.ico);
            this.ico = null;
        }
    }
}


