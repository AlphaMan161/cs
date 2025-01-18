// ILSpyBased#2
public class RoomInfo
{
    protected string name;

    protected string filteredName;

    protected string serverConnectString;

    protected short userOnline;

    protected short userMax;

    public string Name
    {
        get
        {
            return this.name;
        }
    }

    public string FilteredName
    {
        get
        {
            return this.filteredName;
        }
    }

    public string ConnectionString
    {
        get
        {
            return this.serverConnectString;
        }
    }

    public short UserOnline
    {
        get
        {
            return this.userOnline;
        }
    }

    public short UserMax
    {
        get
        {
            return this.userMax;
        }
    }

    public RoomInfo(string name, string connectionString, short userOnline, short userMax)
    {
        this.name = name;
        this.filteredName = BadWorldFilter.CheckLite(name);
        this.serverConnectString = connectionString;
        this.userOnline = userOnline;
        this.userMax = userMax;
    }
}


