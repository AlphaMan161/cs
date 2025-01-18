// ILSpyBased#2
using System;
using UnityEngine;

public class Duration
{
    private long totalSec;

    private ushort day;

    private uint min;

    private uint hour;

    private float lastCalculateTime;

    public long TotalSec
    {
        get
        {
            return this.totalSec;
        }
        set
        {
            if (value != this.totalSec)
            {
                this.totalSec = value;
                this.Calc();
            }
        }
    }

    public ushort Day
    {
        get
        {
            return this.day;
        }
    }

    public uint Min
    {
        get
        {
            return this.min;
        }
    }

    public uint Hour
    {
        get
        {
            return this.hour;
        }
    }

    public Duration(long sec)
    {
        this.totalSec = sec;
        this.Calc();
    }

    public void Calc()
    {
        float num = Time.time - this.lastCalculateTime;
        if (num < 0f)
        {
            num = 0f;
        }
        if ((float)this.totalSec < num)
        {
            this.totalSec = 0L;
        }
        else
        {
            this.totalSec -= Convert.ToUInt32(num);
        }
        this.day = Convert.ToUInt16(this.totalSec / 86400);
        this.hour = Convert.ToUInt32(this.totalSec / 3600);
        this.min = Convert.ToUInt32(this.totalSec / 60);
        this.lastCalculateTime = Time.time;
    }

    public override string ToString()
    {
        if (this.hour != 0)
        {
            return LanguageManager.GetTextFormat("{0} h.", this.hour);
        }
        return LanguageManager.GetText("less than hour");
    }
}


