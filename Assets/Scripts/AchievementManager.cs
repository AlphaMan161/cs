// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AchievementManager
{
    public delegate void AchievementManagerHandler(object sender);

    private static AchievementManager hInstance;

    private Dictionary<long, Achievement> baseList;

    private List<Achievement> showedList;

    private bool needLiteRefresh;

    public static AchievementManager Instance
    {
        get
        {
            if (AchievementManager.hInstance == null)
            {
                AchievementManager.hInstance = new AchievementManager();
            }
            return AchievementManager.hInstance;
        }
    }

    public Dictionary<long, Achievement> BaseList
    {
        get
        {
            return AchievementManager.Instance.baseList;
        }
    }

    public List<Achievement> ShowedList
    {
        get
        {
            return AchievementManager.Instance.showedList;
        }
    }

    public static bool NeedLiteRefresh
    {
        get
        {
            return AchievementManager.Instance.needLiteRefresh;
        }
        set
        {
            AchievementManager.Instance.needLiteRefresh = value;
        }
    }

    public static event AchievementManagerHandler OnLoad;

    public static event AchievementManagerHandler OnError;

    private AchievementManager()
    {
    }

    public void Init()
    {
        UnityEngine.Debug.Log("[AchievementManager] Init " + Time.time);
        Ajax.Request(WebUrls.ACHIEVEMENT_URL, new AjaxRequest.AjaxHandler(AchievementManager.Instance.OnAchievements));
    }

    public void Update(Hashtable input)
    {
        IDictionaryEnumerator enumerator = input.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
                Achievement achievement = this.baseList[(int)dictionaryEntry.Key];
                if (achievement != null)
                {
                    achievement.Update((Hashtable)dictionaryEntry.Value);
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        this.InitShowedList();
    }

    private void OnAchievements(object result, AjaxRequest request)
    {
        JSONNode jSONNode = JSON.Parse(result.ToString());
        if (jSONNode["result"] != (object)null && jSONNode["result"].AsBool)
        {
            this.BaseInfo(jSONNode);
            if (AchievementManager.OnLoad != null)
            {
                AchievementManager.OnLoad(this);
            }
            return;
        }
        if (AchievementManager.OnError != null)
        {
            AchievementManager.OnError(result);
            return;
        }
        throw new Exception("[AchievementManager] OnAchievement not init: " + result);
    }

    public static void RefreshLite()
    {
        Ajax.Request(WebUrls.ACHIEVEMENT_LITE_URL, new AjaxRequest.AjaxHandler(AchievementManager.Instance.OnRefreshLite));
    }

    private void OnRefreshLite(object result, AjaxRequest request)
    {
        JSONNode jSONNode = JSON.Parse(result.ToString());
        if (jSONNode["result"] != (object)null && jSONNode["result"].AsBool)
        {
            this.BaseInfo(jSONNode);
            return;
        }
        if (AchievementManager.OnError != null)
        {
            AchievementManager.OnError(result);
            return;
        }
        throw new Exception("[AchievementManager] OnAchievement not init: " + result);
    }

    private void BaseInfo(JSONNode json)
    {
        JSONNode jSONNode = json["b"];
        StringBuilder stringBuilder = new StringBuilder();
        if (jSONNode != (object)null)
        {
            this.baseList = new Dictionary<long, Achievement>();
            foreach (JSONNode child in jSONNode.Childs)
            {
                long num = Convert.ToInt64(child["i"].Value);
                int asInt = child["v"].AsInt;
                int asInt2 = child["r"].AsInt;
                int num2 = (child["id"] != (object)null) ? child["id"].AsInt : 0;
                int usrLvl = (child["ul"] != (object)null) ? child["ul"].AsInt : 0;
                if (num2 != 0)
                {
                    this.baseList.Add(num2, new Achievement(num2, usrLvl, num, asInt, asInt2));
                }
                else
                {
                    this.baseList.Add(num, new Achievement(num2, usrLvl, num, asInt, asInt2));
                }
                Achievement achievement = new Achievement(num2, usrLvl, num, asInt, asInt2);
                stringBuilder.AppendLine(string.Format("ach{0} {1} {2}", achievement.AchievementID, achievement.Name, achievement.Reward));
            }
        }
        if (json["u"] != (object)null && json["u"]["data"] != (object)null)
        {
            JSONNode jSONNode2 = JSONNode.Parse(json["u"]["data"].Value);
            if (jSONNode2 != (object)null)
            {
                foreach (string key in jSONNode2.Keys)
                {
                    JSONNode jSONNode3 = jSONNode2[key];
                    long tmp_achievement_id = Convert.ToInt64(key);
                    bool complete = true;
                    if (jSONNode2[key]["c"].AsInt == 0)
                    {
                        complete = false;
                    }
                    int asInt3 = jSONNode2[key]["v"].AsInt;
                    if (this.baseList.ContainsKey(tmp_achievement_id))
                    {
                        this.baseList[tmp_achievement_id].Value = asInt3;
                        this.baseList[tmp_achievement_id].Complete = complete;
                    }
                    else
                    {
                        KeyValuePair<long, Achievement> keyValuePair = this.baseList.FirstOrDefault((KeyValuePair<long, Achievement> x) => x.Value.AchievementID == tmp_achievement_id);
                        if (keyValuePair.Value != null)
                        {
                            Achievement value = keyValuePair.Value;
                            this.baseList[value.ID].Value = asInt3;
                            this.baseList[value.ID].Complete = complete;
                        }
                        else
                        {
                            UnityEngine.Debug.LogError("[AchievementManager] Achievement " + tmp_achievement_id + " not exist " + keyValuePair + " currKey: " + key);
                        }
                    }
                }
            }
        }
        this.InitShowedList();
    }

    private void InitShowedList()
    {
        Dictionary<long, Achievement>.Enumerator enumerator = this.BaseList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<long, Achievement> current = enumerator.Current;
                Dictionary<long, Achievement>.Enumerator enumerator2 = this.BaseList.GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        KeyValuePair<long, Achievement> current2 = enumerator2.Current;
                        if (current.Value.AchievementSubID == current2.Value.AchievementSubID && current.Value.MaxLevel < current2.Value.Level)
                        {
                            current.Value.MaxLevel = current2.Value.Level;
                        }
                    }
                }
                finally
                {
                    ((IDisposable)enumerator2).Dispose();
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        this.showedList = new List<Achievement>();
        Dictionary<long, Achievement>.Enumerator enumerator3 = this.BaseList.GetEnumerator();
        try
        {
            while (enumerator3.MoveNext())
            {
                KeyValuePair<long, Achievement> current3 = enumerator3.Current;
                bool flag = false;
                if (current3.Value.Complete && current3.Value.Level != current3.Value.MaxLevel)
                {
                    flag = true;
                }
                List<Achievement>.Enumerator enumerator4 = this.showedList.GetEnumerator();
                try
                {
                    while (enumerator4.MoveNext())
                    {
                        Achievement current4 = enumerator4.Current;
                        if (current4.AchievementSubID == current3.Value.AchievementSubID)
                        {
                            flag = true;
                            if (current4.MaxLevel < current3.Value.Level)
                            {
                                current4.MaxLevel = current3.Value.Level;
                            }
                        }
                    }
                }
                finally
                {
                    ((IDisposable)enumerator4).Dispose();
                }
                if (!flag)
                {
                    this.showedList.Add(current3.Value);
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator3).Dispose();
        }
    }

    public void UnloadResource()
    {
        Dictionary<long, Achievement>.Enumerator enumerator = this.BaseList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.UnloadIco();
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }
}


