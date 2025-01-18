// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Assemblage
{
    private ushort id;

    private List<Wear> wears = new List<Wear>();

    private List<Weapon> weapons = new List<Weapon>();

    private string name = string.Empty;

    private string descAdditional = string.Empty;

    private string ndescAdditional = string.Empty;

    private WeekItem weekItem;

    public ushort ID
    {
        get
        {
            return this.id;
        }
    }

    public List<Wear> Wears
    {
        get
        {
            return this.wears;
        }
    }

    public List<Weapon> Weapons
    {
        get
        {
            return this.weapons;
        }
    }

    public string Name
    {
        get
        {
            return LanguageManager.GetText(this.name);
        }
    }

    public string DescAdditional
    {
        get
        {
            return LanguageManager.GetText(this.descAdditional);
        }
    }

    public string NDescAdditional
    {
        get
        {
            return LanguageManager.GetText(this.ndescAdditional);
        }
    }

    public WeekItem WeekItemRel
    {
        get
        {
            return this.weekItem;
        }
    }

    public Assemblage(JSONNode obj)
    {
        this.id = Convert.ToUInt16(obj["id"].AsInt);
        this.name = "assemblage_" + this.id + "_name";
        this.descAdditional = "assemblage_" + this.id + "_desca";
        this.ndescAdditional = "assemblage_" + this.id + "_ndesc";
        JSONObject jSONObject = new JSONObject(obj["items"].Value);
        if (jSONObject == null || jSONObject.type != JSONObject.Type.ARRAY)
        {
            UnityEngine.Debug.LogError("[Assemblage] items in json is incorrect");
        }
        else
        {
            for (int i = 0; i < jSONObject.Count; i++)
            {
                if (jSONObject[i].type != 0)
                {
                    switch ((jSONObject[i].GetField("it").type != JSONObject.Type.NUMBER) ? Convert.ToInt32(jSONObject[i].GetField("it").str) : Convert.ToInt32(jSONObject[i].GetField("it").n))
                    {
                        case 3:
                        {
                            Wear item2 = new Wear(jSONObject[i]);
                            this.wears.Add(item2);
                            break;
                        }
                        case 1:
                        {
                            Weapon item = new Weapon(jSONObject[i]);
                            this.weapons.Add(item);
                            break;
                        }
                    }
                }
            }
            this.weekItem = WeekItemsManager.GetWeekItem(this);
        }
    }

    public bool Contain(Wear wear)
    {
        List<Wear>.Enumerator enumerator = this.wears.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Wear current = enumerator.Current;
                if (current.WearID == wear.WearID)
                {
                    return true;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return false;
    }

    public bool Contain(Weapon weapon)
    {
        List<Weapon>.Enumerator enumerator = this.weapons.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Weapon current = enumerator.Current;
                if (current.WeaponID == weapon.WeaponID)
                {
                    return true;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        return false;
    }
}


