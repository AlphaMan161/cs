// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class GameScore
{
    private List<ScoreTeam> teams = new List<ScoreTeam>();

    private MapMode.MODE gameMode = MapMode.MODE.DEATHMATCH;

    private List<ScoreControlPoint> controlPoints;

    private Queue<FragKill> frags = new Queue<FragKill>();

    private object fragsLock = new object();

    private float lastFragCheck;

    public List<ScoreTeam> Teams
    {
        get
        {
            return this.teams;
        }
    }

    public ScorePlayer this[int user_id]
    {
        get
        {
            List<ScoreTeam>.Enumerator enumerator = this.teams.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ScoreTeam current = enumerator.Current;
                    if (current.UserList.ContainsKey(user_id))
                    {
                        return current.UserList[user_id];
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            throw new Exception("Game Score user not found user_id: " + user_id);
        }
    }

    public int ControlPointCount
    {
        get
        {
            if (this.controlPoints == null)
            {
                return 0;
            }
            return this.controlPoints.Count;
        }
    }

    public List<ScoreControlPoint> ControlPoints
    {
        get
        {
            return this.controlPoints;
        }
    }

    public Queue<FragKill> Frags
    {
        get
        {
            this.ClearFrags();
            return this.frags;
        }
    }

    public GameScore(MapMode.MODE in_gameMode)
    {
        this.gameMode = in_gameMode;
        switch (this.gameMode)
        {
            case MapMode.MODE.DEATHMATCH:
            case MapMode.MODE.TOWER_DEFENSE:
                this.teams.Add(new ScoreTeam(0));
                break;
            case MapMode.MODE.TEAM_DEATHMATCH:
            case MapMode.MODE.CAPTURE_THE_FLAG:
            case MapMode.MODE.CONTROL_POINTS:
            case MapMode.MODE.ESCORT:
                this.teams.Add(new ScoreTeam(1));
                this.teams.Add(new ScoreTeam(2));
                if (this.gameMode == MapMode.MODE.CONTROL_POINTS)
                {
                    this.controlPoints = new List<ScoreControlPoint>();
                }
                break;
            case MapMode.MODE.ZOMBIE:
                this.teams.Add(new ScoreTeam(1));
                this.teams.Add(new ScoreTeam(2));
                break;
            default:
                throw new Exception("Unkown game type. GameScore Exception");
        }
    }

    public void AddUser(int userID, string userName, int uLvl, bool isPremium, int team_num, Dictionary<int, Hashtable> weapons, int clan_arm_id, string clan_tag)
    {
        ScorePlayer scorePlayer = null;
        if (clan_tag != string.Empty)
        {
            userName = string.Format("[{0}] {1}", clan_tag, userName);
        }
        if (team_num < 2)
        {
            if (!this.teams[0].Contains(userID))
            {
                List<WeaponType> list = new List<WeaponType>();
                if (weapons != null)
                {
                    Dictionary<int, Hashtable>.Enumerator enumerator = weapons.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            KeyValuePair<int, Hashtable> current = enumerator.Current;
                            if (current.Value.ContainsKey((byte)98))
                            {
                                list.Add((WeaponType)(byte)(int)current.Value[(byte)98]);
                            }
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError("[GameScore] AddUser weapons is null");
                }
                scorePlayer = new ScorePlayer(userID, userName, uLvl, isPremium, this.teams[0], list.ToArray());
                scorePlayer.ClanArmId = clan_arm_id;
                this.teams[0].AddUser(scorePlayer);
            }
        }
        else if (!this.teams[1].Contains(userID))
        {
            List<WeaponType> list2 = new List<WeaponType>();
            if (weapons != null)
            {
                Dictionary<int, Hashtable>.Enumerator enumerator2 = weapons.GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        KeyValuePair<int, Hashtable> current2 = enumerator2.Current;
                        if (current2.Value.ContainsKey((byte)98))
                        {
                            list2.Add((WeaponType)(byte)(int)current2.Value[(byte)98]);
                        }
                    }
                }
                finally
                {
                    ((IDisposable)enumerator2).Dispose();
                }
            }
            else
            {
                UnityEngine.Debug.LogError("[GameScore] AddUser weapons is null");
            }
            scorePlayer = new ScorePlayer(userID, userName, uLvl, isPremium, this.teams[1], list2.ToArray());
            scorePlayer.ClanArmId = clan_arm_id;
            this.teams[1].AddUser(scorePlayer);
        }
        if (scorePlayer != null)
        {
            BattleChat.UserJoined(scorePlayer);
        }
    }

    public void UpdateUser(int userID, int team_num)
    {
        ScorePlayer scorePlayer = null;
        scorePlayer = this[userID];
        this.RemoveUser(userID);
        if (team_num < 2)
        {
            if (!this.teams[0].Contains(userID))
            {
                scorePlayer.Team = this.teams[0];
                this.teams[0].AddUser(scorePlayer);
            }
        }
        else if (!this.teams[1].Contains(userID))
        {
            scorePlayer.Team = this.teams[1];
            this.teams[1].AddUser(scorePlayer);
        }
        if (scorePlayer != null)
        {
            BattleChat.UserJoined(scorePlayer);
        }
    }

    public void RemoveUser(int user_id)
    {
        List<ScoreTeam>.Enumerator enumerator = this.teams.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ScoreTeam current = enumerator.Current;
                if (current.UserList.ContainsKey(user_id))
                {
                    current.RemoveUser(user_id);
                    break;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public void RemoveAll()
    {
        List<ScoreTeam>.Enumerator enumerator = this.teams.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ScoreTeam current = enumerator.Current;
                current.RemoveAll();
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public void ChangeTeam(int user_id, int new_team_num)
    {
        List<ScoreTeam>.Enumerator enumerator = this.teams.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ScoreTeam current = enumerator.Current;
                if (current.UserList.ContainsKey(user_id))
                {
                    ScorePlayer player = current.UserList[user_id];
                    if (new_team_num > 2)
                    {
                        this.teams[0].AddUser(player);
                    }
                    else
                    {
                        this.teams[1].AddUser(player);
                    }
                    break;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public bool ContainsUser(int user_id)
    {
        List<ScoreTeam>.Enumerator enumerator = this.teams.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ScoreTeam current = enumerator.Current;
                if (current.UserList.ContainsKey(user_id))
                {
                    return true;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return false;
    }

    public bool HasKills()
    {
        List<ScoreTeam>.Enumerator enumerator = this.teams.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ScoreTeam current = enumerator.Current;
                Dictionary<int, ScorePlayer>.ValueCollection.Enumerator enumerator2 = current.UserList.Values.GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        ScorePlayer current2 = enumerator2.Current;
                        if (current2.Kill > 0)
                        {
                            return true;
                        }
                    }
                }
                finally
                {
                    ((IDisposable)enumerator2).Dispose();
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return false;
    }

    public int GetUserTeam(int user_id)
    {
        List<ScoreTeam>.Enumerator enumerator = this.teams.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ScoreTeam current = enumerator.Current;
                if (current.UserList.ContainsKey(user_id))
                {
                    return current.UserList[user_id].TeamNum;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return 0;
    }

    public void Reset()
    {
        List<ScoreTeam>.Enumerator enumerator = this.teams.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ScoreTeam current = enumerator.Current;
                current.Reset();
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public void DebugLog()
    {
        UnityEngine.Debug.Log(string.Format("[GameScorethis={0}]", this));
        List<ScoreTeam>.Enumerator enumerator = this.teams.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ScoreTeam current = enumerator.Current;
                UnityEngine.Debug.Log(string.Format("[ScoreTeam: Team={0}, Kill={1}, Death={2}, Count={3}, Point={4}, Flag={5}, ControlPoint={6}]", current.Team, current.Kill, current.Death, current.Count, current.Point, current.Flag, current.ControlPoint));
                Dictionary<int, ScorePlayer>.Enumerator enumerator2 = current.UserList.GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        KeyValuePair<int, ScorePlayer> current2 = enumerator2.Current;
                        UnityEngine.Debug.Log(string.Format("[ScorePlayer: UserID={0}, UserName={1}, Kill={2}, Death={3}, KD={4}, Point={5}, Flag={6}, ControlPoint={7}, TeamNum={8}, Team={9}, IsDead={10}, Level={11}, URating={12}, Domination={13}]", current2.Value.UserID, current2.Value.UserName, current2.Value.Kill, current2.Value.Death, current2.Value.KD, current2.Value.Point, current2.Value.Flag, current2.Value.ControlPoint, current2.Value.TeamNum, current2.Value.Team, current2.Value.IsDead, current2.Value.Level, current2.Value.URating, current2.Value.Domination));
                    }
                }
                finally
                {
                    ((IDisposable)enumerator2).Dispose();
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public int GetTeamCount(short team)
    {
        if (team < 2)
        {
            return this.teams[0].Count;
        }
        return this.teams[1].Count;
    }

    public int GetTeamPoints(short team)
    {
        if (team < 2)
        {
            return this.teams[0].Point;
        }
        return this.teams[1].Point;
    }

    public int GetTeamKills(short team)
    {
        if (team < 2)
        {
            return this.teams[0].Kill;
        }
        return this.teams[1].Kill;
    }

    public int GetTeamDeaths(short team)
    {
        if (team < 2)
        {
            return this.teams[0].Death;
        }
        return this.teams[1].Death;
    }

    public void SetTeamPoints(short team, int points)
    {
        if (team < 2)
        {
            this.teams[0].Point = points;
        }
        this.teams[1].Point = points;
    }

    public void AddFrag(FragKill frag)
    {
        object obj = this.fragsLock;
        Monitor.Enter(obj);
        try
        {
            this.frags.Enqueue(frag);
            while (this.frags.Count > 10)
            {
                this.frags.Dequeue();
            }
            if (this.ContainsUser(frag.KillerID) && frag.WeaponType != 0 && this[frag.KillerID].StatsOnWeaponType.ContainsKey(frag.WeaponType))
            {
                Dictionary<WeaponType, int> statsOnWeaponType;
                Dictionary<WeaponType, int> dictionary = statsOnWeaponType = this[frag.KillerID].StatsOnWeaponType;
                WeaponType weaponType;
                WeaponType key = weaponType = frag.WeaponType;
                int num = statsOnWeaponType[weaponType];
                dictionary[key] = num + 1;
            }
            if (this.ContainsUser(frag.KilledID))
            {
                this[frag.KilledID].LastFrag = frag;
            }
            if (frag.KillerID == frag.KilledID && this.ContainsUser(frag.KilledID))
            {
                this[frag.KilledID].LastFrag = null;
            }
        }
        finally
        {
            Monitor.Exit(obj);
        }
    }

    private void ClearFrags()
    {
        if (this.lastFragCheck + 1f < Time.time)
        {
            object obj = this.fragsLock;
            Monitor.Enter(obj);
            try
            {
                while (true)
                {
                    if (this.frags.Count <= 10)
                    {
                        if (this.frags.Count <= 0)
                        {
                            break;
                        }
                        if (!(this.frags.First().TimeKill + 4f < Time.time))
                        {
                            break;
                        }
                    }
                    this.frags.Dequeue();
                }
            }
            finally
            {
                Monitor.Exit(obj);
            }
            this.lastFragCheck = Time.time;
        }
    }
}


