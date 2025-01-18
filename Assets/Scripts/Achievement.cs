// ILSpyBased#2
using System;
using System.Collections;
using UnityEngine;

public class Achievement
{
    private int id;

    private int usrLvl;

    private long achievement_id;

    private long achievement_sub_id;

    private string name = string.Empty;

    private string desc = string.Empty;

    private string icoFileString = string.Empty;

    private Texture2D ico;

    private int val;

    private int maxVal;

    private int reward;

    private int level = 1;

    private int maxLevel = 1;

    private bool isComplete;

    public int ID
    {
        get
        {
            return this.id;
        }
    }

    public int UserLvl
    {
        get
        {
            return this.usrLvl;
        }
    }

    public long AchievementID
    {
        get
        {
            return this.achievement_id;
        }
    }

    public long AchievementSubID
    {
        get
        {
            return this.achievement_sub_id;
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

    public int Value
    {
        get
        {
            return this.val;
        }
        set
        {
            this.val = value;
        }
    }

    public int MaxValue
    {
        get
        {
            return this.maxVal;
        }
    }

    public int Reward
    {
        get
        {
            return this.reward;
        }
    }

    public int Level
    {
        get
        {
            return this.level;
        }
        set
        {
            this.level = value;
        }
    }

    public int MaxLevel
    {
        get
        {
            return this.maxLevel;
        }
        set
        {
            this.maxLevel = value;
        }
    }

    public bool Complete
    {
        get
        {
            return this.isComplete;
        }
        set
        {
            this.isComplete = value;
        }
    }

    public Achievement(long achievement_id)
    {
        this.achievement_id = achievement_id;
        this._initialize();
    }

    public Achievement(long achievement_id, int maxVal)
    {
        this.achievement_id = achievement_id;
        this.maxVal = maxVal;
        this._initialize();
    }

    public Achievement(int id, int usrLvl, long achievement_id, int maxVal, int reward)
    {
        this.id = id;
        this.usrLvl = usrLvl;
        this.achievement_id = achievement_id;
        this.maxVal = maxVal;
        this.reward = reward;
        this._initialize();
    }

    public Achievement(long achievement_id, int maxVal, int reward)
    {
        this.achievement_id = achievement_id;
        this.maxVal = maxVal;
        this.reward = reward;
        this._initialize();
    }

    public Achievement(long achievement_id, int val, int maxVal, int reward)
    {
        this.achievement_id = achievement_id;
        this.val = val;
        this.maxVal = maxVal;
        this.reward = reward;
        this._initialize();
    }

    private void _initialize()
    {
        this.name = "ach" + this.achievement_id + "name";
        this.desc = "ach" + this.achievement_id + "desc";
        this.icoFileString = "GUI/Icons/Achievement/" + LanguageManager.GetText("ach" + this.achievement_id + "img");
        this.achievement_sub_id = this.achievement_id / 100;
        this.level = Convert.ToInt32(this.achievement_id % 100);
        this.maxLevel = this.level;
    }

    public static string GetName(long achievement_id)
    {
        return LanguageManager.GetText("ach" + achievement_id + "name");
    }

    public void Update(Hashtable data)
    {
        int num = this.val;
        if (data.ContainsKey((byte)2))
        {
            this.val = (int)data[(byte)2];
        }
        else
        {
            this.val = 0;
        }
        if (data.ContainsKey((byte)3))
        {
            this.isComplete = (bool)data[(byte)3];
        }
        else
        {
            this.isComplete = false;
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


