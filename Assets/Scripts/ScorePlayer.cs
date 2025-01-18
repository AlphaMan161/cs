// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScorePlayer
{
    private int user_id;

    private string userName;

    private int kill;

    private bool isPremium;

    private int death;

    private float kd;

    private int point;

    private int flag;

    private int controlPoint;

    private int exp;

    private float clanKoef = -1f;

    private int exp2clan;

    private bool _tmpCalculate2clan = true;

    private int clanArmId;

    private int team_num;

    private int pos;

    private ScoreTeam team;

    private bool isDead;

    private int lvl;

    private float uRating;

    private int ping;

    public bool Victim;

    public bool Nemesis;

    private Dictionary<WeaponType, int> statsOnWeaponType = new Dictionary<WeaponType, int>();

    public FragKill LastFrag;

    private List<Achievement> achievements = new List<Achievement>();

    private WeaponType[] weaponTypes;

    public int UserID
    {
        get
        {
            return this.user_id;
        }
    }

    public string UserName
    {
        get
        {
            return this.userName;
        }
    }

    public int Kill
    {
        get
        {
            return this.kill;
        }
        set
        {
            if (this.team != null)
            {
                this.team.Kill += value - this.kill;
            }
            this.kill = value;
            this.kd = Convert.ToSingle(this.kill) / ((this.death <= 0) ? 1f : ((float)this.death));
        }
    }

    public bool IsPremium
    {
        get
        {
            return this.isPremium;
        }
    }

    public int Death
    {
        get
        {
            return this.death;
        }
        set
        {
            if (this.team != null)
            {
                this.team.Death += value - this.death;
            }
            this.death = value;
            this.kd = Convert.ToSingle(this.kill) / ((this.death <= 0) ? 1f : ((float)this.death));
        }
    }

    public float KD
    {
        get
        {
            return this.kd;
        }
    }

    public int Point
    {
        get
        {
            return this.point;
        }
        set
        {
            if (this.point != value)
            {
                this.point = value;
                this.OnChange();
            }
            if (this.team != null)
            {
                this.team.Point += value;
            }
        }
    }

    public int Flag
    {
        get
        {
            return this.flag;
        }
        set
        {
            this.flag = value;
            if (this.team != null)
            {
                this.team.Flag += value;
            }
        }
    }

    public int ControlPoint
    {
        get
        {
            return this.controlPoint;
        }
        set
        {
            this.controlPoint = value;
            if (this.team != null)
            {
                this.team.ControlPoint += value;
            }
        }
    }

    public int Exp
    {
        get
        {
            return this.exp;
        }
        set
        {
            if (this.exp != value && value > 0)
            {
                this.exp = value;
                if (LocalUser.UserID == this.UserID && LocalUser.Clan != null && this._tmpCalculate2clan)
                {
                    if (this.clanKoef == -1f)
                    {
                        this.clanKoef = Convert.ToSingle(ClanManager.AvailableKoef[ClanManager.SelectedIndexKoef]) * 0.01f;
                    }
                    if (this.clanKoef > 0f)
                    {
                        this.exp2clan = (int)Math.Round((double)((float)value * this.clanKoef));
                    }
                }
            }
        }
    }

    public int Exp2clan
    {
        get
        {
            return this.exp2clan;
        }
        set
        {
            this.exp2clan = value;
            this._tmpCalculate2clan = false;
        }
    }

    public int ClanArmId
    {
        get
        {
            return this.clanArmId;
        }
        set
        {
            this.clanArmId = value;
        }
    }

    public int TeamNum
    {
        get
        {
            if (this.team != null)
            {
                return this.team.Team;
            }
            return 0;
        }
    }

    public int Position
    {
        get
        {
            return this.pos;
        }
        set
        {
            this.pos = value;
        }
    }

    public ScoreTeam Team
    {
        get
        {
            return this.team;
        }
        set
        {
            this.team = value;
            this.team_num = this.team.Team;
        }
    }

    public bool IsDead
    {
        get
        {
            return this.isDead;
        }
        set
        {
            this.isDead = value;
            this.OnChange();
        }
    }

    public int Level
    {
        get
        {
            return this.lvl;
        }
        set
        {
            this.lvl = value;
        }
    }

    public float URating
    {
        get
        {
            return this.uRating;
        }
    }

    public int Ping
    {
        get
        {
            return this.ping;
        }
        set
        {
            this.ping = value - 20;
            if (this.ping < 10)
            {
                this.ping = 10;
            }
        }
    }

    public int Domination
    {
        get;
        set;
    }

    public int Revenge
    {
        get;
        set;
    }

    public Dictionary<WeaponType, int> StatsOnWeaponType
    {
        get
        {
            return this.statsOnWeaponType;
        }
    }

    public List<Achievement> Achievements
    {
        get
        {
            return this.achievements;
        }
    }

    public ScorePlayer(int in_userId, string in_userName, int in_lvl, bool isPremium, ScoreTeam in_scoreTeam, WeaponType[] weaponTypes)
    {
        this.weaponTypes = weaponTypes;
        this.user_id = in_userId;
        this.userName = BadWorldFilter.CheckLite(in_userName);
        this.kill = 0;
        this.death = 0;
        this.kd = 0f;
        this.point = 0;
        this.exp = 0;
        this.exp2clan = 0;
        this.isPremium = isPremium;
        if (in_scoreTeam != null)
        {
            this.Team = in_scoreTeam;
        }
        else
        {
            this.team_num = 0;
        }
        this.isDead = true;
        this.lvl = in_lvl;
        this.uRating = 0f;
        this.Domination = 0;
        this.Revenge = 0;
        this.Victim = false;
        this.Nemesis = false;
        WeaponType[] array = this.weaponTypes;
        foreach (WeaponType key in array)
        {
            this.statsOnWeaponType.Add(key, 0);
        }
    }

    private void OnChange()
    {
        if (this.team != null)
        {
            this.team.ResortList();
        }
        else
        {
            UnityEngine.Debug.LogError("[ScorePlayer] OnChange ScoreTeam in null");
        }
    }

    public void AddCompleteAchievement(long achievement_id, int maxValue, int currentValue, int reward)
    {
        this.achievements.Add(new Achievement(achievement_id, currentValue, maxValue, reward));
        WebCall.GameAnalitic("ach" + achievement_id, reward);
    }

    public void Reset()
    {
        this.kill = 0;
        this.death = 0;
        this.kd = 0f;
        this.point = 0;
        this.exp = 0;
        this.exp2clan = 0;
        this.isDead = true;
        this.uRating = 0f;
        if (this.team != null)
        {
            this.team.Kill = 0;
            this.team.Death = 0;
            this.team.Flag = 0;
            this.team.Point = 0;
            this.team.ControlPoint = 0;
        }
        this.Domination = 0;
        this.Revenge = 0;
        this.Victim = false;
        this.Nemesis = false;
        WeaponType[] array = this.weaponTypes;
        foreach (WeaponType key in array)
        {
            this.statsOnWeaponType[key] = 0;
        }
        this.achievements.Clear();
    }
}


