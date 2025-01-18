// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections.Generic;

public class YesterDayBest
{
    private UserRating kill;

    private UserRating exp;

    private UserRating point;

    private UserRating flag;

    private UserRating domination;

    private UserRating nut;

    private UserRating head;

    private UserRating assist;

    private int count;

    public UserRating Kill
    {
        get
        {
            return this.kill;
        }
    }

    public UserRating Exp
    {
        get
        {
            return this.exp;
        }
    }

    public UserRating Point
    {
        get
        {
            return this.point;
        }
    }

    public UserRating Flag
    {
        get
        {
            return this.flag;
        }
    }

    public UserRating Domination
    {
        get
        {
            return this.domination;
        }
    }

    public UserRating Nut
    {
        get
        {
            return this.nut;
        }
    }

    public UserRating Head
    {
        get
        {
            return this.head;
        }
    }

    public UserRating Assist
    {
        get
        {
            return this.assist;
        }
    }

    public int Count
    {
        get
        {
            return this.count;
        }
    }

    public YesterDayBest(JSONNode json)
    {
        List<UserRating> list = UserRating.UserRaringFromList(json);
        this.count = list.Count;
        List<UserRating>.Enumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UserRating current = enumerator.Current;
                if (this.kill == null || this.kill.Kill < current.Kill)
                {
                    this.kill = current;
                }
                if (this.exp == null || this.exp.Exp < current.Exp)
                {
                    this.exp = current;
                }
                if (this.point == null || this.point.ControlPoint < current.ControlPoint)
                {
                    this.point = current;
                }
                if (this.flag == null || this.flag.Flag < current.Flag)
                {
                    this.flag = current;
                }
                if (this.domination == null || this.domination.Domination < current.Domination)
                {
                    this.domination = current;
                }
                if (this.nut == null || this.nut.Nuts < current.Nuts)
                {
                    this.nut = current;
                }
                if (this.head == null || this.head.Head < current.Head)
                {
                    this.head = current;
                }
                if (this.assist == null || this.assist.Assist < current.Assist)
                {
                    this.assist = current;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }
}


