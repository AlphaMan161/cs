// ILSpyBased#2
using System;

public class ResponseRoomList : RoomInfo
{
    private string mapName;

    private string mapSystemName;

    private short lvlMin;

    private short lvlMax;

    private string mode;

    private MapMode.MODE mapMode;

    private short timeLimit;

    private short fragLimit;

    private bool friendlyFire;

    private bool isAvail = true;

    private int sqrAverage = 100;

    private bool isPassword;

    private string connectingPassword = string.Empty;

    private bool isGuest;

    public string MapName
    {
        get
        {
            return this.mapName;
        }
    }

    public string MapSystemName
    {
        get
        {
            return this.mapSystemName;
        }
    }

    public short LvlMin
    {
        get
        {
            return this.lvlMin;
        }
    }

    public short LvlMax
    {
        get
        {
            return this.lvlMax;
        }
    }

    public string Mode
    {
        get
        {
            return this.mode;
        }
    }

    public MapMode.MODE MapMode
    {
        get
        {
            return this.mapMode;
        }
    }

    public short TimeLimit
    {
        get
        {
            return this.timeLimit;
        }
    }

    public short FragLimit
    {
        get
        {
            return this.fragLimit;
        }
    }

    public bool FriendlyFire
    {
        get
        {
            return this.friendlyFire;
        }
    }

    public bool IsAvail
    {
        get
        {
            return this.isAvail;
        }
    }

    public int SqrAverage
    {
        get
        {
            return this.sqrAverage;
        }
    }

    public bool IsPassword
    {
        get
        {
            return this.isPassword;
        }
    }

    public string ConnectingPassword
    {
        get
        {
            return this.connectingPassword;
        }
        set
        {
            this.connectingPassword = value;
        }
    }

    public bool IsGuest
    {
        get
        {
            return this.isGuest;
        }
        set
        {
            this.isGuest = value;
        }
    }

    public ResponseRoomList(string roomName, string[] data, int userLevel)
        : base(roomName, string.Empty, 0, 0)
    {
        base.name = roomName;
        this.mapName = MapList.GetMapName(data[0]);
        this.mapSystemName = data[0];
        this.lvlMin = Convert.ToInt16(data[1]);
        this.lvlMax = Convert.ToInt16(data[2]);
        this.mode = MapModeHelper.ToString((MapMode.MODE)(byte)Convert.ToInt32(data[3]));
        this.mapMode = (MapMode.MODE)Convert.ToInt32(data[3]);
        this.timeLimit = Convert.ToInt16(data[4]);
        this.fragLimit = Convert.ToInt16(data[5]);
        base.userOnline = Convert.ToInt16(data[6]);
        base.userMax = Convert.ToInt16(data[7]);
        this.friendlyFire = Convert.ToBoolean(data[8]);
        this.sqrAverage = (userLevel - this.lvlMin) * (userLevel - this.lvlMin) + (this.lvlMax - userLevel) * (this.lvlMax - userLevel);
        if (base.UserMax == base.UserOnline || this.LvlMin > userLevel || this.LvlMax < userLevel)
        {
            this.isAvail = false;
        }
        if (data.Length > 9)
        {
            this.isPassword = Convert.ToBoolean(data[9]);
        }
    }
}


