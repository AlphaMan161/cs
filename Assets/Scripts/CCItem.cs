// ILSpyBased#2
using UnityEngine;

public class CCItem
{
    protected uint itemID;

    protected string name;

    protected string sname;

    protected string desc;

    protected string descAdditional = string.Empty;

    protected string ndescAdditional = string.Empty;

    protected string icoFileString = string.Empty;

    private Texture2D ico;

    protected Texture2D icoActive;

    protected ShopCost shop_cost;

    protected uint nlvl;

    protected bool isBuyed;

    protected bool isSale;

    public uint ItemID
    {
        get
        {
            return this.itemID;
        }
    }

    public string Name
    {
        get
        {
            return LanguageManager.GetText(this.name);
        }
    }

    public string SystemName
    {
        get
        {
            return this.sname;
        }
    }

    public string Desc
    {
        get
        {
            return LanguageManager.GetText(this.desc);
        }
    }

    public string DescAdditional
    {
        get
        {
            return LanguageManager.GetText(this.descAdditional);
        }
    }

    public string NDescAdditional
    {
        get
        {
            return LanguageManager.GetText(this.ndescAdditional);
        }
    }

    public Texture2D Ico
    {
        get
        {
            if ((Object)this.ico == (Object)null && this.icoFileString != string.Empty)
            {
                UnityEngine.Debug.LogFormat("Resources.Load(icoFileString={0})", this.icoFileString);
                this.ico = (Texture2D)Resources.Load(this.icoFileString);
            }
            return this.ico;
        }
    }

    public Texture2D IcoActive
    {
        get
        {
            if ((Object)this.icoActive == (Object)null)
            {
                return this.Ico;
            }
            return this.icoActive;
        }
    }

    public ShopCost Shop_Cost
    {
        get
        {
            return this.shop_cost;
        }
    }

    public uint NeedLvl
    {
        get
        {
            return this.nlvl;
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

    public bool IsSale
    {
        get
        {
            return this.isSale;
        }
    }

    public static CCItem GetItem(CCItemType ccType, JSONObject json)
    {
        return null;
    }

    public static CCItem Clone(CCItem ccItem)
    {
        return (CCItem)ccItem.MemberwiseClone();
    }

    public void UnloadIco()
    {
        if ((Object)this.icoActive != (Object)null)
        {
            Resources.UnloadAsset(this.icoActive);
            this.icoActive = null;
        }
        if ((Object)this.ico != (Object)null)
        {
            Resources.UnloadAsset(this.ico);
            this.ico = null;
        }
    }
}


