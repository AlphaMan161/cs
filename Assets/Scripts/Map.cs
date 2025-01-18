// ILSpyBased#2
using UnityEngine;

public class Map
{
    private int map_id;

    private string systemName;

    private string name = string.Empty;

    private string desc = string.Empty;

    private int modes;

    private string icoFileString = string.Empty;

    private Texture2D ico;

    private string[] availPlayers;

    private int defaultPlayerIndex;

    private ShopCost shopCost;

    private bool isTryed;

    private bool isBuyed;

    public int MapID
    {
        get
        {
            return this.map_id;
        }
    }

    public string SystemName
    {
        get
        {
            return this.systemName;
        }
    }

    public string Name
    {
        get
        {
            return LanguageManager.GetText(this.name);
        }
    }

    public string Desc
    {
        get
        {
            return LanguageManager.GetText(this.desc);
        }
    }

    public int Modes
    {
        get
        {
            return this.modes;
        }
    }

    public Texture2D Ico
    {
        get
        {
            if ((Object)this.ico == (Object)null && this.icoFileString != string.Empty)
            {
                this.ico = (Texture2D)Resources.Load(this.icoFileString);
            }
            return this.ico;
        }
    }

    public string[] AvailPlayers
    {
        get
        {
            return this.availPlayers;
        }
    }

    public int DefaultPlayerIndex
    {
        get
        {
            return this.defaultPlayerIndex;
        }
    }

    public ShopCost ShopCost
    {
        get
        {
            return this.shopCost;
        }
    }

    public bool Tryed
    {
        get
        {
            return this.isTryed;
        }
        set
        {
            this.isTryed = value;
        }
    }

    public bool Buyed
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

    public Map(string system_name, string name, string desc, int modes)
    {
        this.systemName = system_name;
        this.name = name;
        this.desc = desc;
        this.modes = modes;
        if (this.systemName == "InfernoZombie")
        {
            this.icoFileString = "GUI/Icons/Maps/Inferno";
        }
        else
        {
            this.icoFileString = "GUI/Icons/Maps/" + system_name;
        }
        this.availPlayers = new string[7] {
            "4",
            "6",
            "8",
            "10",
            "12",
            "14",
            "16"
        };
        this.defaultPlayerIndex = 4;
    }

    public Map(int mapID, string system_name, string name, string desc, int modes, string[] players, int defaultPlayer, ShopCost sc)
    {
        this.map_id = mapID;
        this.systemName = system_name;
        this.name = name;
        this.desc = desc;
        this.modes = modes;
        this.availPlayers = players;
        this.defaultPlayerIndex = defaultPlayer;
        this.shopCost = sc;
        if (this.systemName == "InfernoZombie")
        {
            this.icoFileString = "GUI/Icons/Maps/Inferno";
        }
        else
        {
            this.icoFileString = "GUI/Icons/Maps/" + system_name;
        }
    }

    public override bool Equals(object obj)
    {
        return string.Equals(this.Name, ((Map)obj).Name);
    }

    public override int GetHashCode()
    {
        return string.Format("{0};{1};{2}", this.systemName, this.name, this.desc, this.modes).GetHashCode();
    }

    public void UnloadIco()
    {
        if ((Object)this.ico != (Object)null)
        {
            Resources.UnloadAsset(this.ico);
            this.ico = null;
        }
    }
}


