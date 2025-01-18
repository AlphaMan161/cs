// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WeekItemsManager
{
    public delegate void WeekItemsManagerHandler(object sender);

    private static WeekItemsManager hInstance = null;

    private static object syncLook = new object();

    private Dictionary<CCItemType, WeekItem> weekItems = new Dictionary<CCItemType, WeekItem>();

    private static WeekItemsManager Instance
    {
        get
        {
            if (WeekItemsManager.hInstance == null)
            {
                object obj = WeekItemsManager.syncLook;
                Monitor.Enter(obj);
                try
                {
                    if (WeekItemsManager.hInstance == null)
                    {
                        WeekItemsManager.hInstance = new WeekItemsManager();
                    }
                }
                finally
                {
                    Monitor.Exit(obj);
                }
            }
            return WeekItemsManager.hInstance;
        }
    }

    public static event WeekItemsManagerHandler OnLoad;

    public static event WeekItemsManagerHandler OnError;

    public static void Init(object json)
    {
        WeekItemsManager.Instance.OnInit(json, null);
    }

    private void OnInit(object result, AjaxRequest request)
    {
        JSONNode jSONNode = (result.GetType() != typeof(JSONArray)) ? JSON.Parse(result.ToString()) : ((JSONArray)result);
        if (!(jSONNode == (object)null))
        {
            WeekItem weekItem = null;
            foreach (JSONNode child in jSONNode.Childs)
            {
                weekItem = new WeekItem(child);
                this.weekItems.Add(weekItem.Type, weekItem);
                UnityEngine.Debug.LogError(weekItem.ToString());
            }
        }
    }

    public static WeekItem GetWeekItem(Weapon weapon)
    {
        Dictionary<CCItemType, WeekItem>.Enumerator enumerator = WeekItemsManager.Instance.weekItems.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<CCItemType, WeekItem> current = enumerator.Current;
                if (current.Key == CCItemType.WEAPON && current.Value.ItemID == weapon.WeaponID)
                {
                    return current.Value;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return null;
    }

    public static WeekItem GetWeekItem(Assemblage assemblage)
    {
        Dictionary<CCItemType, WeekItem>.Enumerator enumerator = WeekItemsManager.Instance.weekItems.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<CCItemType, WeekItem> current = enumerator.Current;
                if (current.Key == CCItemType.ASEEMBLAGE && current.Value.ItemID == assemblage.ID)
                {
                    return current.Value;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return null;
    }
}


