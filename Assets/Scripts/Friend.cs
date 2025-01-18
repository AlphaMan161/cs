// ILSpyBased#2
public class Friend : SocialPlayer
{
    public Friend(int user_id, string name, short lvl, uint exp, UserStatus status, UserState state)
        : base(user_id, name, lvl, status, state)
    {
    }

    public override string ToString()
    {
        return string.Format("user_id[{0}] name[{1}] lvl[{2}]", base.UserID, base.Name, base.Level);
    }
}


