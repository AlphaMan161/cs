// ILSpyBased#2
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DurationManager
{
    public delegate void DurationEventHandler(object sender);

    private static DurationManager hInstance = null;

    private static object syncLook = new object();

    private Dictionary<Duration, object> durationList = new Dictionary<Duration, object>();

    private float lastCalculateTime;

    private float nextCalculateTime;

    public static DurationManager Instance
    {
        get
        {
            if (DurationManager.hInstance == null)
            {
                object obj = DurationManager.syncLook;
                Monitor.Enter(obj);
                try
                {
                    if (DurationManager.hInstance == null)
                    {
                        DurationManager.hInstance = new DurationManager();
                    }
                }
                finally
                {
                    Monitor.Exit(obj);
                }
            }
            return DurationManager.hInstance;
        }
    }

    public static event DurationEventHandler OnEnd;

    public static bool Add(Duration duration, object owner)
    {
        if (DurationManager.Instance.durationList.ContainsKey(duration))
        {
            throw new Exception("[DurationManager] this duration exists");
        }
        DurationManager.Instance.durationList.Add(duration, owner);
        DurationManager.Instance.nextCalculateTime = 0f;
        return true;
    }

    private void Calc()
    {
        this.lastCalculateTime = Time.time;
        List<Duration> list = new List<Duration>();
        this.nextCalculateTime = 3.40282347E+38f;
        float num = 0f;
        Dictionary<Duration, object>.Enumerator enumerator = this.durationList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<Duration, object> current = enumerator.Current;
                current.Key.Calc();
                if (current.Key.TotalSec == 0L)
                {
                    list.Add(current.Key);
                }
                num = ((current.Key.Day <= 0) ? ((current.Key.Hour == 0) ? (Time.time + 60f) : (Time.time + 3600f)) : (Time.time + 86400f));
                if (this.nextCalculateTime > num)
                {
                    this.nextCalculateTime = num;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        if (this.durationList.Count == 0)
        {
            this.nextCalculateTime = 3.40282347E+38f;
        }
        if (list.Count > 0)
        {
            List<Duration>.Enumerator enumerator2 = list.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    Duration current2 = enumerator2.Current;
                    if (DurationManager.OnEnd != null)
                    {
                        DurationManager.OnEnd(this.durationList[current2]);
                    }
                    this.durationList.Remove(current2);
                }
            }
            finally
            {
                ((IDisposable)enumerator2).Dispose();
            }
        }
    }

    public void LateUpdate()
    {
        if (DurationManager.Instance.nextCalculateTime < Time.time)
        {
            DurationManager.Instance.Calc();
        }
    }
}


