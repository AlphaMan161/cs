// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEnhancer
{
    protected float canFireTime;

    protected int reload = 9;

    protected int Index;

    private int enhancer_id;

    private bool active = true;

    private long activateTime;

    private string name;

    private int count;

    private int reloadTime;

    public Dictionary<int, EnhancerValue> Values;

    public int EnhancerID
    {
        get
        {
            return this.enhancer_id;
        }
        set
        {
            this.enhancer_id = value;
        }
    }

    public string Name
    {
        get
        {
            return this.name;
        }
        set
        {
            this.name = value;
        }
    }

    public int Count
    {
        get
        {
            return this.count;
        }
        set
        {
            this.count = value;
        }
    }

    public int ReloadTime
    {
        get
        {
            return this.reloadTime;
        }
        set
        {
            this.reloadTime = value;
        }
    }

    public CombatEnhancer()
    {
    }

    public CombatEnhancer(int Index, Hashtable enhancerData)
    {
        this.Values = new Dictionary<int, EnhancerValue>();
        this.Index = Index;
        this.enhancer_id = (int)enhancerData[(byte)57];
        this.name = (string)enhancerData[(byte)56];
        this.count = (int)enhancerData[(byte)55];
        this.reloadTime = (int)enhancerData[(byte)49] + 10;
        if (this.reloadTime < 100)
        {
            this.reloadTime = 110;
        }
        Dictionary<int, Hashtable> dictionary = (Dictionary<int, Hashtable>)enhancerData[(byte)54];
        Dictionary<int, Hashtable>.Enumerator enumerator = dictionary.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, Hashtable> current = enumerator.Current;
                EnhancerValue value = new EnhancerValue {
                    Key = (string)current.Value[(byte)52],
                    Value = (int)current.Value[(byte)50],
                    Type = (EnhancerValueType)(byte)current.Value[(byte)51]
                };
                this.Values.Add(current.Key, value);
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public void UpdateCount(int count)
    {
        this.count = count;
    }

    public void AddCount(int amount)
    {
        this.count += amount;
    }

    public void Shoot()
    {
        this.count--;
        if (this.count < 0)
        {
            UnityEngine.Debug.Log("NOOO AMMOOO SHOOOT!!!");
        }
    }

    public int getIndex()
    {
        return this.Index + 1;
    }

    public int getCount()
    {
        return this.count;
    }

    public bool Fire()
    {
        if (!this.active)
        {
            return false;
        }
        if (Time.time > this.canFireTime)
        {
            this.doReload();
            return true;
        }
        return false;
    }

    public void Activate()
    {
        this.active = true;
        UnityEngine.Debug.Log("Turn Speed: OFF period = " + (DateTime.Now.Ticks - this.activateTime) / 10000);
    }

    public void Deactivate()
    {
        this.activateTime = DateTime.Now.Ticks;
        UnityEngine.Debug.Log("Turn Speed: ON");
        this.active = false;
    }

    public bool doReload()
    {
        this.reload = 0;
        this.canFireTime = Time.time + (float)this.reloadTime / 1000f;
        return true;
    }

    public int getReload()
    {
        if (this.reload != 9)
        {
            this.reload = 8 - (int)Mathf.Ceil(9000f * (this.canFireTime - Time.time) / (float)this.reloadTime);
            if (this.reload > 9)
            {
                this.reload = 9;
            }
        }
        return this.reload;
    }
}


