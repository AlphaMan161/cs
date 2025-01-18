// ILSpyBased#2
public class SocialPlayer : Player
{
    protected UserStatus status = UserStatus.Offline;

    private RoomInfo roomInfo;

    protected UserState state;

    public UserStatus Status
    {
        get
        {
            return this.status;
        }
        set
        {
            this.status = value;
        }
    }

    public RoomInfo RoomInfo
    {
        get
        {
            return this.roomInfo;
        }
        set
        {
            this.roomInfo = value;
        }
    }

    public UserState State
    {
        get
        {
            return this.state;
        }
        set
        {
            this.state = value;
        }
    }

    public SocialPlayer(int user_id, string name, short lvl, UserStatus status, UserState state)
    {
        base.user_id = user_id;
        base.name = name;
        base.level = lvl;
        this.status = status;
        this.state = state;
    }
}


