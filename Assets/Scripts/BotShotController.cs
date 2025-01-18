// dnSpy decompiler from Assembly-CSharp.dll class: BotShotController
using System;
using System.Collections;
using UnityEngine;

public class BotShotController : MonoBehaviour
{
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

	private CombatPlayer Player
	{
		get
		{
			if (this.player == null)
			{
				this.player = base.transform.GetComponent<CombatPlayer>();
			}
			return this.player;
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
	}

	public void UseEnhancer(int enhancerNum)
	{
		if (this.enhancers[enhancerNum - 1] != null && this.enhancers[enhancerNum - 1].Fire())
		{
			this.NetworkManager.SendEnhancer(enhancerNum);
			UnityEngine.Debug.Log("Use enhancer " + enhancerNum);
		}
	}

	public Vector3 CurrentLookTarget
	{
		get
		{
			return this.currentLookTarget;
		}
	}

	public bool IsPlayerTarget
	{
		get
		{
			return this.isPlayerTarget;
		}
	}

	public int InitWeapons()
	{
		this.weapons[0] = new CombatWeapon(0, WeaponType.HAND_GUN, 240, 2000, 100, 0, 0);
		this.weapons[0].UpdateAmmoCount(500, 1000);
		this.weapons[0].UpdateAmmoCount(0, 0);
		this.weapons[1] = new CombatWeapon(1, WeaponType.ROCKET_LAUNCHER, 120, 2000, 1, 20, 0);
		this.weapons[1].UpdateAmmoCount(0, 0);
		this.weapons[3] = new CombatWeapon(4, WeaponType.MACHINE_GUN, 120, 2000, 1, 500, 0);
		this.weapons[3].UpdateAmmoCount(500, 5000);
		this.weapons[4] = new CombatWeapon(5, WeaponType.FLAMER, 120, 2000, 1, 500, 0);
		this.weapons[4].UpdateAmmoCount(500, 5000);
		int num = 0;
		if (num == 0)
		{
			this.weapon = this.weapons[num];
			this.loadedAmmo = this.weapon.LoadedAmmo;
			this.ammoReserve = this.weapon.AmmoReserve;
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
		this.weapons = new CombatWeapon[10];
		this.enhancers = new CombatEnhancer[2];
		this.InitWeapons();
	}

	private void Start()
	{
		this.navigationController = base.transform.GetComponent<BotNavigationController>();
		this.currentLookTargetDistance = 10000f;
		this.combatPlayer = base.transform.GetComponent<CombatPlayer>();
	}

	private void Update()
	{
		if (this.weapon.Fire())
		{
			this.DoShot();
		}
	}

	public static float ClampAngle180(float angle, float min, float max)
	{
		while (angle > 180f)
		{
			angle -= 360f;
		}
		while (angle < -180f)
		{
			angle += 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	private float ScanForEnemies(Ray weaponRay)
	{
		float num = 180f;
		float num2 = 10000f;
		Vector3 vector = this.navigationController.MoveDirection;
		foreach (CombatPlayer combatPlayer in LocalPlayerManager.Instance.Players.Values)
		{
			if (!combatPlayer.IsDead)
			{
				if (combatPlayer == LocalPlayerManager.Instance.LocalPlayer)
				{
					Vector3 from = combatPlayer.transform.position - weaponRay.origin;
					float num3 = Mathf.Abs(BotShotController.ClampAngle180(Mathf.Abs(Vector3.Angle(from, weaponRay.direction)), -180f, 180f));
					if (num3 < num)
					{
						Ray ray = new Ray(weaponRay.origin, combatPlayer.transform.position - weaponRay.origin);
						int num4 = ShotCalculator.StraightScan(ray, true);
						if (num4 != -1)
						{
							num = num3;
							num2 = from.magnitude;
							vector = combatPlayer.transform.position;
						}
					}
				}
			}
		}
		if (num > 90f || num2 > 100f)
		{
			this.currentLookTarget = this.navigationController.MoveDirection;
			this.isPlayerTarget = false;
			this.currentLookTargetDistance = num2;
			return num;
		}
		this.currentLookTargetDistance = num2;
		this.currentLookTarget = vector;
		this.isPlayerTarget = true;
		return num;
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
		float num2;
		if (this.weapon.GetType() == 1 || this.weapon.GetType() == 7 || this.weapon.GetType() == 10)
		{
			num2 = 0.004f * (num + 10f);
		}
		num2 = 0.15f;
		float x = Convert.ToSingle(random.NextDouble() * (double)num2) - num2 / 2f;
		float y = Convert.ToSingle(random.NextDouble() * (double)num2) - num2 / 2f;
		Vector3 direction = transform.transform.TransformDirection(new Vector3(x, y, 1f));
		Ray ray = new Ray(transform.transform.position, direction);
		float num3 = this.ScanForEnemies(ray);
		if (num3 > 30f)
		{
			return;
		}
		CombatPlayer localPlayer = LocalPlayerManager.Instance.LocalPlayer;
		if (localPlayer.IsDead)
		{
			return;
		}
		switch ((byte)this.weapon.GetType())
		{
		case 3:
		case 4:
		case 6:
		case 7:
		{
			Shot shot = ShotCalculator.StraightShot(ray, transform, this.weapon.GetType(), 0, true);
			foreach (ShotTarget shotTarget in shot.Targets)
			{
				shotTarget.EnergyDamage = 2;
			}
			if ((shot.Origin - ray.origin).magnitude + 10f < this.currentLookTargetDistance && this.currentLookTargetDistance != 10000f)
			{
				return;
			}
			LocalPlayerManager.Instance.ShotPlayer(this.combatPlayer.playerID, shot);
			this.EffectManager.machineGunEffect(shot, this.combatPlayer, false);
			break;
		}
		case 5:
		{
			Shot shot = ShotCalculator.StraightShot(ray, transform, this.weapon.GetType(), 0, true);
			LocalPlayerManager.Instance.ShotPlayer(1, shot);
			this.EffectManager.flamerEffect(shot, this.Player, true);
			break;
		}
		case 8:
		{
			Shot shot = ShotCalculator.RocketShot(ray, transform, this.weapon.GetType(), (float)this.weapon.Velocity);
			LocalPlayerManager.Instance.ShotPlayer(1, shot);
			this.EffectManager.rocketLauncherEffect(shot, this.Player, true);
			break;
		}
		case 9:
		{
			Shot shot = ShotCalculator.GrenadeShot(ray, transform, this.weapon.GetType(), (float)this.weapon.Velocity, (float)this.weapon.Life, false);
			this.NetworkManager.SendShot(shot);
			this.EffectManager.grenadeLauncherEffect(shot, this.Player, true);
			break;
		}
		case 10:
		{
			Shot shot = ShotCalculator.StraightShot(ray, transform, this.weapon.GetType(), 0, true);
			this.NetworkManager.SendShot(shot);
			this.EffectManager.rgunEffect(shot, this.Player, true);
			break;
		}
		case 15:
		{
			Shot shot = ShotCalculator.GrenadeShot(ray, transform, this.weapon.GetType(), (float)this.weapon.Velocity, (float)this.weapon.Life, true);
			this.NetworkManager.SendShot(shot);
			this.EffectManager.grenadeLauncherEffect(shot, this.Player, true);
			break;
		}
		}
	}

	private void DoShot2x()
	{
		WeaponType weaponType = (WeaponType)this.weapon.GetType();
		if (weaponType == WeaponType.MINE_REMOTE)
		{
			this.EffectManager.mineRemoteEffect(null, this.Player, true, true);
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

	public void UpdateAmmoCount(int index, int loadedAmmo, int ammo)
	{
		if (this.weapons[index] != null)
		{
			this.weapons[index].UpdateAmmoCount(loadedAmmo, ammo);
			if (this.weapon == this.weapons[index])
			{
				this.loadedAmmo = loadedAmmo;
				this.ammoReserve = this.ammoReserve;
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

	public bool zoomAllowed()
	{
		return this.weapon != null && (this.weapon.GetType() == 3 || this.weapon.GetType() == 11);
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

	public int WeaponNum = 12;

	private int loadedAmmo;

	private int ammoReserve;

	private bool shot;

	private CombatWeapon weapon;

	private CombatWeapon[] weapons;

	private CombatEnhancer[] enhancers;

	private int previousWeapon = 1;

	private CombatPlayer player;

	private EffectManager effectManager;

	private NetworkManager networkManager;

	private float currentLookTargetDistance;

	private Vector3 currentLookTarget;

	private bool isPlayerTarget;

	private BotNavigationController navigationController;

	private CombatPlayer combatPlayer;
}
