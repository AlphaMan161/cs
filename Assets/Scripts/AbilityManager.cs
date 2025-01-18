// ILSpyBased#2
using GameLogic.Ability;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager
{
    public delegate void AbilityManagerHandler(object sender);

    private static AbilityManager hInstance;

    private List<Ability> baseList;

    private Dictionary<uint, short> userAbility;

    private AbilityValues userAbilityValues = new AbilityValues(string.Empty);

    private Dictionary<GameLogic.Ability.ValueType, List<AbilityValue>> userAbilityValuesCache = new Dictionary<GameLogic.Ability.ValueType, List<AbilityValue>>();

    private List<Ability> showedList = new List<Ability>();

    public static AbilityManager Instance
    {
        get
        {
            if (AbilityManager.hInstance == null)
            {
                AbilityManager.hInstance = new AbilityManager();
            }
            return AbilityManager.hInstance;
        }
    }

    public List<Ability> ShowedList
    {
        get
        {
            return this.showedList;
        }
    }

    public static event AbilityManagerHandler OnLoad;

    public static event AbilityManagerHandler OnError;

    public void Init()
    {
        Ajax.Request(WebUrls.ABILITY_URL, new AjaxRequest.AjaxHandler(AbilityManager.Instance.OnAbility));
    }

    private void InitShowedList()
    {
        this.showedList.Clear();
        this.userAbilityValues.Clean();
        this.userAbilityValuesCache.Clear();
        this.baseList.Sort(delegate(Ability a, Ability b)
        {
            if (a.AbilityID != b.AbilityID)
            {
                return (a.AbilityID > b.AbilityID) ? 1 : (-1);
            }
            return (a.Level > b.Level) ? 1 : (-1);
        });
        List<uint> list = new List<uint>();
        List<Ability>.Enumerator enumerator = this.baseList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Ability current = enumerator.Current;
                if (this.userAbility.ContainsKey(current.AbilityID))
                {
                    if (this.userAbility[current.AbilityID] == current.Level)
                    {
                        current.IsBuyed = true;
                        this.showedList.Add(current);
                        this.userAbilityValues.AddValues(current.Values);
                    }
                }
                else if (!list.Contains(current.AbilityID))
                {
                    list.Add(current.AbilityID);
                    this.showedList.Add(current);
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        List<Ability>.Enumerator enumerator2 = this.showedList.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                Ability current2 = enumerator2.Current;
                if (current2.Level < 5)
                {
                    List<Ability>.Enumerator enumerator3 = this.baseList.GetEnumerator();
                    try
                    {
                        while (enumerator3.MoveNext())
                        {
                            Ability current3 = enumerator3.Current;
                            if (current2.AbilityID == current3.AbilityID && current2.Level + 1 == current3.Level)
                            {
                                current2.NextLevel = current3;
                            }
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator3).Dispose();
                    }
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator2).Dispose();
        }
    }

    private void OnAbility(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(result.ToString());
        if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
        {
            this.baseList = new List<Ability>();
            JSONObject field = jSONObject.GetField("b");
            if (field != null && field.type == JSONObject.Type.ARRAY)
            {
                for (int i = 0; i < field.Count; i++)
                {
                    if (field[i].type != 0)
                    {
                        this.baseList.Add(new Ability(field[i]));
                    }
                }
            }
            this.userAbility = new Dictionary<uint, short>();
            JSONObject field2 = jSONObject.GetField("u");
            if (field2 != null && field2.type == JSONObject.Type.ARRAY)
            {
                for (int j = 0; j < field2.Count; j++)
                {
                    if (field2[j].type != 0)
                    {
                        this.userAbility.Add(Convert.ToUInt32(field2[j].GetField("i").n), Convert.ToInt16(field2[j].GetField("l").n));
                    }
                }
            }
            this.InitShowedList();
            if (AbilityManager.OnLoad != null)
            {
                AbilityManager.OnLoad(this);
            }
            return;
        }
        if (AbilityManager.OnError != null)
        {
            AbilityManager.OnError(result);
            return;
        }
        throw new Exception("[AbilityManager] OnAbility not init: " + result);
    }

    public void Buy(Ability ability)
    {
        if (ability.Cost.TimePVCost > LocalUser.Money)
        {
            ErrorInfo.CODE code = ErrorInfo.CODE.MISSING_MONEY;
            code.AddNotification(ErrorInfo.TYPE.BUY_ABILITY);
        }
        else
        {
            Notification notification = new Notification(Notification.Type.BUY_ITEM, ability.Name, string.Empty, LanguageManager.GetText("Buy"), new Notification.ButtonClick(this.OnBuyConfirmed), ability);
            notification.Item = ability;
            NotificationWindow.Add(notification);
        }
    }

    private void OnBuyConfirmed(object item)
    {
        Ability ability = item as Ability;
        UnityEngine.Debug.Log("[AbilityManager] Init " + Time.time);
        AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.BUY_ABILITY_URL + "&id=" + ability.AbilityID, ability);
        ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(this.OnBuy);
        Ajax.Request(ajaxRequest);
    }

    private void OnBuy(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(result.ToString());
        if (jSONObject.GetField("result") != null && jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
        {
            LocalUser.SubShopCost((request.Tag as Ability).Cost);
            (request.Tag as Ability).IsBuyed = true;
            if (this.userAbility.ContainsKey((request.Tag as Ability).AbilityID))
            {
                this.userAbility[(request.Tag as Ability).AbilityID] = (request.Tag as Ability).Level;
            }
            else
            {
                this.userAbility.Add((request.Tag as Ability).AbilityID, (request.Tag as Ability).Level);
            }
            Ability ability = request.Tag as Ability;
            GameLogicServerNetworkController.SendChange(4);
            WebCall.Analitic("BuyAbility", string.Format("{0}_{1}", ability.AbilityID, ability.Level));
            this.InitShowedList();
        }
    }

    public uint GetNewValue(GameLogic.Ability.ValueType type, uint value)
    {
        if (this.userAbilityValuesCache.ContainsKey(type) && this.userAbilityValuesCache[type] != null)
        {
            uint num = 0u;
            List<AbilityValue>.Enumerator enumerator = this.userAbilityValuesCache[type].GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AbilityValue current = enumerator.Current;
                    if (current.EstimateType == EstimateType.Point)
                    {
                        num = (uint)((int)num + current.Value);
                    }
                    else if (current.EstimateType == EstimateType.Percent)
                    {
                        num += Convert.ToUInt32((float)(double)value * (Convert.ToSingle(current.Value) * 0.01f));
                    }
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            value += num;
        }
        else
        {
            this.userAbilityValuesCache.Add(type, null);
            this.userAbilityValuesCache[type] = this.userAbilityValues.AbilityValue.FindAll((AbilityValue x) => x.ValueType == type).ToList();
        }
        return value;
    }

    public void UnloadResource()
    {
        List<Ability>.Enumerator enumerator = this.baseList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Ability current = enumerator.Current;
                current.UnloadIco();
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }
}


