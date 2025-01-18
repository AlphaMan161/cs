// ILSpyBased#2
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserRatingAdvanced : Player
{
    private int exp;

    private int minExp;

    private int maxExp;

    private long kill;

    private long death;

    private int suicide;

    private long headShot;

    private long nutsShot;

    private long playedTime;

    private string playedTimeStr = string.Empty;

    private uint win;

    private uint lose;

    private long deathHeadShot;

    private long deathNutsShot;

    private long domination;

    private long revenge;

    private int maxDomination;

    private int maxRevenge;

    private long shot;

    private long hit;

    private float accuracy;

    private float kd;

    private List<UserRatingWeapon> weaponStat = new List<UserRatingWeapon>();

    private List<UserRatingGameMode> gameModeStat = new List<UserRatingGameMode>();

    private List<UserRatingMap> mapStat = new List<UserRatingMap>();

    private bool isLoaded;

    private UserRatingWeapon favoriteWeapon;

    private UserRatingGameMode favoriteMode;

    private UserRatingMap favoriteMap;

    private string socialUrl = string.Empty;

    private Clan clan;

    public PlayerView View;

    public WeaponSlot WeaponSlot;

    public new int UserID
    {
        get
        {
            return base.user_id;
        }
    }

    public new string Name
    {
        get
        {
            return base.name;
        }
    }

    public new short Level
    {
        get
        {
            return base.level;
        }
    }

    public int Exp
    {
        get
        {
            return this.exp;
        }
    }

    public int MinExp
    {
        get
        {
            return this.minExp;
        }
    }

    public int MaxExp
    {
        get
        {
            return this.maxExp;
        }
    }

    public long Kill
    {
        get
        {
            return this.kill;
        }
    }

    public long Death
    {
        get
        {
            return this.death;
        }
    }

    public int Suicide
    {
        get
        {
            return this.suicide;
        }
    }

    public long HeadShot
    {
        get
        {
            return this.headShot;
        }
    }

    public long NutsShot
    {
        get
        {
            return this.nutsShot;
        }
    }

    public long PlayedTime
    {
        get
        {
            return this.playedTime;
        }
    }

    public string PlayedTimeString
    {
        get
        {
            return this.playedTimeStr;
        }
    }

    public uint Win
    {
        get
        {
            return this.win;
        }
    }

    public uint Lose
    {
        get
        {
            return this.lose;
        }
    }

    public long DeathHeadShot
    {
        get
        {
            return this.deathHeadShot;
        }
    }

    public long DeathNutsShot
    {
        get
        {
            return this.deathNutsShot;
        }
    }

    public long Domination
    {
        get
        {
            return this.domination;
        }
    }

    public long Revenge
    {
        get
        {
            return this.revenge;
        }
    }

    public int MaxDomination
    {
        get
        {
            return this.maxDomination;
        }
    }

    public int MaxRevenge
    {
        get
        {
            return this.maxRevenge;
        }
    }

    public long Shot
    {
        get
        {
            return this.shot;
        }
    }

    public long Hit
    {
        get
        {
            return this.hit;
        }
    }

    public float Accuracy
    {
        get
        {
            return this.accuracy;
        }
    }

    public float KD
    {
        get
        {
            return this.kd;
        }
    }

    public List<UserRatingWeapon> WeaponStat
    {
        get
        {
            return this.weaponStat;
        }
    }

    public List<UserRatingGameMode> GameModeStat
    {
        get
        {
            return this.gameModeStat;
        }
    }

    public List<UserRatingMap> MapStat
    {
        get
        {
            return this.mapStat;
        }
    }

    public bool Ready
    {
        get
        {
            return this.isLoaded;
        }
    }

    public UserRatingWeapon FavoriteWeapon
    {
        get
        {
            return this.favoriteWeapon;
        }
    }

    public UserRatingGameMode FavoriteMode
    {
        get
        {
            return this.favoriteMode;
        }
    }

    public UserRatingMap FavoriteMap
    {
        get
        {
            return this.favoriteMap;
        }
    }

    public string SocialUrl
    {
        get
        {
            return this.socialUrl;
        }
    }

    public Clan Clan
    {
        get
        {
            return this.clan;
        }
    }

    public UserRatingAdvanced()
    {
    }

    public UserRatingAdvanced(JSONObject json, bool isLocal)
    {
        this.Update(json, isLocal);
    }

    public void Update(JSONObject json, bool isLocal)
    {
        this.kill = 0L;
        this.death = 0L;
        this.suicide = 0;
        this.headShot = 0L;
        this.nutsShot = 0L;
        this.playedTime = 0L;
        this.playedTimeStr = "0";
        this.win = 0u;
        this.lose = 0u;
        this.deathHeadShot = 0L;
        this.deathNutsShot = 0L;
        this.domination = 0L;
        this.revenge = 0L;
        this.maxDomination = 0;
        this.maxRevenge = 0;
        this.shot = 0L;
        this.hit = 0L;
        this.accuracy = 0f;
        this.weaponStat.Clear();
        this.gameModeStat.Clear();
        this.mapStat.Clear();
        if (!isLocal)
        {
            JSONObject field = json.GetField("info");
            JSONObject field2 = json.GetField("info").GetField("exp");
            base.user_id = Convert.ToInt32(field.GetField("u_id").n);
            base.name = BadWorldFilter.CheckLite(Ajax.DecodeUtf(field.GetField("un").str));
            base.level = Convert.ToInt16(field.GetField("lvl").n);
            this.exp = Convert.ToInt32(field2.GetField("cur").n);
            this.minExp = Convert.ToInt32(field2.GetField("min").n);
            this.maxExp = Convert.ToInt32(field2.GetField("max").n);
            if (json.GetField("cl") != null)
            {
                this.clan = new Clan(JSONNode.Parse(json["cl"].input_str));
                UnityEngine.Debug.LogError(this.clan.ToString());
                base.name = string.Format("[{0}]{1}", this.clan.Tag, base.name);
            }
            JSONObject field3 = json.GetField("view");
            JSONObject field4 = json.GetField("weap");
            List<Weapon> list = new List<Weapon>();
            if (field4.GetField("id1").type == JSONObject.Type.OBJECT)
            {
                list.Add(new Weapon(field4.GetField("id1")));
            }
            if (field4.GetField("id2").type == JSONObject.Type.OBJECT)
            {
                list.Add(new Weapon(field4.GetField("id2")));
            }
            if (field4.GetField("id3").type == JSONObject.Type.OBJECT)
            {
                list.Add(new Weapon(field4.GetField("id3")));
            }
            if (field4.GetField("id4").type == JSONObject.Type.OBJECT)
            {
                list.Add(new Weapon(field4.GetField("id4")));
            }
            if (field4.GetField("id5").type == JSONObject.Type.OBJECT)
            {
                list.Add(new Weapon(field4.GetField("id5")));
            }
            if (field4.GetField("id6").type == JSONObject.Type.OBJECT)
            {
                list.Add(new Weapon(field4.GetField("id6")));
            }
            if (field4.GetField("id7").type == JSONObject.Type.OBJECT)
            {
                list.Add(new Weapon(field4.GetField("id7")));
            }
            this.WeaponSlot = new WeaponSlot(list.ToArray());
            this.View = new PlayerView();
            if (field3.GetField("hat").type == JSONObject.Type.OBJECT)
            {
                this.View.DressUp(new Wear(field3.GetField("hat")));
            }
            if (field3.GetField("head").type == JSONObject.Type.OBJECT)
            {
                this.View.DressUp(new Wear(field3.GetField("head")));
            }
            if (field3.GetField("mask").type == JSONObject.Type.OBJECT)
            {
                this.View.DressUp(new Wear(field3.GetField("mask")));
            }
            if (field3.GetField("gloves").type == JSONObject.Type.OBJECT)
            {
                this.View.DressUp(new Wear(field3.GetField("gloves")));
            }
            if (field3.GetField("shirt").type == JSONObject.Type.OBJECT)
            {
                this.View.DressUp(new Wear(field3.GetField("shirt")));
            }
            if (field3.GetField("pants").type == JSONObject.Type.OBJECT)
            {
                this.View.DressUp(new Wear(field3.GetField("pants")));
            }
            if (field3.GetField("boots").type == JSONObject.Type.OBJECT)
            {
                this.View.DressUp(new Wear(field3.GetField("boots")));
            }
            if (field3.GetField("backpack").type == JSONObject.Type.OBJECT)
            {
                this.View.DressUp(new Wear(field3.GetField("backpack")));
            }
            if (field3.GetField("other").type == JSONObject.Type.OBJECT)
            {
                this.View.DressUp(new Wear(field3.GetField("other")));
            }
        }
        else
        {
            base.user_id = LocalUser.UserID;
            base.name = LocalUser.Name;
            base.level = LocalUser.Level;
            this.exp = LocalUser.Exp;
            this.minExp = LocalUser.MinExp;
            this.maxExp = LocalUser.MaxExp;
            this.WeaponSlot = LocalUser.WeaponSlot;
            this.View = LocalUser.View;
        }
        JSONObject field5 = json.GetField("sA");
        if (field5.type == JSONObject.Type.OBJECT)
        {
            if (field5.GetField("wd").type == JSONObject.Type.STRING && field5.GetField("wd").str != string.Empty)
            {
                JSONObject jSONObject = new JSONObject(field5.GetField("wd").str);
                if (jSONObject.type == JSONObject.Type.ARRAY)
                {
                    for (int i = 0; i < jSONObject.Count; i++)
                    {
                        if (jSONObject[i].type != 0)
                        {
                            UserRatingWeapon item = new UserRatingWeapon(jSONObject[i]);
                            this.weaponStat.Add(item);
                        }
                    }
                    this.weaponStat.Sort((UserRatingWeapon x, UserRatingWeapon y) => y.Kill.CompareTo(x.Kill));
                }
            }
            if (field5.GetField("ud").type == JSONObject.Type.STRING && field5.GetField("ud").str != string.Empty)
            {
                JSONObject jSONObject2 = new JSONObject(field5.GetField("ud").str);
                if (jSONObject2.GetField("k") != null)
                {
                    this.kill = Convert.ToInt64(jSONObject2.GetField("k").n);
                }
                if (jSONObject2.GetField("d") != null)
                {
                    this.death = Convert.ToInt64(jSONObject2.GetField("d").n);
                }
                if (jSONObject2.GetField("s") != null)
                {
                    this.suicide = Convert.ToInt32(jSONObject2.GetField("s").n);
                }
                if (jSONObject2.GetField("hs") != null)
                {
                    this.headShot = Convert.ToInt64(jSONObject2.GetField("hs").n);
                }
                if (jSONObject2.GetField("ns") != null)
                {
                    this.nutsShot = Convert.ToInt64(jSONObject2.GetField("ns").n);
                }
                if (jSONObject2.GetField("pt") != null)
                {
                    this.playedTime = Convert.ToInt64(jSONObject2.GetField("pt").n);
                }
                if (jSONObject2.GetField("w") != null)
                {
                    this.win = Convert.ToUInt32(jSONObject2.GetField("w").n);
                }
                if (jSONObject2.GetField("l") != null)
                {
                    this.lose = Convert.ToUInt32(jSONObject2.GetField("l").n);
                }
                if (jSONObject2.GetField("dhs") != null)
                {
                    this.deathHeadShot = Convert.ToInt64(jSONObject2.GetField("dhs").n);
                }
                if (jSONObject2.GetField("dns") != null)
                {
                    this.deathNutsShot = Convert.ToInt64(jSONObject2.GetField("dns").n);
                }
                if (jSONObject2.GetField("do") != null)
                {
                    this.domination = Convert.ToInt64(jSONObject2.GetField("do").n);
                }
                if (jSONObject2.GetField("re") != null)
                {
                    this.revenge = Convert.ToInt64(jSONObject2.GetField("re").n);
                }
                if (jSONObject2.GetField("mdo") != null)
                {
                    this.maxDomination = Convert.ToInt32(jSONObject2.GetField("mdo").n);
                }
                if (jSONObject2.GetField("mre") != null)
                {
                    this.maxRevenge = Convert.ToInt32(jSONObject2.GetField("mre").n);
                }
                if (jSONObject2.GetField("sh") != null)
                {
                    this.shot = Convert.ToInt64(jSONObject2.GetField("sh").n);
                }
                if (jSONObject2.GetField("hi") != null)
                {
                    this.hit = Convert.ToInt64(jSONObject2.GetField("hi").n);
                }
                if (this.hit != 0L)
                {
                    this.accuracy = Convert.ToSingle(Convert.ToDouble(this.hit) / (double)this.shot * 100.0);
                }
                if (this.playedTime > 60)
                {
                    short num = Convert.ToInt16(this.playedTime % 60);
                    this.playedTimeStr = LanguageManager.GetTextFormat("{0} h {1} min", (this.playedTime - num) / 60, num);
                }
                else
                {
                    this.playedTimeStr = LanguageManager.GetTextFormat("{0} min.", this.playedTime);
                }
            }
            if (field5.GetField("md").type == JSONObject.Type.STRING && field5.GetField("md").str != string.Empty)
            {
                JSONObject jSONObject3 = new JSONObject(field5.GetField("md").str);
                if (jSONObject3.type == JSONObject.Type.ARRAY)
                {
                    for (int j = 0; j < jSONObject3.Count; j++)
                    {
                        if (jSONObject3[j].type != 0)
                        {
                            UserRatingGameMode item2 = new UserRatingGameMode(jSONObject3[j]);
                            this.gameModeStat.Add(item2);
                        }
                    }
                    this.gameModeStat.Sort((UserRatingGameMode x, UserRatingGameMode y) => y.PlayedTime.CompareTo(x.PlayedTime));
                }
            }
            if (field5.GetField("mad").type == JSONObject.Type.STRING && field5.GetField("mad").str != string.Empty)
            {
                JSONObject jSONObject4 = new JSONObject(field5.GetField("mad").str);
                if (jSONObject4.type == JSONObject.Type.ARRAY)
                {
                    for (int k = 0; k < jSONObject4.Count; k++)
                    {
                        if (jSONObject4[k].type != 0)
                        {
                            UserRatingMap item3 = new UserRatingMap(jSONObject4[k]);
                            this.mapStat.Add(item3);
                        }
                    }
                    this.mapStat.Sort((UserRatingMap x, UserRatingMap y) => y.PlayedTime.CompareTo(x.PlayedTime));
                }
            }
        }
        if (json.GetField("vk") != null && json.GetField("vk").str != string.Empty)
        {
            ulong num2 = Convert.ToUInt64(json.GetField("vk").str);
            if (num2 != 0L)
            {
                this.socialUrl = "https://vk.com/id" + num2.ToString();
            }
        }
        List<UserRatingWeapon>.Enumerator enumerator = this.weaponStat.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UserRatingWeapon current = enumerator.Current;
                if (this.favoriteWeapon == null || this.favoriteWeapon.Kill < current.Kill)
                {
                    this.favoriteWeapon = current;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
        List<UserRatingGameMode>.Enumerator enumerator2 = this.gameModeStat.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                UserRatingGameMode current2 = enumerator2.Current;
                if (this.favoriteMode == null || this.favoriteMode.PlayedTime < current2.PlayedTime)
                {
                    this.favoriteMode = current2;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator2).Dispose();
        }
        List<UserRatingMap>.Enumerator enumerator3 = this.mapStat.GetEnumerator();
        try
        {
            while (enumerator3.MoveNext())
            {
                UserRatingMap current3 = enumerator3.Current;
                if (this.favoriteMap == null || this.favoriteMap.PlayedTime < current3.PlayedTime)
                {
                    this.favoriteMap = current3;
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator3).Dispose();
        }
        this.kd = ((this.death >= 1) ? (Convert.ToSingle(this.kill) / (float)this.death) : ((float)this.kill));
        this.isLoaded = true;
    }

    public void AddWeaponStat(ArrayList data)
    {
        foreach (object datum in data)
        {
            Dictionary<string, object> dictionary = (Dictionary<string, object>)datum;
            int weapon_id = Convert.ToInt32(dictionary["wid"]);
            UserRatingWeapon userRatingWeapon = this.weaponStat.Find((UserRatingWeapon x) => x.Weapon.WeaponID == weapon_id);
            if (userRatingWeapon != null)
            {
                userRatingWeapon.AddFromDictionary(dictionary);
            }
            else
            {
                UserRatingWeapon item = new UserRatingWeapon(dictionary);
                this.weaponStat.Add(item);
            }
        }
        this.weaponStat.Sort((UserRatingWeapon x, UserRatingWeapon y) => y.Kill.CompareTo(x.Kill));
    }

    public void AddPlayerStat(Dictionary<string, object> data)
    {
        this.kill += Convert.ToInt64((!data.ContainsKey("k")) ? ((object)0) : data["k"]);
        this.death += Convert.ToInt64((!data.ContainsKey("d")) ? ((object)0) : data["d"]);
        this.suicide += Convert.ToInt32((!data.ContainsKey("s")) ? ((object)0) : data["s"]);
        this.headShot += Convert.ToInt64((!data.ContainsKey("hs")) ? ((object)0) : data["hs"]);
        this.nutsShot += Convert.ToInt64((!data.ContainsKey("ns")) ? ((object)0) : data["ns"]);
        this.playedTime += Convert.ToInt64((!data.ContainsKey("pt")) ? ((object)0) : data["pt"]);
        this.win += Convert.ToUInt32((!data.ContainsKey("w")) ? ((object)0) : data["w"]);
        this.lose += Convert.ToUInt32((!data.ContainsKey("l")) ? ((object)0) : data["l"]);
        this.deathHeadShot += Convert.ToInt64((!data.ContainsKey("dhs")) ? ((object)0) : data["dhs"]);
        this.deathNutsShot += Convert.ToInt64((!data.ContainsKey("dns")) ? ((object)0) : data["dns"]);
        this.domination += Convert.ToInt64((!data.ContainsKey("do")) ? ((object)0) : data["do"]);
        this.revenge += Convert.ToInt64((!data.ContainsKey("re")) ? ((object)0) : data["re"]);
        int num = Convert.ToInt32((!data.ContainsKey("mdo")) ? ((object)0) : data["mdo"]);
        if (num > this.maxDomination)
        {
            this.maxDomination = num;
        }
        int num2 = Convert.ToInt32((!data.ContainsKey("mre")) ? ((object)0) : data["mre"]);
        if (num2 > this.maxRevenge)
        {
            this.maxRevenge = num2;
        }
        this.shot += Convert.ToInt64((!data.ContainsKey("sh")) ? ((object)0) : data["sh"]);
        this.hit += Convert.ToInt64((!data.ContainsKey("hi")) ? ((object)0) : data["hi"]);
        if (this.playedTime > 60)
        {
            short num3 = Convert.ToInt16(this.playedTime % 60);
            this.playedTimeStr = LanguageManager.GetTextFormat("{0} h {1} min", (this.playedTime - num3) / 60, num3);
        }
        else
        {
            this.playedTimeStr = LanguageManager.GetTextFormat("{0} min.", this.playedTime);
        }
    }

    public void AddGameModeStat(ArrayList data)
    {
        foreach (object datum in data)
        {
            Dictionary<string, object> dictionary = datum as Dictionary<string, object>;
            MapMode.MODE mode = (MapMode.MODE)Convert.ToInt32(dictionary["m"]);
            UserRatingGameMode userRatingGameMode = this.gameModeStat.Find((UserRatingGameMode x) => x.Mode == mode);
            if (userRatingGameMode != null)
            {
                UnityEngine.Debug.LogError("Exist game mode: " + mode);
                userRatingGameMode.AddFromDictionary(dictionary);
            }
            else
            {
                UnityEngine.Debug.LogError("New game mode: " + mode);
                UserRatingGameMode item = new UserRatingGameMode(dictionary);
                this.gameModeStat.Add(item);
            }
        }
        this.gameModeStat.Sort((UserRatingGameMode x, UserRatingGameMode y) => y.PlayedTime.CompareTo(x.PlayedTime));
    }

    public void AddMapStat(ArrayList data)
    {
        foreach (object datum in data)
        {
            Dictionary<string, object> dictionary = datum as Dictionary<string, object>;
            string name = dictionary["n"].ToString();
            UserRatingMap userRatingMap = this.mapStat.Find((UserRatingMap x) => x.Map.SystemName == name);
            if (userRatingMap != null)
            {
                userRatingMap.AddFromDictionary(dictionary);
            }
            else
            {
                UserRatingMap item = new UserRatingMap(dictionary);
                this.mapStat.Add(item);
            }
        }
        this.mapStat.Sort((UserRatingMap x, UserRatingMap y) => y.PlayedTime.CompareTo(x.PlayedTime));
    }
}


