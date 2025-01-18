// dnSpy decompiler from Assembly-CSharp.dll class: LocalShotController
using System;
using System.Collections;
using UnityEngine;

public class LocalShotController : MonoBehaviour
{
	public static LocalShotController Instance
	{
		get
		{
			return LocalShotController.instance;
		}
	}

	public CombatWeapon CurrentWeapon
	{
		get
		{
			return this.weapon;
		}
	}

	public CombatWeapon[] Weapons
	{
		get
		{
			return this.weapons;
		}
	}

	public CombatEnhancer[] Enhancers
	{
		get
		{
			return this.enhancers;
		}
	}

	public string GetAmmoCountString()
	{
		return this.loadedAmmo + "/" + this.ammoReserve;
	}

	public string GetWeaponString()
	{
		return this.weapon.GetTypeString();
	}

	public void ResetWeapon()
	{
		this.weapon = this.weapons[0];
	}

	private CombatPlayer LocalPlayer
	{
		get
		{
			if (this.localPlayer == null)
			{
				this.localPlayer = base.transform.GetComponent<CombatPlayer>();
			}
			return this.localPlayer;
		}
	}

	private EffectManager EffectManager
	{
		get
		{
			if (this.effectManager == null)
			{
				this.effectManager = EffectManager.Instance;
			}
			return this.effectManager;
		}
	}

	private NetworkManager NetworkManager
	{
		get
		{
			if (this.networkManager == null)
			{
				this.networkManager = NetworkManager.Instance;
			}
			return this.networkManager;
		}
	}

	public void SetWeapon(int weaponNum)
	{
		if (this.weapons[weaponNum - 1] != null && this.weapons[weaponNum - 1] != this.weapon)
		{
			this.previousWeapon = this.weapon.GetIndex();
			this.weapon = this.weapons[weaponNum - 1];
			this.Change(weaponNum);
			this.loadedAmmo = this.weapon.LoadedAmmo;
			this.ammoReserve = this.weapon.AmmoReserve;
			if (this.loadedAmmo == 0 && this.ammoReserve > 0)
			{
				this.Reload();
			}
		}
		this.SetCrossHair();
	}

	private void SetCrossHair()
	{
		if (base.transform.parent)
		{
			Transform transform = LocalGameHUD.Instance.CrossHairs.FindChild("CrossHair");
			if (transform == null)
			{
				return;
			}
			transform.GetComponent<Renderer>().enabled = true;
			transform.GetComponent<Renderer>().material.SetTexture("_MainTex", (Texture)Resources.Load("Ammo/Crosshair" + this.weapon.GetType()));
		}
	}

	public void UseEnhancer(int enhancerNum)
	{
		if (this.enhancers[enhancerNum - 1] != null && this.enhancers[enhancerNum - 1].Fire())
		{
			this.NetworkManager.SendEnhancer(enhancerNum);
			UnityEngine.Debug.Log("Use enhancer " + enhancerNum);
		}
	}

	public int InitWeapons(bool giveAmmo)
	{
		this.weapons[0] = new CombatWeapon(0, WeaponType.MACHINE_GUN, 120, 2000, 100, 0, 0);
		this.weapons[1] = new CombatWeapon(1, WeaponType.ROCKET_LAUNCHER, 200, 2000, 1, 20, 0);
		this.weapons[2] = new CombatWeapon(2, WeaponType.HAND_GUN, 200, 2000, 1, 0, 0);
		this.weapons[3] = new CombatWeapon(3, WeaponType.FLAMER, 150, 2000, 500, 0, 0);
		this.weapons[4] = new CombatWeapon(4, WeaponType.SHOT_GUN, 200, 2000, 50, 0, 0);
		this.weapons[0].MaxAmmoReserve = 5000;
		this.weapons[1].MaxAmmoReserve = 10;
		this.weapons[2].MaxAmmoReserve = 10;
		this.weapons[3].MaxAmmoReserve = 5000;
		this.weapons[4].MaxAmmoReserve = 100;
		if (giveAmmo)
		{
			this.weapons[0].UpdateAmmoCount(500, 5000);
			this.weapons[1].UpdateAmmoCount(1, 10);
			this.weapons[2].UpdateAmmoCount(1, 10);
			this.weapons[3].UpdateAmmoCount(500, 5000);
			this.weapons[4].UpdateAmmoCount(50, 100);
		}
		int num = 0;
		if (num == 0)
		{
			this.weapon = this.weapons[num];
			this.loadedAmmo = this.weapon.LoadedAmmo;
			this.ammoReserve = this.weapon.AmmoReserve;
			this.SetCrossHair();
			WeaponLook[] componentsInChildren = base.transform.GetComponentsInChildren<WeaponLook>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Active = false;
				if (componentsInChildren[i].transform.name == this.weapon.GetName() || componentsInChildren[i].transform.name == this.weapon.GetName() + "L" || componentsInChildren[i].transform.name == this.weapon.GetName() + "R")
				{
					componentsInChildren[i].Active = true;
				}
			}
		}
		this.shot = false;
		return this.weapons[num].GetType();
	}

	public int InitWeapon(int index, Hashtable weaponData)
	{
		this.weapons[index] = new CombatWeapon(index, weaponData);
		if (index == 0)
		{
			this.weapon = this.weapons[index];
			this.loadedAmmo = this.weapon.LoadedAmmo;
			this.ammoReserve = this.weapon.AmmoReserve;
			this.SetCrossHair();
			WeaponLook[] componentsInChildren = base.transform.GetComponentsInChildren<WeaponLook>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Active = false;
				if (componentsInChildren[i].transform.name == this.weapon.GetName() || componentsInChildren[i].transform.name == this.weapon.GetName() + "L" || componentsInChildren[i].transform.name == this.weapon.GetName() + "R")
				{
					componentsInChildren[i].Active = true;
				}
			}
		}
		this.shot = false;
        return this.weapons[index].GetType();
	}

	public void InitEnhancer(int index, Hashtable enhancerData)
	{
		if (this.enhancers[index] == null)
		{
			this.enhancers[index] = new CombatEnhancer(index, enhancerData);
		}
	}

	private void Awake()
	{
		LocalShotController.instance = this;
		this.weapons = new CombatWeapon[10];
		this.enhancers = new CombatEnhancer[2];
	}

	private void Update()
	{
		if (!LocalGameHUD.Instance.isActive())
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Fire1))
		{
			this.shot = true;
		}
		if (UnityEngine.Input.GetKeyUp(TRInput.Fire1))
		{
			this.shot = false;
		}
		if (this.loadedAmmo > 0 && this.shot)
		{
			if (this.weapon.Fire())
			{
				this.DoShot();
			}
		}
		else if (UnityEngine.Input.GetKeyDown(TRInput.Reload))
		{
			UnityEngine.Debug.Log("Force Reload");
			this.Reload();
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.QuickChange))
		{
			this.QuickChange();
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon1))
		{
			this.SetWeapon(1);
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon2))
		{
			this.SetWeapon(2);
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon3))
		{
			this.SetWeapon(3);
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon4))
		{
			this.SetWeapon(4);
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon5))
		{
			this.SetWeapon(5);
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Enhancer1))
		{
			this.UseEnhancer(1);
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Enhancer2))
		{
			this.UseEnhancer(2);
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Zoom))
		{
			this.DoShot2x();
		}
		this.CheckRaycastWithEnemy();
	}

	private void DoShot()
	{
		System.Random random = new System.Random();
		Transform transform = base.transform.FindChild("SShip").FindChild(this.weapon.GetName());
		if (transform != null)
		{
			transform = transform.FindChild("Target");
		}
		else
		{
			transform = base.transform;
		}
		float num = 0f;
		WalkController component = base.transform.GetComponent<WalkController>();
		if (component)
		{
			num = component.Speed;
		}
		float num2 = 0f;
		if (this.weapon.GetType() == 1 || this.weapon.GetType() == 7 || this.weapon.GetType() == 10)
		{
			num2 = 0.004f * (num + 10f);
		}
		float num3 = Convert.ToSingle(random.NextDouble() * (double)num2) - num2 / 2f;
		float num4 = Convert.ToSingle(random.NextDouble() * (double)num2) - num2 / 2f;
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f + num3, 0.5f + num4, 20f));
		switch ((byte)this.weapon.GetType())
		{
		case 3:
		case 4:
		case 6:
		case 7:
                {
                    Shot shot = ShotCalculator.StraightShot(ray, transform, this.weapon.GetType(), 0, false);
                    foreach (ShotTarget shotTarget in shot.Targets)
                    {
                        shotTarget.EnergyDamage = 10;
                    }
                    LocalPlayerManager.Instance.ShotPlayer(1, shot);
                    LocalPlayerManager.Instance.hitTurret(shot, this.LocalPlayer, true, 20);
                    this.EffectManager.machineGunEffect(shot, this.LocalPlayer, true);
                    break;
                }
		case 5:
		{
			Shot shot = ShotCalculator.StraightShot(ray, transform, this.weapon.GetType(), 0, false);
			LocalPlayerManager.Instance.flamerEffect(shot, this.LocalPlayer, true);
			LocalPlayerManager.Instance.ShotPlayer(1, shot);
			this.EffectManager.flamerEffect(shot, this.LocalPlayer, true);
			break;
		}
		case 8:
		{
			Shot shot = ShotCalculator.RocketShot(ray, transform, this.weapon.GetType(), (float)this.weapon.Velocity);
			LocalPlayerManager.Instance.ShotPlayer(1, shot);
			this.EffectManager.rocketLauncherEffect(shot, this.LocalPlayer, true);
			break;
		}
		case 9:
		{
			Shot shot = ShotCalculator.GrenadeShot(ray, transform, this.weapon.GetType(), (float)this.weapon.Velocity, (float)this.weapon.Life, false);
			this.EffectManager.grenadeLauncherEffect(shot, this.LocalPlayer, true);
			break;
		}
		case 10:
		{
			Camera componentInChildren = base.GetComponentInChildren<Camera>();
			float num5 = componentInChildren.fov / 55f;
			Shot shot;
			if ((double)num5 > 0.5)
			{
				shot = ShotCalculator.StraightShot(ray, transform, this.weapon.GetType(), 0, false);
			}
			else
			{
				shot = ShotCalculator.RailShot(ray, transform, this.weapon.GetType(), 0, false);
			}
			foreach (ShotTarget shotTarget2 in shot.Targets)
			{
				shotTarget2.EnergyDamage = 100;
			}
			LocalPlayerManager.Instance.ShotPlayer(1, shot);
			LocalPlayerManager.Instance.hitTurret(shot, this.LocalPlayer, true, 100);
			this.EffectManager.rgunEffect(shot, this.LocalPlayer, true);
			break;
		}
		}
	}

	private void DoShot2x()
	{
		WeaponType weaponType = (WeaponType)this.weapon.GetType();
		if (weaponType == WeaponType.MINE_REMOTE)
		{
			this.EffectManager.mineRemoteEffect(null, this.LocalPlayer, true, true);
		}
	}

	private void Reload()
	{
		this.weapon.Reload();
		int num = this.weapon.MaxLoadedAmmo - this.weapon.LoadedAmmo;
		if (num > this.weapon.AmmoReserve)
		{
			num = this.weapon.AmmoReserve;
		}
		this.weapon.UpdateAmmoCount(num, this.weapon.AmmoReserve - num);
        this.loadedAmmo = this.weapon.LoadedAmmo;
        this.ammoReserve = this.weapon.AmmoReserve;
	}

	private void QuickChange()
	{
		UnityEngine.Debug.Log("QuickChange: " + this.previousWeapon);
		if (this.previousWeapon != this.weapon.GetIndex())
		{
			int index = this.weapon.GetIndex();
			this.SetWeapon(this.previousWeapon);
			this.previousWeapon = index;
		}
	}

	private void Change(int weaponNum)
	{
		WeaponLook[] componentsInChildren = base.transform.GetComponentsInChildren<WeaponLook>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Active = false;
			if (componentsInChildren[i].transform.name == this.weapon.GetName() || componentsInChildren[i].transform.name == this.weapon.GetName() + "L" || componentsInChildren[i].transform.name == this.weapon.GetName() + "R")
			{
				componentsInChildren[i].Active = true;
			}
		}
		LocalGameHUD.Instance.Zoom(false);
	}

	public CombatWeapon GetWeaponByType(int weaponType)
	{
		foreach (CombatWeapon combatWeapon in this.weapons)
		{
			if (combatWeapon != null && combatWeapon.GetType() == weaponType)
			{
				return combatWeapon;
			}
		}
		return null;
	}

	public void AddRapidity(int weaponType, int amount)
	{
		foreach (CombatWeapon combatWeapon in this.weapons)
		{
			if (combatWeapon != null && combatWeapon.GetType() == weaponType)
			{
				combatWeapon.ShotTime += amount;
			}
		}
	}

	public void AddAmmoToReserve(int weaponType, int amount)
	{
		foreach (CombatWeapon combatWeapon in this.weapons)
		{
			if (combatWeapon != null && combatWeapon.GetType() == weaponType)
			{
				combatWeapon.AddAmmoToReserve(amount);
				if (this.weapon == combatWeapon)
				{
					this.loadedAmmo = this.weapon.LoadedAmmo;
					this.ammoReserve = this.weapon.AmmoReserve;
					if (this.loadedAmmo == 0 && this.ammoReserve > 0)
					{
						this.Reload();
					}
					this.loadedAmmo = this.weapon.LoadedAmmo;
					this.ammoReserve = this.weapon.AmmoReserve;
				}
			}
		}
	}

	public void ShootAmmo(int weaponType)
	{
		foreach (CombatWeapon combatWeapon in this.weapons)
		{
			if (combatWeapon != null && combatWeapon.GetType() == weaponType)
			{
				combatWeapon.ShootAmmo();
				if (this.weapon == combatWeapon)
				{
					this.loadedAmmo = this.weapon.LoadedAmmo;
					this.ammoReserve = this.weapon.AmmoReserve;
				}
				if (this.weapon.LoadedAmmo == 0 && this.weapon.AmmoReserve > 0)
				{
					this.Reload();
				}
			}
		}
	}

	public void UpdateAmmoCount(int index, int loadedAmmo, int ammoReserve)
	{
		if (this.weapons[index] != null)
		{
			this.weapons[index].UpdateAmmoCount(loadedAmmo, ammoReserve);
			if (this.weapon == this.weapons[index])
			{
				this.loadedAmmo = loadedAmmo;
				this.ammoReserve = ammoReserve;
			}
		}
		if (this.loadedAmmo == 0 && this.ammoReserve > 0)
		{
			this.Reload();
		}
	}

	private void CheckRaycastWithEnemy()
	{
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, 256))
		{
			raycastHit.collider.SendMessage("RaycastMessage", SendMessageOptions.DontRequireReceiver);
		}
	}

	public bool IsZoomAllowed
	{
		get
		{
			return this.weapon != null && (this.weapon.GetType() == 3 || this.weapon.GetType() == 11 || this.weapon.GetType() == 19);
		}
	}

	public bool SwitchZoom()
	{
		return true;
	}

	public string GetAmmoCount(int index)
	{
		if (this.weapons[index] == null)
		{
			return string.Empty;
		}
		return string.Empty + (this.weapons[index].AmmoReserve + this.weapons[index].LoadedAmmo);
	}

	public string GetEnhancerCount(int index)
	{
		if (this.enhancers[index] == null)
		{
			return string.Empty;
		}
		if (this.enhancers[index].Count < 1)
		{
			return string.Empty;
		}
		return string.Empty + this.enhancers[index].getCount();
	}

	public int getWeaponReload(int index)
	{
		if (this.weapons[index] == null)
		{
			return 9;
		}
		return this.weapons[index].getReload();
	}

	public int getEnhancerReload(int index)
	{
		if (this.enhancers[index] == null)
		{
			return 9;
		}
		return this.enhancers[index].getReload();
	}

	private static LocalShotController instance;

	public int WeaponNum = 12;

	private int loadedAmmo;

	private int ammoReserve;

	private bool shot;

	private CombatWeapon weapon;

	private CombatWeapon[] weapons;

	private CombatEnhancer[] enhancers;

	private int previousWeapon = 1;

	private CombatPlayer localPlayer;

	private EffectManager effectManager;

	private NetworkManager networkManager;
}
