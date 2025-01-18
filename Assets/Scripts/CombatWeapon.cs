// dnSpy decompiler from Assembly-CSharp.dll class: CombatWeapon
using System;
using System.Collections;
using UnityEngine;

public class CombatWeapon
{
	public CombatWeapon(int index, int weaponID, WeaponType type, string systemName)
	{
		this.weaponID = weaponID;
		this.systemName = systemName;
		this.Index = index;
		this.type = type;
	}

	public CombatWeapon(int index, WeaponType type, int shotTime, int reloadTime, int maxAmmo, int velocity, int life)
	{
		this.Index = index;
		this.type = type;
		this.maxLoadedAmmo = maxAmmo;
		this.Life = life;
		this.velocity = velocity;
		this.shotTime = shotTime + 10;
		if (this.shotTime < 100)
		{
			this.shotTime = 100;
		}
		this.shotTime *= 10000;
		this.reloadTime = reloadTime + 10;
		if (this.reloadTime < 100)
		{
			this.reloadTime = 100;
		}
		if ((this.type == WeaponType.SHOT_GUN || this.type == WeaponType.ROCKET_LAUNCHER || this.type == WeaponType.GRENADE_LAUNCHER || this.type == WeaponType.BOMB_LAUNCHER) && this.maxLoadedAmmo >= 3)
		{
			this.reloadSingleTime = this.reloadTime / this.maxLoadedAmmo + 10;
			this.complexReload = true;
		}
		else
		{
			this.reloadSingleTime = this.reloadTime;
			this.complexReload = false;
		}
		this.reloadTime *= 10000;
		this.reloadSingleTime = this.reloadTime * 10000;
	}

	public CombatWeapon(int index, Hashtable weaponData)
	{
		this.Index = index;
		this.weaponID = (int)weaponData[(byte)80];
		this.type = (WeaponType)((int)weaponData[(byte)98]);
		this.maxLoadedAmmo = (int)weaponData[(byte)92];
		this.velocity = (int)weaponData[(byte)97];
		this.Life = (int)weaponData[(byte)90];
		this.loadedAmmo = this.maxLoadedAmmo;
        this.maxAmmoReserve = (int)weaponData[(byte)91];
        this.ammoReserve = this.maxAmmoReserve - this.loadedAmmo;
		this.distance = (float)((int)weaponData[(byte)96]);
		this.angle = (float)weaponData[(byte)95];
		this.systemName = (string)weaponData[(byte)99];
		this.deviation = (float)((int)weaponData[(byte)87]);
		if (this.systemName == "GG_M249" || this.systemName == "GG_FNMAG")
		{
			this.launchTime = 4000000;
		}
		this.shotTime = (int)weaponData[(byte)94] + 10;
		if (this.shotTime < 100)
		{
			this.shotTime = 110;
		}
		this.shotTime *= 10000;
		this.reloadTime = (int)weaponData[(byte)93] + 10;
		if (this.reloadTime < 100)
		{
			this.reloadTime = 110;
		}
		if ((this.type == WeaponType.SHOT_GUN || this.type == WeaponType.ROCKET_LAUNCHER || this.type == WeaponType.GRENADE_LAUNCHER || this.type == WeaponType.BOMB_LAUNCHER) && this.maxLoadedAmmo >= 3)
		{
			this.reloadSingleTime = this.reloadTime / this.maxLoadedAmmo + 10;
			this.complexReload = true;
		}
		else
		{
			this.reloadSingleTime = this.reloadTime;
			this.complexReload = false;
		}
		this.reloadTime *= 10000;
		this.reloadSingleTime *= 10000;
		if (weaponData.ContainsKey((byte)79))
		{
			Hashtable hashtable = (Hashtable)weaponData[(byte)79];
			if (hashtable.ContainsKey((byte)78))
			{
				this.speedValue = 1f + (float)((int)hashtable[(byte)78]) / 100f;
			}
			if (hashtable.ContainsKey((byte)75))
			{
				this.canLaunch = true;
			}
			if (hashtable.ContainsKey((byte)76))
			{
				this.isShaking = true;
			}
		}
	}

	public WeaponMode WeaponMode
	{
		get
		{
			return this.weaponMode;
		}
	}

	public Transform Target
	{
		get
		{
			return this.target;
		}
		set
		{
			this.target = value;
		}
	}

	public Transform Transform
	{
		get
		{
			return this.transform;
		}
		set
		{
			this.transform = value;
		}
	}

	public int WeaponID
	{
		get
		{
			return this.weaponID;
		}
	}

	public WeaponType Type
	{
		get
		{
			return this.type;
		}
	}

	public bool IsShaking
	{
		get
		{
			return this.isShaking;
		}
	}

	public string Attributes
	{
		get
		{
			if (this.attributes == "_")
			{
				if (this.SystemName.Split(new char[]
				{
					'_'
				}).Length > 2)
				{
					this.attributes = this.SystemName.Split(new char[]
					{
						'_'
					})[2];
				}
				else
				{
					this.attributes = string.Empty;
				}
			}
			return this.attributes;
		}
	}

	public bool CanLaunch
	{
		get
		{
			return this.canLaunch;
		}
	}

	public int EnhacerModes
	{
		get
		{
			return this.enhancerModes;
		}
	}

	public void SetEnhancerMode(EnhancerMode mode)
	{
		this.enhancerModes |= (int)mode;
	}

	public void UnsetEnhancerMode(EnhancerMode mode)
	{
		this.enhancerModes ^= (int)mode;
	}

	public int ShotTime
	{
		get
		{
			return this.shotTime;
		}
		set
		{
			this.shotTime = value;
		}
	}

	public int ReloadSingleTime
	{
		get
		{
			return this.reloadSingleTime;
		}
		set
		{
			this.reloadSingleTime = value;
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

	public int MaxAmmoReserve
	{
		get
		{
			return this.maxAmmoReserve;
		}
		set
		{
			this.maxAmmoReserve = value;
		}
	}

	public int AmmoReserve
	{
		get
		{
			return this.ammoReserve;
		}
	}

	public int MaxLoadedAmmo
	{
		get
		{
			return this.maxLoadedAmmo;
		}
	}

	public int LoadedAmmo
	{
		get
		{
			return this.loadedAmmo;
		}
	}

	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = value;
		}
	}

	public int Velocity
	{
		get
		{
			return this.velocity;
		}
	}

	public float Distance
	{
		get
		{
			return this.distance;
		}
		set
		{
			this.distance = value;
		}
	}

	public float Angle
	{
		get
		{
			return this.angle;
		}
		set
		{
			this.angle = value;
		}
	}

	public float Deviation
	{
		get
		{
			return this.deviation;
		}
	}

	public string SystemName
	{
		get
		{
			return this.systemName;
		}
		set
		{
			this.systemName = value;
		}
	}

	public bool ComplexReload
	{
		get
		{
			return this.complexReload;
		}
	}

	public float SpeedValue
	{
		get
		{
			return this.speedValue;
		}
	}

	public void SwitchWeaponTexture(int textureID)
	{
		if (this.Type == WeaponType.TWO_HANDED_COLD_ARMS)
		{
			RuntimeTextureSwitch componentInChildren = this.transform.GetComponentInChildren<RuntimeTextureSwitch>();
			if (componentInChildren == null)
			{
				return;
			}
			componentInChildren.Switch(textureID);
		}
	}

	public void UpdateAmmoCount(int loadedAmmo, int ammoReserve)
	{
		this.loadedAmmo = loadedAmmo;
		this.ammoReserve = ammoReserve;
	}

	public void AddAmmoToReserve(int amount)
	{
		this.ammoReserve += amount;
		if (this.ammoReserve > this.maxAmmoReserve)
		{
			this.ammoReserve = this.maxAmmoReserve;
		}
	}

	public void AddAmmoToReserve(int amount, bool allWeapons)
	{
		this.ammoReserve += amount;
		if (this.ammoReserve > this.maxAmmoReserve - this.loadedAmmo)
		{
			this.ammoReserve = this.maxAmmoReserve - this.loadedAmmo;
		}
	}

	public void ShootAmmo()
	{
		this.loadedAmmo--;
		if (this.loadedAmmo < 0)
		{
			UnityEngine.Debug.Log("NOOO AMMOOO SHOOOT!!!");
		}
	}

	public int GetIndex()
	{
		return this.Index + 1;
	}

	public new int GetType()
	{
		return (int)this.type;
	}

	public string GetTypeString()
	{
		return this.Type.ToString();
	}

	public string GetName()
	{
		return CombatWeapon.getName(this.type);
	}

	public static string getName(WeaponType ind)
	{
		switch (ind)
		{
		case WeaponType.ONE_HANDED_COLD_ARMS:
			return "OneHandedColdArmsWeapon";
		case WeaponType.TWO_HANDED_COLD_ARMS:
			return "TwoHandedColdArmsWeapon";
		case WeaponType.HAND_GUN:
			return "HandGunWeapon";
		case WeaponType.MACHINE_GUN:
			return "MachineGunWeapon";
		case WeaponType.FLAMER:
		case WeaponType.SNOW_GUN:
		case WeaponType.ACID_THROWER:
			return "FlamerWeapon";
		case WeaponType.GATLING_GUN:
			return "GatlingGunWeapon";
		case WeaponType.SHOT_GUN:
			return "ShotGunWeapon";
		case WeaponType.ROCKET_LAUNCHER:
			return "RocketLauncherWeapon";
		case WeaponType.GRENADE_LAUNCHER:
			return "GrenadeLauncherWeapon";
		case WeaponType.SNIPER_RIFLE:
			return "SniperRifleWeapon";
		case WeaponType.BOMB_LAUNCHER:
			return "BombLauncherWeapon";
		}
		return "None";
	}

	public WeaponMode CalcMode()
	{
		switch (this.weaponMode)
		{
		case WeaponMode.Changing:
			if (DateTime.Now.Ticks - this.lastChangeTime >= (long)this.changeTime)
			{
				this.weaponMode = WeaponMode.Ready;
			}
			break;
		case WeaponMode.Shooting:
			if (DateTime.Now.Ticks - this.lastShotTime >= (long)this.shotTime)
			{
				if (this.type == WeaponType.GATLING_GUN || this.type == WeaponType.FLAMER || this.type == WeaponType.ACID_THROWER || this.type == WeaponType.SNOW_GUN)
				{
					this.weaponMode = WeaponMode.Launching;
				}
				else
				{
					this.weaponMode = WeaponMode.Ready;
				}
			}
			break;
		case WeaponMode.Reloading:
			if (DateTime.Now.Ticks - this.lastReloadTime >= (long)this.reloadTime)
			{
				this.weaponMode = WeaponMode.Ready;
			}
			else if (DateTime.Now.Ticks - this.lastReloadTime >= (long)this.reloadSingleTime)
			{
				this.weaponMode = WeaponMode.ReloadingReady;
			}
			break;
		case WeaponMode.ReloadingReady:
			if (DateTime.Now.Ticks - this.lastReloadTime >= (long)this.reloadTime)
			{
				this.weaponMode = WeaponMode.Ready;
			}
			break;
		case WeaponMode.Launching:
			if (DateTime.Now.Ticks - this.lastLaunchTime >= (long)this.launchTime)
			{
			}
			break;
		}
		return this.weaponMode;
	}

	public bool CanFire()
	{
		if (this.type == WeaponType.GATLING_GUN && DateTime.Now.Ticks - this.lastLaunchTime < (long)this.launchTime)
		{
			return false;
		}
		if (this.type == WeaponType.GATLING_GUN || this.type == WeaponType.FLAMER || this.type == WeaponType.ACID_THROWER || this.type == WeaponType.SNOW_GUN)
		{
			return this.weaponMode == WeaponMode.Launching && DateTime.Now.Ticks - this.lastShotTime >= (long)this.shotTime;
		}
		return ((this.type == WeaponType.SHOT_GUN || this.type == WeaponType.ROCKET_LAUNCHER || this.type == WeaponType.GRENADE_LAUNCHER || this.type == WeaponType.BOMB_LAUNCHER) && this.weaponMode == WeaponMode.ReloadingReady) || this.weaponMode == WeaponMode.Ready;
	}

	public bool Fire()
	{
		if (this.CanFire())
		{
			this.lastShotTime = DateTime.Now.Ticks;
			this.weaponMode = WeaponMode.Shooting;
			return true;
		}
		return false;
	}

	public bool Launch()
	{
		if (this.weaponMode == WeaponMode.Ready)
		{
			this.lastLaunchTime = DateTime.Now.Ticks;
			this.weaponMode = WeaponMode.Launching;
			return true;
		}
		return false;
	}

	public bool Stop()
	{
		if (this.weaponMode == WeaponMode.Launching || this.weaponMode == WeaponMode.Shooting)
		{
			this.weaponMode = WeaponMode.Ready;
			return true;
		}
		return false;
	}

	public bool IsWeaponBlockedChange()
	{
		return this.weaponMode == WeaponMode.Changing;
	}

	public bool IsWeaponBlocked()
	{
		return this.type != WeaponType.ONE_HANDED_COLD_ARMS && this.type != WeaponType.TWO_HANDED_COLD_ARMS && (this.type != WeaponType.SHOT_GUN || this.weaponMode != WeaponMode.ReloadingReady) && (this.weaponMode == WeaponMode.Reloading || this.loadedAmmo + this.ammoReserve == 0);
	}

	public bool CanReload()
	{
		return this.ammoReserve != 0 && this.loadedAmmo != this.maxLoadedAmmo && this.weaponMode != WeaponMode.Reloading;
	}

	public int Reload()
	{
		this.weaponMode = WeaponMode.Reloading;
		this.lastReloadSingleTime = DateTime.Now.Ticks; this.lastReloadTime = (this.lastReloadSingleTime );
		return Math.Min(this.maxLoadedAmmo - this.loadedAmmo, this.ammoReserve);
	}

	public int getReload()
	{
		if (this.weaponMode == WeaponMode.Reloading)
		{
			return 0;
		}
		return 9;
	}

	public void Change()
	{
		this.lastChangeTime = DateTime.Now.Ticks;
		this.weaponMode = WeaponMode.Changing;
	}

	private WeaponMode weaponMode = WeaponMode.Ready;

	protected Transform target;

	protected Transform transform;

	private int weaponID = -1;

	protected WeaponType type;

	private bool isShaking;

	private string attributes = "_";

	private bool canLaunch;

	private int enhancerModes;

	protected int launchTime = 15000000;

	protected int shotTime = 1000000;

	protected int changeTime = 6000000;

	protected int reloadSingleTime = 1500000;

	protected int reloadTime = 1500000;

	protected long lastChangeTime;

	protected long lastReloadTime;

	protected long lastReloadSingleTime;

	protected long lastShotTime;

	protected long lastLaunchTime;

	protected int ammoReserve;

	protected int maxAmmoReserve;

	protected int maxLoadedAmmo;

	protected int loadedAmmo;

	protected int index;

	private int velocity;

	public int Life;

	private float distance;

	private float angle;

	private float deviation;

	private string systemName = string.Empty;

	private bool complexReload;

	private float speedValue = 1f;
}
