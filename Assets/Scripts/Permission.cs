// ILSpyBased#2
using SimpleJSON;

public class Permission : Player
{
    private bool admin;

    private bool password;

    private bool kick;

    private bool guest;

    public bool Admin
    {
        get
        {
            return this.admin;
        }
    }

    public bool Password
    {
        get
        {
            return this.password;
        }
    }

    public bool Kick
    {
        get
        {
            return this.kick;
        }
    }

    public bool Guest
    {
        get
        {
            return this.guest;
        }
    }

    public Permission()
    {
        this.SetDefault();
    }

    public Permission(JSONNode json)
    {
        this.Update(json);
    }

    public void Update(JSONNode json)
    {
        if (json != (object)null)
        {
            this.admin = (json["a"] != (object)null && json["a"].AsInt == 1);
            if (this.admin)
            {
                this.password = true;
                this.kick = true;
                this.guest = true;
            }
            else
            {
                this.password = (json["p"] != (object)null && json["p"].AsInt == 1);
                this.kick = (json["k"] != (object)null && json["k"].AsInt == 1);
                this.guest = (json["g"] != (object)null && json["g"].AsInt == 1);
            }
        }
        else
        {
            this.SetDefault();
        }
    }

    private void SetDefault()
    {
        this.admin = false;
        this.password = false;
        this.kick = false;
        this.guest = false;
    }

    public override string ToString()
    {
        return string.Format("[Permission: Admin={0}, Password={1}, Kick={2}, Guest={3}]", this.Admin, this.Password, this.Kick, this.Guest);
    }
}


