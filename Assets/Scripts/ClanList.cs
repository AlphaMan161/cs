// ILSpyBased#2
using System.Collections.Generic;

public class ClanList
{
    private List<Clan> clans = new List<Clan>();

    public List<Clan> Clans
    {
        get
        {
            return this.clans;
        }
    }

    public void Add(Clan clan)
    {
        this.clans.Add(clan);
    }
}


