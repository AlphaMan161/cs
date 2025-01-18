// ILSpyBased#2
using System;
using System.Collections.Generic;

public class UserRatingWeapon
{
    private Weapon weapon;

    private long kill;

    private int headShot;

    private int nutsShot;

    private long shot;

    private long hit;

    private float accuracy;

    public Weapon Weapon
    {
        get
        {
            return this.weapon;
        }
    }

    public long Kill
    {
        get
        {
            return this.kill;
        }
    }

    public int HeadShot
    {
        get
        {
            return this.headShot;
        }
    }

    public int NutsShot
    {
        get
        {
            return this.nutsShot;
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

    public UserRatingWeapon(JSONObject data)
    {
        if (data.type != JSONObject.Type.OBJECT)
        {
            throw new Exception("UserRatingWeapon::UserRatingWeapon unkown data format");
        }
        this.weapon = new Weapon(Convert.ToInt32(data.GetField("wid").n), (WeaponType)(byte)Convert.ToInt32(data.GetField("wt").n), data.GetField("sn").str);
        this.kill = Convert.ToInt64(data.GetField("k").n);
        this.headShot = Convert.ToInt32(data.GetField("hs").n);
        this.nutsShot = Convert.ToInt32(data.GetField("ns").n);
        this.shot = Convert.ToInt64(data.GetField("sh").n);
        this.hit = Convert.ToInt64(data.GetField("hi").n);
        if (this.hit != 0L)
        {
            this.accuracy = Convert.ToSingle(Convert.ToDouble(this.hit) / (double)this.shot * 100.0);
        }
    }

    public UserRatingWeapon(Dictionary<string, object> data)
    {
        int weapon_id = Convert.ToInt32((!data.ContainsKey("wid")) ? ((object)0) : data["wid"]);
        int num = Convert.ToInt16((!data.ContainsKey("wt")) ? ((object)0) : data["wt"]);
        string systemName = (!data.ContainsKey("sn")) ? string.Empty : data["sn"].ToString();
        this.weapon = new Weapon(weapon_id, (WeaponType)(byte)num, systemName);
        this.kill = Convert.ToInt64((!data.ContainsKey("k")) ? ((object)0) : data["k"]);
        this.headShot = Convert.ToInt32((!data.ContainsKey("hs")) ? ((object)0) : data["hs"]);
        this.nutsShot = Convert.ToInt32((!data.ContainsKey("ns")) ? ((object)0) : data["ns"]);
        this.shot = Convert.ToInt32((!data.ContainsKey("sh")) ? ((object)0) : data["sh"]);
        this.hit = Convert.ToInt32((!data.ContainsKey("hi")) ? ((object)0) : data["hi"]);
        if (this.hit != 0L)
        {
            this.accuracy = Convert.ToSingle(Convert.ToDouble(this.hit) / (double)this.shot * 100.0);
        }
    }

    public void AddFromDictionary(Dictionary<string, object> data)
    {
        this.kill += Convert.ToInt64((!data.ContainsKey("k")) ? ((object)0) : data["k"]);
        this.headShot += Convert.ToInt32((!data.ContainsKey("hs")) ? ((object)0) : data["hs"]);
        this.nutsShot += Convert.ToInt32((!data.ContainsKey("ns")) ? ((object)0) : data["ns"]);
        this.shot += Convert.ToInt32((!data.ContainsKey("sh")) ? ((object)0) : data["sh"]);
        this.hit += Convert.ToInt32((!data.ContainsKey("hi")) ? ((object)0) : data["hi"]);
    }
}


