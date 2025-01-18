// ILSpyBased#2
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class MyRoomSetting
{
    private long startTime;

    private string name = MapModeHelper.GenerateName();

    protected string filteredName;

    private string password = string.Empty;

    private int guestMode;

    private short max_name_lenght = 37;

    private short max_password_lenght = 12;

    private short global_min_time_limit = 1;

    private short global_max_time_limit = 50;

    private bool game_pause;

    private short current_time_limit = 10;

    private short globalMinLevel = 1;

    private short current_min_level = 4;

    private short globalMaxLevel = 50;

    private short current_max_level = 50;

    private short current_frags = 30;

    private int selectPlayerIndex = 2;

    private short maxPlayers = 16;

    private short maxPlayersTeam = 8;

    private bool isFriendlyFire;

    private MapMode.MODE game_mode = MapMode.MODE.TEAM_DEATHMATCH;

    private Map map;

    private string[] availTimes = new string[4] {
        "5",
        "10",
        "15",
        "20"
    };

    private string[] guestModes = new string[2] {
        "off",
        "on"
    };

    private int selectTimeIndex = 1;

    private string[] availFragFlag = new string[5] {
        "5",
        "10",
        "15",
        "20",
        "30"
    };

    private string[] availFragControl = new string[5] {
        "50",
        "100",
        "200",
        "300",
        "500"
    };

    private int selectedFragIndex = 1;

    private short currentUserLevel = 2;

    public long StartTime
    {
        get
        {
            return this.startTime;
        }
        set
        {
            this.startTime = value;
        }
    }

    public string Name
    {
        get
        {
            return this.name;
        }
        set
        {
            if (value.Length <= this.max_name_lenght)
            {
                this.name = value;
                this.name = Regex.Replace(this.name, "\\s", " ");
                this.name = Regex.Replace(this.name, "(.)\\1{2,}", string.Empty);
            }
            this.filteredName = BadWorldFilter.CheckLite(this.name);
        }
    }

    public string FilteredName
    {
        get
        {
            return this.filteredName;
        }
    }

    public string Password
    {
        get
        {
            return this.password;
        }
        set
        {
            if (value.Length <= this.max_password_lenght)
            {
                this.password = value;
            }
        }
    }

    public int GuestMode
    {
        get
        {
            return this.guestMode;
        }
        set
        {
            this.guestMode = value;
        }
    }

    public short MaxNameLenght
    {
        get
        {
            return this.max_name_lenght;
        }
    }

    public short MaxPasswordLenght
    {
        get
        {
            return this.max_password_lenght;
        }
    }

    public short GlobalMinTimeLimit
    {
        get
        {
            return this.global_min_time_limit;
        }
    }

    public short GlobalMaxTimeLimit
    {
        get
        {
            return this.global_max_time_limit;
        }
    }

    public bool GamePause
    {
        get
        {
            return this.game_pause;
        }
        set
        {
            this.game_pause = value;
        }
    }

    public short TimeLimit
    {
        get
        {
            return this.current_time_limit;
        }
        set
        {
            if (value > this.global_max_time_limit)
            {
                this.current_time_limit = this.global_max_time_limit;
            }
            else if (value < this.global_min_time_limit)
            {
                this.current_time_limit = this.global_min_time_limit;
            }
            else
            {
                this.current_time_limit = value;
            }
        }
    }

    public short GlobalMinLevel
    {
        get
        {
            return this.globalMinLevel;
        }
    }

    public short MinLevel
    {
        get
        {
            return this.current_min_level;
        }
        set
        {
            if (value <= this.globalMinLevel)
            {
                this.current_min_level = this.globalMinLevel;
            }
            else if (value <= this.current_max_level && this.currentUserLevel >= value)
            {
                this.current_min_level = value;
            }
        }
    }

    public short GlobalMaxLevel
    {
        get
        {
            return this.globalMaxLevel;
        }
    }

    public short MaxLevel
    {
        get
        {
            return this.current_max_level;
        }
        set
        {
            if (value > this.GlobalMaxLevel)
            {
                this.current_max_level = this.GlobalMaxLevel;
            }
            else if (value >= this.current_min_level && this.currentUserLevel <= value)
            {
                this.current_max_level = value;
            }
        }
    }

    public short Frags
    {
        get
        {
            return this.current_frags;
        }
        set
        {
            this.current_frags = value;
        }
    }

    public int SelectPlayerIndex
    {
        get
        {
            return this.selectPlayerIndex;
        }
        set
        {
            this.selectPlayerIndex = value;
            this.MaxPlayers = Convert.ToInt16(this.Map.AvailPlayers[this.selectPlayerIndex]);
        }
    }

    public short MaxPlayers
    {
        get
        {
            return this.maxPlayers;
        }
        set
        {
            this.maxPlayers = value;
            this.maxPlayersTeam = (short)((float)this.maxPlayers * 0.5f);
        }
    }

    public short MaxPlayersTeam
    {
        get
        {
            return this.maxPlayersTeam;
        }
    }

    public bool FriendlyFire
    {
        get
        {
            return this.isFriendlyFire;
        }
        set
        {
            this.isFriendlyFire = value;
        }
    }

    public MapMode.MODE GameMode
    {
        get
        {
            return this.game_mode;
        }
        set
        {
            this.game_mode = value;
            if (this.game_mode == MapMode.MODE.TOWER_DEFENSE && (this.maxPlayers > 8 || this.maxPlayers < 4))
            {
                this.maxPlayers = 6;
            }
            this.SelectedFragIndex = this.selectedFragIndex;
        }
    }

    public Map Map
    {
        get
        {
            return this.map;
        }
        set
        {
            if ((value.Modes & (int)this.game_mode) != (int)this.game_mode)
            {
                foreach (byte value2 in Enum.GetValues(typeof(MapMode.MODE)))
                {
                    if ((value.Modes & value2) == value2 && value2 != 0)
                    {
                        this.game_mode = (MapMode.MODE)value2;
                        break;
                    }
                }
            }
            this.map = value;
            this.selectPlayerIndex = this.map.DefaultPlayerIndex;
            this.MaxPlayers = Convert.ToInt16(this.map.AvailPlayers[this.selectPlayerIndex]);
        }
    }

    public string[] AvailTimes
    {
        get
        {
            return this.availTimes;
        }
    }

    public string[] GuestModes
    {
        get
        {
            return this.guestModes;
        }
    }

    public int SelectTimeIndex
    {
        get
        {
            return this.selectTimeIndex;
        }
        set
        {
            this.selectTimeIndex = value;
            this.TimeLimit = Convert.ToInt16(this.availTimes[this.selectTimeIndex]);
        }
    }

    public string[] AvailFragLimit
    {
        get
        {
            if (this.game_mode == MapMode.MODE.CAPTURE_THE_FLAG)
            {
                return this.availFragFlag;
            }
            return this.availFragControl;
        }
    }

    public int SelectedFragIndex
    {
        get
        {
            return this.selectedFragIndex;
        }
        set
        {
            this.selectedFragIndex = value;
            this.Frags = Convert.ToInt16(this.AvailFragLimit[this.selectedFragIndex]);
        }
    }

    public MyRoomSetting(string name, short userLevel, int mapIndex)
    {
        this.currentUserLevel = userLevel;
        this.MinLevel = (short)(userLevel - 2);
        this.MaxLevel = (short)(userLevel + 2);
        UnityEngine.Debug.Log("[MyRoomSetting] this.currentUserLevel = " + this.currentUserLevel);
        UnityEngine.Debug.Log("[MyRoomSetting] this.MinLevel = " + this.MinLevel);
        UnityEngine.Debug.Log("[MyRoomSetting] this.MaxLevel = " + this.MaxLevel);
        this.globalMinLevel = 6;
        this.globalMaxLevel = 50;
        this.Map = MapList.Instance[mapIndex];
    }

    public MyRoomSetting(Hashtable data)
    {
        this.FromHashtable(data);
    }

    public Hashtable ToHashtable()
    {
        UnityEngine.Debug.Log("time_limit: " + this.TimeLimit);
        UnityEngine.Debug.Log("frag_limit: " + this.Frags);
        UnityEngine.Debug.Log("friendly_fire: " + this.isFriendlyFire);
        UnityEngine.Debug.Log("lvl_min: " + this.current_min_level);
        UnityEngine.Debug.Log("lvl_max: " + this.current_max_level);
        UnityEngine.Debug.Log("game_mode: " + this.GameMode);
        UnityEngine.Debug.Log("map: " + this.map.SystemName);
        UnityEngine.Debug.Log("max_users: " + this.MaxPlayers);
        UnityEngine.Debug.Log("name: " + this.Name);
        Hashtable hashtable = new Hashtable();
        hashtable.Add("time_limit", this.TimeLimit);
        hashtable.Add("frag_limit", this.Frags);
        hashtable.Add("friendly_fire", this.isFriendlyFire);
        hashtable.Add("lvl_min", this.current_min_level);
        hashtable.Add("lvl_max", this.current_max_level);
        hashtable.Add("game_mode", (byte)this.GameMode);
        hashtable.Add("map", this.map.SystemName);
        hashtable.Add("max_users", this.MaxPlayers);
        hashtable.Add("name", this.Name);
        if (this.password != string.Empty)
        {
            hashtable.Add("password", this.password);
        }
        if (this.guestMode > 0)
        {
            hashtable.Add("guest_mode", (short)this.GuestMode);
        }
        return hashtable;
    }

    public void FromHashtable(Hashtable data)
    {
        this.TimeLimit = (short)data["time_limit"];
        this.current_frags = (short)data["frag_limit"];
        this.isFriendlyFire = (bool)data["friendly_fire"];
        this.current_min_level = (short)data["lvl_min"];
        this.current_max_level = (short)data["lvl_max"];
        this.game_mode = MapModeHelper.FromByte((byte)data["game_mode"]);
        this.map = MapList.GetMap((string)data["map"]);
        this.MaxPlayers = (short)data["max_users"];
        this.Name = (string)data["name"];
        if (data.ContainsKey("game_param"))
        {
            Hashtable hashtable = (Hashtable)data["game_param"];
            NetworkDev.Remote_Animation_Send = (bool)hashtable["remote_animation_send"];
            NetworkDev.Remote_Animation = (bool)hashtable["remote_animation_receive"];
            NetworkDev.TPS = (int)hashtable["transform_per_second"];
            if (hashtable.ContainsKey("tcp_transform_per_second"))
            {
                NetworkDev.TCP_TPS = (int)hashtable["tcp_transform_per_second"];
            }
            if (hashtable.ContainsKey("interpolation_mode"))
            {
                NetworkDev.InterpolationMode = (InterpolationMode)(byte)hashtable["interpolation_mode"];
            }
            NetworkDev.Destroy_Geometry = (bool)hashtable["destroy_geometry"];
        }
        else
        {
            NetworkDev.Remote_Animation = true;
            NetworkDev.Remote_Animation_Send = true;
            NetworkDev.TPS = 100;
            NetworkDev.TCP_TPS = 0;
            NetworkDev.Destroy_Geometry = true;
            NetworkDev.InterpolationMode = InterpolationMode.SMOOTH_LINEAR_IN_EX;
        }
        if (data.ContainsKey("guest_mode"))
        {
            this.guestMode = (short)data["guest_mode"];
        }
    }

    public static Hashtable JoinRoomNameToHashtable(string gameName, string password, bool guest)
    {
        Hashtable hashtable = new Hashtable();
        if (guest)
        {
            hashtable.Add("guest_mode", true);
        }
        hashtable.Add("name", gameName);
        if (password != string.Empty)
        {
            hashtable.Add("password", password);
        }
        return hashtable;
    }

    public void GenerateName()
    {
        this.name = MapModeHelper.GenerateName();
    }
}


