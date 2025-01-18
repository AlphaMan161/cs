// ILSpyBased#2
public class Player
{
    protected int user_id;

    protected short level;

    protected string name;

    protected bool isPremium;

    public int UserID
    {
        get
        {
            return this.user_id;
        }
    }

    public short Level
    {
        get
        {
            return this.level;
        }
    }

    public string Name
    {
        get
        {
            return this.name;
        }
    }

    public bool Premium
    {
        get
        {
            return this.isPremium;
        }
    }

    public override string ToString()
    {
        return string.Format("[Player: UserID={0}, Level={1}, Name={2}, Premium={3}]", this.UserID, this.Level, this.Name, this.Premium);
    }
}


