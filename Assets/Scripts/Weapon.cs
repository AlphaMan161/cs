// dnSpy decompiler from Assembly-CSharp.dll class: Weapon
using System;
using System.Collections.Generic;

public class Weapon : CCItem
{
	public Weapon(int weapon_id, WeaponType weaponType, string systemName)
	{
		this.w_id = weapon_id;
		this.wt = weaponType;
		this.sname = systemName;
		this.name = "w_" + this.w_id + "_name";
		this.desc = "w_" + this.w_id + "_desc";
		this.descAdditional = "w_" + this.w_id + "_desca";
		this.LoadIco(this.isDefault);
	}

	public Weapon(JSONObject obj)
	{
		if (obj.type != JSONObject.Type.OBJECT)
		{
			throw new Exception("[Weapon] Input object not a valid data");
		}
		if (obj.GetField("w_id") != null)
		{
			this.w_id = Convert.ToInt32(obj.GetField("w_id").n);
		}
		else if (obj.GetField("id"))
		{
			if (obj.GetField("id").type == JSONObject.Type.NUMBER)
			{
				this.w_id = Convert.ToInt32(obj.GetField("id").n);
			}
			else
			{
				this.w_id = Convert.ToInt32(obj.GetField("id").str);
			}
		}
		this.itemID = Convert.ToUInt32(this.w_id);
		if (obj.GetField("wt") != null)
		{
			this.wt = (WeaponType)Convert.ToInt32(obj.GetField("wt").n);
		}
		if (obj.GetField("u_id") != null)
		{
			this.isUpgrade = true;
			this.upgradeID = Convert.ToInt32(obj.GetField("u_id").n);
			this.upgradeTxt = "w_" + this.w_id + "_descupgrade";
		}
		if (obj.GetField("ws") != null)
		{
			this.ws = Convert.ToInt32(obj.GetField("ws").n);
		}
		if (obj.GetField("vel") != null)
		{
			this.vel = Convert.ToUInt32(obj.GetField("vel").n);
		}
		if (obj.GetField("rad") != null)
		{
			this.rad = Convert.ToUInt32(obj.GetField("rad").n);
		}
		if (obj.GetField("ang") != null)
		{
			this.ang = Convert.ToUInt32(obj.GetField("ang").n);
		}
		if (obj.GetField("dev") != null)
		{
			this.dev = Convert.ToUInt32(obj.GetField("dev").n);
		}
		if (obj.GetField("rap") != null)
		{
			this.rap = Convert.ToUInt32(obj.GetField("rap").n);
		}
		if (obj.GetField("rt") != null)
		{
			this.rt = Convert.ToUInt32(obj.GetField("rt").n);
		}
		if (obj.GetField("lt") != null)
		{
			this.lt = Convert.ToUInt32(obj.GetField("lt").n);
		}
		if (obj.GetField("smindam") != null)
		{
			this.smindam = Convert.ToUInt32(obj.GetField("smindam").n);
		}
		if (obj.GetField("smaxdam") != null)
		{
			this.smaxdam = Convert.ToUInt32(obj.GetField("smaxdam").n);
		}
		if (obj.GetField("mmindam") != null)
		{
			this.mmindam = Convert.ToUInt32(obj.GetField("mmindam").n);
		}
		if (obj.GetField("mmaxdam") != null)
		{
			this.mmaxdam = Convert.ToUInt32(obj.GetField("mmaxdam").n);
		}
		if (obj.GetField("lmindam") != null)
		{
			this.lmindam = Convert.ToUInt32(obj.GetField("lmindam").n);
		}
		if (obj.GetField("smaxdam") != null)
		{
			this.lmaxdam = Convert.ToUInt32(obj.GetField("lmaxdam").n);
		}
		if (obj.GetField("krit") != null)
		{
			this.krit = Convert.ToUInt32(obj.GetField("krit").n);
		}
		if (obj.GetField("ammo") != null)
		{
			this.ammo = Convert.ToUInt32(obj.GetField("ammo").n);
		}
		if (obj.GetField("ammo_tot") != null)
		{
			this.ammo_tot = Convert.ToUInt32(obj.GetField("ammo_tot").n);
		}
		if (obj.GetField("name") != null)
		{
			this.name = obj.GetField("name").str;
		}
		if (obj.GetField("sname") != null)
		{
			this.sname = obj.GetField("sname").str;
		}
		if (obj.GetField("nlvl") != null)
		{
			this.nlvl = Convert.ToUInt32(obj.GetField("nlvl").n);
		}
		if (obj.GetField("stRa") != null)
		{
			this.starRapidity = Convert.ToInt16(obj.GetField("stRa").n);
		}
		if (obj.GetField("stDi") != null)
		{
			this.starDistance = Convert.ToInt16(obj.GetField("stDi").n);
		}
		if (obj.GetField("stDa") != null)
		{
			this.starDamage = Convert.ToInt16(obj.GetField("stDa").n);
		}
		if (obj.GetField("eD") != null)
		{
			long num = Convert.ToInt64(obj.GetField("eD").n);
			if (num != 0L)
			{
				this.duration = new Duration(num - (long)((ulong)Inventory.Instance.ServerTime));
			}
		}
		if (obj.GetField("iS") != null)
		{
			if (obj.GetField("iS").type == JSONObject.Type.NUMBER)
			{
				this.isSale = (Convert.ToUInt32(obj.GetField("iS").n) == 1u);
			}
			else
			{
				this.isSale = (Convert.ToUInt32(obj.GetField("iS").str) == 1u);
			}
		}
		this.name = "w_" + this.w_id + "_name";
		this.desc = "w_" + this.w_id + "_desc";
		this.descAdditional = "w_" + this.w_id + "_desca";
		this.LoadIco(this.isDefault);
		if (obj.GetField("sc") != null)
		{
			this.shop_cost = new ShopCost(obj.GetField("sc"));
		}
		this.assemblage = AssemblageManager.GetAssemblage(this);
		this.weekItem = WeekItemsManager.GetWeekItem(this);
		if (this.duration != null)
		{
			DurationManager.Add(this.duration, this);
		}
	}

	public int WeaponID
	{
		get
		{
			return this.w_id;
		}
	}

	public WeaponType WeaponType
	{
		get
		{
			return this.wt;
		}
	}

	public int WeaponSlot
	{
		get
		{
			return this.ws;
		}
	}

	private uint Velocity
	{
		get
		{
			return this.vel;
		}
	}

	public uint Radius
	{
		get
		{
			return this.rad;
		}
	}

	public uint Angel
	{
		get
		{
			return this.ang;
		}
	}

	public uint Deviation
	{
		get
		{
			return this.dev;
		}
	}

	public uint Rapidity
	{
		get
		{
			return this.rap;
		}
	}

	public uint ReloadTime
	{
		get
		{
			return this.rt;
		}
	}

	public uint LifeTime
	{
		get
		{
			return this.lt;
		}
	}

	public uint ShortMinDamage
	{
		get
		{
			return this.smindam;
		}
	}

	public uint ShortMaxDamage
	{
		get
		{
			return this.smaxdam;
		}
	}

	public uint MediumMinDam
	{
		get
		{
			return this.mmindam;
		}
	}

	public uint MediumMaxDam
	{
		get
		{
			return this.mmaxdam;
		}
	}

	private uint LongMinDamage
	{
		get
		{
			return this.lmindam;
		}
	}

	public uint LongMaxDamage
	{
		get
		{
			return this.lmaxdam;
		}
	}

	public uint Krit
	{
		get
		{
			return this.krit;
		}
	}

	public uint Ammo
	{
		get
		{
			return this.ammo;
		}
	}

	public uint AmmoTotal
	{
		get
		{
			return this.ammo_tot;
		}
	}

	public bool ShowOnCamera
	{
		get
		{
			return this.showOnCamera;
		}
		set
		{
			this.showOnCamera = value;
		}
	}

	public bool IsDefault
	{
		get
		{
			return this.isDefault;
		}
		set
		{
			if (this.isDefault != value)
			{
				this.isDefault = value;
				this.LoadIco(this.isDefault);
			}
		}
	}

	public short StarRapidity
	{
		get
		{
			return this.starRapidity;
		}
	}

	public short StarDistance
	{
		get
		{
			return this.starDistance;
		}
	}

	public short StarDamage
	{
		get
		{
			return this.starDamage;
		}
	}

	public bool IsUpgrade
	{
		get
		{
			return this.isUpgrade;
		}
	}

	public Assemblage AssemblageRel
	{
		get
		{
			return this.assemblage;
		}
	}

	public WeekItem WeekItemRel
	{
		get
		{
			return this.weekItem;
		}
	}

	public Weapon Upgrade
	{
		get
		{
			return this.upgrade;
		}
		set
		{
			this.upgrade = value;
		}
	}

	public int UpgradeID
	{
		get
		{
			return this.upgradeID;
		}
	}

	public Duration Duration
	{
		get
		{
			return this.duration;
		}
		set
		{
			this.duration = value;
		}
	}

	public string UpgradeTxt
	{
		get
		{
			return LanguageManager.GetText(this.upgradeTxt);
		}
	}

	public static List<Weapon> WeaponFromList(JSONObject list)
	{
		if (list.type != JSONObject.Type.ARRAY)
		{
			throw new Exception("[Weapon] WeaponFromList: Input object not a array");
		}
		List<Weapon> list2 = new List<Weapon>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(new Weapon(list[i]));
		}
		return list2;
	}

	private void LoadIco(bool isDefault)
	{
		if (!isDefault)
		{
			this.icoFileString = string.Format("GUI/Icons/Items/Weapon/{0}", base.SystemName);
		}
		else
		{
			this.icoFileString = string.Format("GUI/Icons/Items/Weapon/Default/{0}", base.SystemName);
		}
	}

	public bool Equals(Weapon p)
	{
		return p != null && this.w_id == p.w_id;
	}

	public void Replace(Weapon source)
	{
		this.w_id = source.w_id;
		this.itemID = source.itemID;
		this.wt = source.wt;
		this.isUpgrade = source.isUpgrade;
		this.upgradeID = source.upgradeID;
		this.ws = source.ws;
		this.vel = source.vel;
		this.rad = source.rad;
		this.ang = source.ang;
		this.dev = source.dev;
		this.rap = source.rap;
		this.rt = source.rt;
		this.lt = source.lt;
		this.smindam = source.smindam;
		this.smaxdam = source.smaxdam;
		this.mmindam = source.mmindam;
		this.mmaxdam = source.mmaxdam;
		this.lmindam = source.lmindam;
		this.lmaxdam = source.lmaxdam;
		this.krit = source.krit;
		this.ammo = source.ammo;
		this.ammo_tot = source.ammo_tot;
		this.sname = source.sname;
		this.nlvl = source.nlvl;
		this.starRapidity = source.starRapidity;
		this.starDistance = source.starDistance;
		this.starDamage = source.starDamage;
		this.duration = source.duration;
		this.name = source.name;
		this.desc = source.desc;
		this.descAdditional = source.descAdditional;
		this.icoFileString = source.icoFileString;
		this.shop_cost = source.shop_cost;
		this.assemblage = source.assemblage;
		this.duration = source.duration;
		this.isUpgrade = source.isUpgrade;
		this.upgradeTxt = source.upgradeTxt;
		this.isSale = source.isSale;
	}

	private int w_id;

	private WeaponType wt;

	private int ws;

	private uint vel;

	private uint rad;

	private uint ang;

	private uint dev;

	private uint rap;

	private uint rt;

	private uint lt;

	private uint smindam;

	private uint smaxdam;

	private uint mmindam;

	private uint mmaxdam;

	private uint lmindam;

	private uint lmaxdam;

	private uint krit;

	private uint ammo;

	private uint ammo_tot;

	private bool showOnCamera = true;

	private bool isDefault;

	private short starRapidity;

	private short starDistance;

	private short starDamage;

	private bool isUpgrade;

	private Assemblage assemblage;

	private WeekItem weekItem;

	private Weapon upgrade;

	private int upgradeID;

	private Duration duration;

	private string upgradeTxt = string.Empty;
}
