// ILSpyBased#2
using System;
using System.Collections.Generic;
using System.Linq;

public class ScoreTeam
{
    private int team;

    private FlagState flagState = FlagState.Captured;

    private int kill;

    private int death;

    private int count;

    private int livedCount;

    private int point;

    private int wins;

    private int flag;

    private int controlPoint;

    public Dictionary<int, ScorePlayer> UserList = new Dictionary<int, ScorePlayer>();

    public Dictionary<int, ScorePlayer> sortedUserList = new Dictionary<int, ScorePlayer>();

    public int Team
    {
        get
        {
            return this.team;
        }
    }

    public FlagState FlagState
    {
        get
        {
            return this.flagState;
        }
        set
        {
            this.flagState = value;
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
            this.kill = value;
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
            this.death = value;
        }
    }

    public int Count
    {
        get
        {
            return this.count;
        }
        set
        {
            this.count = value;
        }
    }

    public int LivedCount
    {
        get
        {
            return this.livedCount;
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
            this.point = value;
        }
    }

    public int Wins
    {
        get
        {
            return this.wins;
        }
        set
        {
            this.wins = value;
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
        }
    }

    public Dictionary<int, ScorePlayer> SortedUserList
    {
        get
        {
            return this.sortedUserList;
        }
    }

    public ScoreTeam(int in_teamNum)
    {
        this.team = in_teamNum;
        this.kill = 0;
        this.death = 0;
        this.count = 0;
        this.point = 0;
        this.flag = 0;
        this.controlPoint = 0;
    }

    public void ResortList()
    {
        this.sortedUserList = (from entry in this.UserList
        orderby entry.Value.Point descending
        select entry).ToDictionary((KeyValuePair<int, ScorePlayer> pair) => pair.Key, (KeyValuePair<int, ScorePlayer> pair) => pair.Value);
        int num = 1;
        this.livedCount = 0;
        Dictionary<int, ScorePlayer>.Enumerator enumerator = this.sortedUserList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, ScorePlayer> current = enumerator.Current;
                current.Value.Position = num;
                num++;
                if (!current.Value.IsDead)
                {
                    this.livedCount++;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public void AddUser(ScorePlayer player)
    {
        if (!this.UserList.ContainsKey(player.UserID))
        {
            this.UserList.Add(player.UserID, player);
            player.Team = this;
            this.Count++;
        }
        this.ResortList();
    }

    public bool Contains(int user_id)
    {
        return this.UserList.ContainsKey(user_id);
    }

    public void RemoveUser(int user_id)
    {
        if (this.UserList.ContainsKey(user_id))
        {
            this.UserList.Remove(user_id);
            this.Count--;
        }
        this.ResortList();
    }

    public void RemoveAll()
    {
        this.UserList.Clear();
        this.Count = 0;
    }

    public void Reset()
    {
        this.kill = 0;
        this.death = 0;
        this.point = 0;
        this.flag = 0;
        this.controlPoint = 0;
        Dictionary<int, ScorePlayer>.Enumerator enumerator = this.UserList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.Reset();
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }
}


