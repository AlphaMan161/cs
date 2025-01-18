// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;

public class League
{
    private List<UserRating> users = new List<UserRating>();

    private uint minExp;

    private uint maxExp;

    public List<UserRating> Users
    {
        get
        {
            return this.users;
        }
    }

    public uint MinExp
    {
        get
        {
            return this.minExp;
        }
    }

    public uint MaxExp
    {
        get
        {
            return this.maxExp;
        }
    }

    public League(JSONNode jsonData, JSONNode jsonLimits)
    {
        this.minExp = Convert.ToUInt32(jsonLimits[0].AsInt);
        this.maxExp = Convert.ToUInt32(jsonLimits[1].AsInt);
        this.users = UserRating.UserRaringFromList(jsonData);
        if (this.users.Count > 100)
        {
            this.users[100].Show = false;
        }
    }

    public void ReSort()
    {
        this.users = (from u in this.users
        orderby u.Exp descending
        select u).ToList();
        int num = 1;
        List<UserRating>.Enumerator enumerator = this.users.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UserRating current = enumerator.Current;
                if (num > 100 && current.UserID != LocalUser.UserID)
                {
                    current.Show = false;
                }
                else
                {
                    current.Show = true;
                }
                if (current.Place == string.Empty)
                {
                    current.Place = ((num <= 100) ? num.ToString() : "100+");
                }
                num++;
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }
}


