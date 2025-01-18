// ILSpyBased#2
using System;
using System.Collections.Generic;

namespace GameLogic.Ability
{
    public class AbilityValues
    {
    static Dictionary<string, int> _003C_003Ef__switch_0024map9;
        private List<AbilityValue> abilityValue = new List<AbilityValue>();

        public List<AbilityValue> AbilityValue
        {
            get
            {
                return this.abilityValue;
            }
        }

        public AbilityValues(string value)
        {
            this.AddValues(value);
        }

        public void AddValues(string value)
        {
            if (!(value == string.Empty))
            {
                List<Dictionary<string, string>> list = null;
                try
                {
                    JSONObject jSONObject = new JSONObject(value);
                    foreach (object item in jSONObject.list)
                    {
                        if (item != null && item.GetType() == typeof(JSONObject))
                        {
                            JSONObject jSONObject2 = (JSONObject)item;
                            Dictionary<string, string> dictionary = new Dictionary<string, string>();
                            foreach (object key in jSONObject2.keys)
                            {
                                if (jSONObject2[key.ToString()] != null)
                                {
                                    dictionary.Add(key.ToString(), jSONObject2[key.ToString()].str);
                                }
                            }
                            if (list == null)
                            {
                                list = new List<Dictionary<string, string>>();
                            }
                            list.Add(dictionary);
                        }
                    }
                }
                catch (Exception)
                {
                }
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        string value2 = string.Empty;
                        string name = string.Empty;
                        string value3 = string.Empty;
                        Dictionary<string, string>.Enumerator enumerator3 = list[i].GetEnumerator();
                        try
                        {
                            while (enumerator3.MoveNext())
                            {
                                KeyValuePair<string, string> current3 = enumerator3.Current;
                                if (current3.Key.Equals("t"))
                                {
                                    value2 = current3.Value;
                                }
                                else
                                {
                                    name = current3.Key;
                                    value3 = current3.Value;
                                }
                            }
                        }
                        finally
                        {
                            ((IDisposable)enumerator3).Dispose();
                        }
                        this.abilityValue.Add(new AbilityValue((EstimateType)Convert.ToInt32(value2), AbilityValues.GetByName(name), Convert.ToInt32(value3)));
                    }
                }
            }
        }

        private static ValueType GetByName(string name)
        {
            if (name != null)
            {
                if (AbilityValues._003C_003Ef__switch_0024map9 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(20);
                    dictionary.Add("cdef", 0);
                    dictionary.Add("cheal", 1);
                    dictionary.Add("cspd", 2);
                    dictionary.Add("cdecdam", 3);
                    dictionary.Add("wrap", 4);
                    dictionary.Add("wcrit", 5);
                    dictionary.Add("wam", 6);
                    dictionary.Add("wmdam", 7);
                    dictionary.Add("wmxdam", 8);
                    dictionary.Add("wacc", 9);
                    dictionary.Add("whcrit", 10);
                    dictionary.Add("wang", 11);
                    dictionary.Add("wrel", 12);
                    dictionary.Add("cboom", 13);
                    dictionary.Add("cmed", 14);
                    dictionary.Add("zzdam", 15);
                    dictionary.Add("zzheal", 16);
                    dictionary.Add("zzdecdam", 17);
                    dictionary.Add("zhdam", 18);
                    dictionary.Add("zhdecdam", 19);
                    AbilityValues._003C_003Ef__switch_0024map9 = dictionary;
                }
                int num = default(int);
                if (AbilityValues._003C_003Ef__switch_0024map9.TryGetValue(name, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return ValueType.CharacterDefense;
                        case 1:
                            return ValueType.CharacterHealth;
                        case 2:
                            return ValueType.CharacterSpeed;
                        case 3:
                            return ValueType.CharacterDescentDamage;
                        case 4:
                            return ValueType.WeaponRapidity;
                        case 5:
                            return ValueType.WeaponCrit;
                        case 6:
                            return ValueType.WeaponAmmo;
                        case 7:
                            return ValueType.WeaponMinimalDamage;
                        case 8:
                            return ValueType.WeaponMaximalDamage;
                        case 9:
                            return ValueType.WeaponAccuracy;
                        case 10:
                            return ValueType.WeaponHeadCrit;
                        case 11:
                            return ValueType.WeaponAngle;
                        case 12:
                            return ValueType.WeaponReload;
                        case 13:
                            return ValueType.CharacterKamikaze;
                        case 14:
                            return ValueType.CharacterMedication;
                        case 15:
                            return ValueType.ZombieDamage;
                        case 16:
                            return ValueType.ZombieHealth;
                        case 17:
                            return ValueType.ZombieDescentDamage;
                        case 18:
                            return ValueType.ZMDamage;
                        case 19:
                            return ValueType.ZMDescentDamage;
                    }
                }
            }
            return ValueType.None;
        }

        public void Clean()
        {
            this.abilityValue.Clear();
        }
    }
}


