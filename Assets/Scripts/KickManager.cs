// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class KickManager : MonoBehaviour
{
    private const long KICK_OUTDATE_PERIOD = 36000000000L;

    private const long KickPeriod = 30000L;

    private const long KickShowPeriod = 5000L;

    private const long KickDelayPeriod = 240000L;

    private KickReason voteReason;

    private int kickedAuthID = -1;

    private string kickStarterName = string.Empty;

    private string kickedName = string.Empty;

    private long kickStart;

    private int voteYes;

    private int voteNo;

    private static KickManager instance;

    private bool? voteResult;

    private KickVoteState state;

    public KickReason VoteReason
    {
        get
        {
            return this.voteReason;
        }
    }

    public int KickedAuthID
    {
        get
        {
            return this.kickedAuthID;
        }
    }

    public string KickStarterName
    {
        get
        {
            return this.kickStarterName;
        }
    }

    public string KickedName
    {
        get
        {
            return this.kickedName;
        }
    }

    public long KickStart
    {
        get
        {
            return this.kickStart;
        }
    }

    public int VoteYes
    {
        get
        {
            return this.voteYes;
        }
    }

    public int VoteNo
    {
        get
        {
            return this.voteNo;
        }
    }

    public static KickManager Instance
    {
        get
        {
            return KickManager.instance;
        }
    }

    public bool VoteResult
    {
        get
        {
            if (!this.voteResult.HasValue)
            {
                return false;
            }
            return this.voteResult.Value;
        }
    }

    public KickVoteState State
    {
        get
        {
            this.CheckState();
            return this.state;
        }
    }

    private void Awake()
    {
        KickManager.instance = this;
    }

    private void CheckState()
    {
        if (this.state == KickVoteState.Result && TimeManager.Instance.NetworkTime > this.kickStart + 30000 + 5000)
        {
            this.ResetVote();
        }
    }

    private void Start()
    {
        this.kickStart = TimeManager.Instance.NetworkTime - 240000;
    }

    public bool CanStartVote()
    {
        return !this.IsVoting() && TimeManager.Instance.NetworkTime > this.kickStart + 240000;
    }

    public bool CanVote()
    {
        return this.IsVoting() && !this.voteResult.HasValue;
    }

    public long TimeLeft()
    {
        long num = this.kickStart + 30000 - TimeManager.Instance.NetworkTime;
        if (num < 0)
        {
            num = 0L;
        }
        return num;
    }

    public bool IsVoting()
    {
        return this.kickedAuthID != -1;
    }

    public void ReadVote(Hashtable data)
    {
        int num = (int)data[(byte)1];
        int num2 = (int)data[(byte)6];
        int num3 = (int)data[(byte)7];
        if (data.ContainsKey((byte)2))
        {
            bool value = (bool)data[(byte)2];
            if (num == this.kickedAuthID)
            {
                this.voteYes = num2;
                this.voteNo = num3;
                this.voteResult = value;
                this.state = KickVoteState.Result;
                this.kickStart = TimeManager.Instance.NetworkTime - 30000;
            }
        }
        else if (data.ContainsKey((byte)9))
        {
            byte reason = (byte)data[(byte)9];
            string text = (string)data[(byte)10];
            string text2 = (string)data[(byte)11];
            this.kickStart = TimeManager.Instance.NetworkTime;
            this.kickedName = text;
            this.kickStarterName = text2;
            this.voteYes = num2;
            this.voteNo = num3;
            this.voteReason = KickManagerHelper.FromByte(reason);
            this.kickedAuthID = num;
            this.state = KickVoteState.Progress;
        }
        else if (num == this.kickedAuthID)
        {
            this.voteYes = num2;
            this.voteNo = num3;
            this.state = KickVoteState.Progress;
        }
    }

    public void ResetVote()
    {
        this.state = KickVoteState.None;
        this.voteResult = null;
        this.voteNo = 0; this.voteYes = (this.voteNo );
        this.kickedAuthID = -1;
    }

    public bool InitVote(int kickedAuthID, byte voteReason)
    {
        if (!this.CanStartVote())
        {
            return false;
        }
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)1] = kickedAuthID;
        CombatPlayer playerByAuthID = PlayerManager.Instance.GetPlayerByAuthID(kickedAuthID);
        if ((Object)playerByAuthID == (Object)null)
        {
            return false;
        }
        hashtable[(byte)5] = playerByAuthID.playerID;
        hashtable[(byte)9] = voteReason;
        NetworkManager.Instance.SendKickVote(hashtable);
        return true;
    }

    public bool Vote(bool vote)
    {
        if (!this.CanVote())
        {
            return false;
        }
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)1] = this.kickedAuthID;
        CombatPlayer playerByAuthID = PlayerManager.Instance.GetPlayerByAuthID(this.kickedAuthID);
        if ((Object)playerByAuthID == (Object)null)
        {
            return false;
        }
        hashtable[(byte)5] = playerByAuthID.playerID;
        hashtable[(byte)2] = vote;
        NetworkManager.Instance.SendKickVote(hashtable);
        return true;
    }
}


