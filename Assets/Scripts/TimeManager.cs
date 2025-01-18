// ILSpyBased#2
using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;

    private long timeShift;

    private long serverTime;

    private NetworkManager networkManager;

    public long TimeShift
    {
        get
        {
            return this.timeShift;
        }
    }

    public static TimeManager Instance
    {
        get
        {
            return TimeManager.instance;
        }
    }

    private NetworkManager NetworkManager
    {
        get
        {
            if ((UnityEngine.Object)this.networkManager == (UnityEngine.Object)null)
            {
                this.networkManager = ((Component)base.transform).GetComponent<NetworkManager>();
            }
            return this.networkManager;
        }
    }

    public long NetworkTime
    {
        get
        {
            if ((UnityEngine.Object)this.NetworkManager != (UnityEngine.Object)null)
            {
                if (this.serverTime == 0L)
                {
                    this.serverTime = this.NetworkManager.getServerTimestamp();
                }
                return this.serverTime;
            }
            return DateTime.Now.ToBinary() / 10000;
        }
    }

    public int AveragePing
    {
        get
        {
            if ((UnityEngine.Object)this.NetworkManager != (UnityEngine.Object)null)
            {
                return this.NetworkManager.getAveragePing();
            }
            return 0;
        }
    }

    private void Awake()
    {
        TimeManager.instance = this;
    }

    private void Update()
    {
        if (!((UnityEngine.Object)this.NetworkManager == (UnityEngine.Object)null))
        {
            this.timeShift = this.NetworkManager.getLocalTimestamp() - this.NetworkManager.getServerTimestamp();
            this.serverTime = this.NetworkManager.getServerTimestamp();
        }
    }
}


