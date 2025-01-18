// dnSpy decompiler from Assembly-CSharp.dll class: CombatPlayer
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
	public CombatPlayer(int actorNr)
	{
		this.playerID = actorNr;
	}

	public long SpawnTime
	{
		get
		{
			return this.spawnTime;
		}
	}

	public int Ping
	{
		get
		{
			return this.ping;
		}
		set
		{
			this.ping = value;
		}
	}

	private Dictionary<int, Transform[]> WeaponTransfoms
	{
		get
		{
			if (this.weaponTransfoms == null)
			{
				this.weaponTransfoms = new Dictionary<int, Transform[]>();
			}
			return this.weaponTransfoms;
		}
	}

	public bool IsTarget
	{
		set
		{
			if (this.info != null)
			{
				this.info.IsTarget = value;
			}
		}
	}

	public string Name
	{
		get
		{
			return this.name;
		}
		set
		{
			this.name = value;
		}
	}

	public int AuthID
	{
		get
		{
			return this.authID;
		}
		set
		{
			this.authID = value;
		}
	}

	public int Health
	{
		get
		{
			return this.health;
		}
		set
		{
			this.health = value;
		}
	}

	public int MaxHealth
	{
		get
		{
			return 100;
		}
		set
		{
			this.maxHealth = value;
		}
	}

	public int Energy
	{
		get
		{
			return this.energy;
		}
		set
		{
			this.energy = value;
		}
	}

	public int MaxEnergy
	{
		get
		{
			return 100;
		}
		set
		{
			this.maxEnergy = value;
		}
	}

	public short Team
	{
		get
		{
			return this.team;
		}
		set
		{
			this.team = value;
		}
	}

	public int Level { get; set; }

	public bool IsPremium
	{
		get
		{
			return this.isPremium;
		}
	}

	public bool IsGuest
	{
		get
		{
			return this.isGuest;
		}
	}

	public bool IsDead
	{
		get
		{
			return this.isDead;
		}
	}

	public bool IsZombie
	{
		get
		{
			return this.isZombie;
		}
	}

	public ZombieType ZombieType
	{
		get
		{
			return this.zombieType;
		}
	}

	public Dictionary<long, ItemTracer> RegisteredItems
	{
		get
		{
			return this.registeredItems;
		}
	}

	public Dictionary<int, Hashtable> DecoleInfo
	{
		get
		{
			return this.decoleInfo;
		}
	}

	public Dictionary<int, Hashtable> WearInfo
	{
		get
		{
			return this.wearInfo;
		}
	}

	public Dictionary<int, Hashtable> WeaponInfo
	{
		get
		{
			return this.weaponInfo;
		}
	}

	public Dictionary<int, Hashtable> ModuleInfo
	{
		get
		{
			return this.moduleInfo;
		}
	}

	public int GetTauntID(int tauntIndex)
	{
		if (this.taunts == null || !this.taunts.ContainsKey(tauntIndex))
		{
			return 1;
		}
		return this.taunts[tauntIndex];
	}

	public WalkController WalkController
	{
		get
		{
			if (this.walkController == null)
			{
				this.walkController = base.transform.GetComponentInChildren<WalkController>();
			}
			return this.walkController;
		}
	}

	public SoldierController SoldierController
	{
		get
		{
			if (this.soldierController == null)
			{
				this.soldierController = base.transform.GetComponentInChildren<SoldierController>();
			}
			return this.soldierController;
		}
	}

	public ActorAnimator ActorAnimator
	{
		get
		{
			if (this.actorAnimator == null)
			{
				this.actorAnimator = base.transform.GetComponentInChildren<ActorAnimator>();
			}
			return this.actorAnimator;
		}
	}

	public CharacterMotor CharacterMotor
	{
		get
		{
			if (this.characterMotor == null)
			{
				this.characterMotor = base.transform.GetComponentInChildren<CharacterMotor>();
			}
			return this.characterMotor;
		}
	}

	public SoldierCamera SoldierCamera
	{
		get
		{
			if (this.soldierCamera == null)
			{
				this.soldierCamera = base.transform.GetComponentInChildren<SoldierCamera>();
			}
			return this.soldierCamera;
		}
	}

	public ShotController ShotController
	{
		get
		{
			if (this.shotController == null)
			{
				this.shotController = base.transform.GetComponentInChildren<ShotController>();
			}
			return this.shotController;
		}
	}

	public PlayerCustomisator PlayerCustomisator
	{
		get
		{
			if (this.playerCustomisator == null)
			{
				this.playerCustomisator = base.transform.GetComponent<PlayerCustomisator>();
			}
			return this.playerCustomisator;
		}
	}

	public PlayerRemote PlayerRemote
	{
		get
		{
			if (this.playerRemote == null)
			{
				this.playerRemote = base.transform.GetComponent<PlayerRemote>();
			}
			return this.playerRemote;
		}
	}

	public WeaponController WeaponController
	{
		get
		{
			if (this.weaponController == null)
			{
				this.weaponController = base.transform.GetComponent<WeaponController>();
			}
			return this.weaponController;
		}
	}

	public LocalShotController LocalShotController
	{
		get
		{
			if (this.localShotController == null)
			{
				this.localShotController = base.transform.GetComponentInChildren<LocalShotController>();
			}
			return this.localShotController;
		}
	}

	public Flame Flame
	{
		get
		{
			if (this.flame == null)
			{
				this.flame = base.transform.GetComponentInChildren<Flame>();
			}
			return this.flame;
		}
	}

	public Camera Camera
	{
		get
		{
			if (this.camera == null)
			{
				this.InitCameras();
			}
			return this.camera;
		}
	}

	public Camera RailCamera
	{
		get
		{
			if (this.camera == null)
			{
				this.InitCameras();
			}
			return this.railCamera;
		}
	}

	public Transform Biped
	{
		get
		{
			if (this.biped == null)
			{
				this.biped = base.transform.FindChild("Bip01");
			}
			return this.biped;
		}
	}

	private void InitCameras()
	{
		Camera[] componentsInChildren = base.transform.GetComponentsInChildren<Camera>();
		foreach (Camera camera in componentsInChildren)
		{
			if (camera.gameObject.name == "MainCamera")
			{
				this.camera = camera;
			}
			else
			{
				this.railCamera = camera;
			}
		}
	}

	public AnimationSynchronizer AnimationSynchronizer
	{
		get
		{
			if (this.animationSynchronizer == null)
			{
				this.animationSynchronizer = base.transform.GetComponent<AnimationSynchronizer>();
			}
			return this.animationSynchronizer;
		}
	}

	public NetworkTransformReceiver NetworkTransformReceiver
	{
		get
		{
			if (this.networkTransformReceiver == null)
			{
				this.networkTransformReceiver = base.transform.GetComponent<NetworkTransformReceiver>();
			}
			return this.networkTransformReceiver;
		}
	}

	public NetworkTransformSender NetworkTransformSender
	{
		get
		{
			if (this.networkTransformSender == null)
			{
				this.networkTransformSender = base.transform.GetComponent<NetworkTransformSender>();
			}
			return this.networkTransformSender;
		}
	}

	public AudioSource Audio
	{
		get
		{
			return this.audio;
		}
		set
		{
			this.audio = value;
		}
	}

	public bool IsDominator
	{
		get
		{
			return this.isDominator;
		}
		set
		{
			this.isDominator = value;
			if (!this.ContainsEnhancer(EnhancerType.DisableDominationIcon))
			{
				this.info.SetDomination(this.isDominator);
			}
		}
	}

	public int ClanId
	{
		get
		{
			return this.clanId;
		}
		set
		{
			this.clanId = value;
		}
	}

	public string ClanTag
	{
		get
		{
			return this.clanTag;
		}
		set
		{
			this.clanTag = value;
		}
	}

	public int ClanArmId
	{
		get
		{
			return this.clanArmId;
		}
		set
		{
			this.clanArmId = value;
		}
	}

	public void Init()
	{
		this.registeredItems = new Dictionary<long, ItemTracer>();
	}

	public void Init(int actorNr, Hashtable actorData, AudioSource audio)
	{
		this.playerID = actorNr;
		this.name = (string)actorData[(byte)242];
		this.authID = (int)actorData[(byte)241];
		this.registeredItems = new Dictionary<long, ItemTracer>();
		Hashtable hashtable = (Hashtable)actorData[(byte)96];
		this.Speed = (int)hashtable[(byte)95] / 10;
		this.Jump = (int)hashtable[(byte)92];
		this.health = (int)hashtable[(byte)100];
		this.energy = (int)hashtable[(byte)99];
		this.weaponInfo = (Dictionary<int, Hashtable>)hashtable[(byte)94];
		this.Level = (int)hashtable[(byte)76];
		if (hashtable.ContainsKey((byte)46))
		{
			this.moduleInfo = (Dictionary<int, Hashtable>)hashtable[(byte)46];
		}
		if (actorData.ContainsKey((byte)239))
		{
			this.team = (short)actorData[(byte)239];
		}
		if (hashtable.ContainsKey((byte)8))
		{
			this.ClanId = (int)hashtable[(byte)8];
		}
		if (hashtable.ContainsKey((byte)7))
		{
		}
		if (hashtable.ContainsKey((byte)6))
		{
			this.ClanTag = (string)hashtable[(byte)6];
		}
		if (hashtable.ContainsKey((byte)5))
		{
			this.clanArmId = (int)hashtable[(byte)5];
		}
		this.isPremium = Convert.ToBoolean(hashtable[(byte)36]);
		Hashtable hashtable2 = new Hashtable();
		hashtable2[(byte)153] = this.playerID;
		if (hashtable.ContainsKey((byte)30))
		{
			this.wearInfo = (Dictionary<int, Hashtable>)hashtable[(byte)30];
		}
		if (hashtable.ContainsKey((byte)4))
		{
			this.isGuest = true;
		}
		if (hashtable.ContainsKey((byte)106))
		{
			Dictionary<int, int> dictionary = (Dictionary<int, int>)hashtable[(byte)106];
			this.taunts = new Dictionary<int, int>();
			foreach (KeyValuePair<int, int> keyValuePair in dictionary)
			{
				this.taunts.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		if (hashtable.ContainsKey((byte)108))
		{
			Dictionary<byte, Hashtable> dictionary2 = (Dictionary<byte, Hashtable>)hashtable[(byte)108];
			this.enhancers = new Dictionary<byte, Hashtable>();
			foreach (KeyValuePair<byte, Hashtable> keyValuePair2 in dictionary2)
			{
				this.enhancers.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
		}
		this.isDead = true;
		this.health = 0;
		this.energy = 0;
		this.Hide();
		UnityEngine.Debug.Log(string.Format("Init Player {0} No.{1} ID:{2}", this.name, actorNr, this.authID));
		this.audio = audio;
		if (this.ShotController != null)
		{
			QualityLevel qualityLevel = OptionsManager.QualityLevel;
			this.ShotController.SetDiffuseShader(qualityLevel != QualityLevel.Fast && qualityLevel != QualityLevel.Fastest);
		}
	}

	public bool ContainsEnhancer(EnhancerType enhancerType)
	{
		return this.enhancers != null && this.enhancers.ContainsKey((byte)enhancerType);
	}

	public void InitSecurity(bool isLocal)
	{
		this.combatPlayerSecurity = new CombatPlayerSecurity(this);
	}

	public int Check()
	{
		int num = this.CharacterMotor.Check();
		if (num > 0)
		{
			return num;
		}
		return this.combatPlayerSecurity.Check(this);
	}

	public void Spawn(Transform parent, Vector3 position, Vector3 rotation, short team, int health, int energy, bool resurrect, ZombieType zombieType)
	{
        this.spawnTime = TimeManager.Instance.NetworkTime;
		base.transform.parent = parent;
		base.transform.position = position;
		base.transform.localEulerAngles = rotation;
		if (this.SoldierCamera != null)
		{
			this.SoldierCamera.ResetRotation(rotation.y, 0f);
		}
		this.team = team;
		this.health = health;
		this.energy = energy;
		this.isDead = false;
		this.setPlayerShipVisible(true);
        if (this.info != null)
		{
			this.info.Show();
			this.info.SetTeam((int)this.team);
			if (!this.ContainsEnhancer(EnhancerType.DisableDominationIcon))
			{
				this.info.SetDomination(this.isDominator);
			}
		}
		PlayerRemote component = base.transform.GetComponent<PlayerRemote>();
		if (component == null)
		{
			return;
		}
		component.Dead = false;
		if (this != PlayerManager.Instance.LocalPlayer)
		{
			component.ResurrectShield.SetActive(resurrect);
			if (resurrect)
			{
				base.StartCoroutine(this.DelayedResurrectShieldRemove(component));
			}
		}
		if (PlayerManager.Instance.RoomSettings.GameMode == MapMode.MODE.ZOMBIE)
		{
			this.SetupZombiePlayer(this.team, this.health, this.energy, zombieType);
		}
    }

	public void SetupZombiePlayer(short team, int health, int energy, ZombieType zombieType)
	{
		this.zombieType = zombieType;
		this.team = team;
		this.health = health;
		this.energy = energy;
        if (team == 1)
		{
            this.SetZomie();
        }
		else
		{
			this.UnsetZomie();
		}
		if (this.info != null)
		{
			this.info.SetTeam((int)this.team);
		}
	}

	private void SetZomie()
	{
		this.isZombie = true;
		this.ActorAnimator.SetZombieAnimation(true);
		if (this == PlayerManager.Instance.LocalPlayer)
		{
			this.ShotController.ResetWeapon();
			this.setPlayerShipVisible(false);
			this.setPlayerShipVisible(true);
			this.ShotController.InitZombieWeapon(true);
		}
		else
		{
			SoundManager.Instance.Play(this.Audio, "M134_Stop", AudioPlayMode.Stop);
			this.Audio.Stop();
			this.WeaponController.InitZombieWeapon(true);
			this.InitWear(true, false);
		}
		this.ZombieSound("ZombieSound_Inf");
		this.SetCameraColor(1.2f, 1f, 0.8f);
		if (this.SoldierController != null)
		{
			this.SoldierController.TPSCamera.fieldOfView = 75f;
			this.SoldierController.FPSCamera.fieldOfView = 90f;
		}
	}

	private void RandomZombieSound()
	{
		int num = (int)Math.Ceiling((double)(UnityEngine.Random.value * 6f));
		if (num <= 3)
		{
			this.ZombieSound("ZombieSound_" + num);
		}
	}

	private void ZombieSound(string soundName)
	{
		if (this.zombieType == ZombieType.Boss)
		{
		}
		EffectManager.Instance.PlayerSoundEffect(this.Audio, soundName);
	}

	private void UnsetZomie()
	{
		this.isZombie = false;
		this.ActorAnimator.SetZombieAnimation(false);
		this.InitWear(true, false);
		if (this == PlayerManager.Instance.LocalPlayer)
		{
			this.ShotController.InitZombieWeapon(false);
		}
		this.SetCameraColor(1f, 1f, 1f);
		if (this.SoldierController != null)
		{
			this.SoldierController.TPSCamera.fieldOfView = 50f;
			this.SoldierController.FPSCamera.fieldOfView = 60f;
		}
	}

	private void SetCameraColor(float r, float g, float b)
	{
		foreach (ColorCorrectionCurves colorCorrectionCurves in base.transform.GetComponentsInChildren<ColorCorrectionCurves>(true))
		{
			colorCorrectionCurves.enabled = false;
			Keyframe keyframe = colorCorrectionCurves.redChannel.keys[1];
			Keyframe keyframe2 = colorCorrectionCurves.blueChannel.keys[1];
			Keyframe keyframe3 = colorCorrectionCurves.greenChannel.keys[1];
			keyframe.value = r;
			keyframe3.value = g;
			keyframe2.value = b;
			colorCorrectionCurves.redChannel = new AnimationCurve(new Keyframe[]
			{
				colorCorrectionCurves.redChannel.keys[0],
				keyframe
			});
			colorCorrectionCurves.greenChannel = new AnimationCurve(new Keyframe[]
			{
				colorCorrectionCurves.greenChannel.keys[0],
				keyframe3
			});
			colorCorrectionCurves.blueChannel = new AnimationCurve(new Keyframe[]
			{
				colorCorrectionCurves.blueChannel.keys[0],
				keyframe2
			});
		}
	}

	private IEnumerator DelayedResurrectShieldRemove(PlayerRemote playerRemote)
	{
		yield return new WaitForSeconds(3.5f);
		playerRemote.ResurrectShield.SetActive(false);
		yield break;
	}

	public void Kill()
	{
		this.isDead = true;
		this.health = 0;
		this.energy = 0;
		if (this.info != null)
		{
			this.info.Hide();
		}
		if (this.WeaponController != null)
		{
			this.WeaponController.HideWeapons();
		}
		else
		{
			this.setPlayerShipVisible(false);
		}
		PlayerRemote component = base.transform.GetComponent<PlayerRemote>();
		if (component == null)
		{
			return;
		}
		component.Dead = true;
		SoundManager.Instance.Play(this.Audio, "M134_Stop", AudioPlayMode.Stop);
		this.Audio.Stop();
		this.DisactivateBombs();
	}

	public void DisactivateBombs()
	{
		List<long> list = new List<long>();
		foreach (ItemTracer itemTracer in this.registeredItems.Values)
		{
			if (itemTracer.WeaponType == WeaponType.BOMB_LAUNCHER)
			{
				itemTracer.Destroy();
				list.Add(itemTracer.TimeStamp);
			}
		}
		foreach (long key in list)
		{
			this.registeredItems.Remove(key);
		}
	}

	public void Hide()
	{
		this.setPlayerShipVisible(false);
	}

	public void OnPaintLoaded(bool success, AssetBundle assetBundle, Hashtable parameters)
	{
		GameObject gameObject = base.transform.gameObject;
		Transform transform = gameObject.transform.FindChild("SShip");
		if (transform == null)
		{
			return;
		}
		Transform transform2 = transform.FindChild("ShipModel");
		MeshRenderer component = transform2.GetComponent<MeshRenderer>();
		Texture2D texture = assetBundle.LoadAllAssets()[0] as Texture2D;
		if (component != null)
		{
			component.material.SetTexture("_MainTex", texture);
		}
	}

	public void DestroyItems()
	{
		try
		{
			foreach (ItemTracer itemTracer in this.registeredItems.Values)
			{
				if (!(itemTracer == null))
				{
					itemTracer.Destroy();
				}
			}
			this.registeredItems.Clear();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("[Combat Player] Destroy Items Exception: " + ex.Message);
		}
	}

	public void RegisterItem(ItemTracer item)
	{
		if (!this.registeredItems.ContainsKey(item.TimeStamp))
		{
			this.registeredItems.Add(item.TimeStamp, item);
		}
	}

	public void UnregisterItem(long itemTimeStamp)
	{
		if (this.registeredItems.ContainsKey(itemTimeStamp))
		{
			ItemTracer itemTracer = this.registeredItems[itemTimeStamp];
			if (itemTracer != null)
			{
				itemTracer.Destroy();
			}
			this.registeredItems.Remove(itemTimeStamp);
		}
	}

	public override string ToString()
	{
		return this.playerID + "'" + this.name;
	}

	public void InitEnemyInfo()
	{
		this.info = base.transform.GetComponentInChildren<EnemyInfo>();
		if (this.clanTag != string.Empty)
		{
			this.info.SetName(string.Format("[{0}] {1}", this.clanTag, this.name));
		}
		else
		{
			this.info.SetName(this.name);
		}
		this.info.SetTeam((int)this.team);
		this.showingInfo = false;
		if (!this.ContainsEnhancer(EnhancerType.DisableDominationIcon))
		{
			this.info.SetDomination(this.isDominator);
		}
	}

	public void InitWear(bool show, bool cleanZombie)
	{
		if (this.isZombie)
		{
			string text = string.Empty;
			if (this.zombieType == ZombieType.Boss)
			{
				text += "boss";
			}
			this.PlayerCustomisator.SetCloth(CCWearType.Boots, "Boots_zombie");
			this.PlayerCustomisator.SetCloth(CCWearType.Gloves, "Gloves_zombie" + text);
			this.PlayerCustomisator.SetCloth(CCWearType.Heads, "Heads_zombie" + text);
			this.PlayerCustomisator.SetCloth(CCWearType.Shirts, "Shirts_zombie" + text);
			this.PlayerCustomisator.SetCloth(CCWearType.Pants, "Pants_zombie" + text);
			this.PlayerCustomisator.SetCloth(CCWearType.Backpacks, string.Empty);
			this.PlayerCustomisator.SetCloth(CCWearType.Masks, string.Empty);
			this.PlayerCustomisator.SetCloth(CCWearType.Hats, string.Empty);
		}
		else if (!cleanZombie)
		{
			this.PlayerCustomisator.SetCloth(CCWearType.Boots, string.Empty);
			this.PlayerCustomisator.SetCloth(CCWearType.Gloves, string.Empty);
			this.PlayerCustomisator.SetCloth(CCWearType.Heads, string.Empty);
			this.PlayerCustomisator.SetCloth(CCWearType.Shirts, string.Empty);
			this.PlayerCustomisator.SetCloth(CCWearType.Pants, string.Empty);
		}
		if (!this.isZombie)
		{
			if (this.wearInfo != null && this.wearInfo.Count != 0)
			{
				foreach (Hashtable hashtable in this.wearInfo.Values)
				{
					CCWearType ccwearType = (CCWearType)((int)hashtable[(byte)98]);
					string system_name = (string)hashtable[(byte)99];
					this.PlayerCustomisator.SetCloth(ccwearType, Wear.GetModelName(ccwearType, system_name));
				}
			}
			else
			{
				this.PlayerCustomisator.SetDefaultCloth();
			}
		}
		if (cleanZombie)
		{
			this.PlayerCustomisator.CleanCloth();
		}
		if (!show)
		{
			this.PlayerCustomisator.HideCloth();
			return;
		}
	}

	public void InitTaunt(bool show, int tauntID)
	{
        this.PlayerCustomisator.InitTaunt(show, tauntID);
    }

	public void ResumeWeapon()
	{
		if (this.isDead)
		{
			return;
		}
		if (this.ShotController != null)
		{
			this.ShotController.ResumeWeapon();
		}
		if (this.WeaponController != null)
		{
			this.WeaponController.ResumeWeapon();
		}
	}

	public void InitWeapon()
	{
		if (this.weaponInfo != null)
		{
			for (int i = 0; i < 7; i++)
			{
				if (this.weaponInfo.ContainsKey(i))
				{
					if (this.isZombie && i == 0)
					{
						this.ShotController.InitZombieWeapon(true);
					}
					else
					{
						int num = this.ShotController.InitWeapon(i, this.weaponInfo[i]);
					}
				}
			}
		}
		this.combatPlayerSecurity.InitWeapon(this);
	}

	public void InitEnemyWeapon()
	{
		if (this.isZombie)
		{
			return;
		}
		if (this.weaponInfo != null)
		{
			for (int i = 0; i < 7; i++)
			{
				if (this.weaponInfo.ContainsKey(i))
				{
					int num = this.WeaponController.InitWeapon(i, this.weaponInfo[i]);
				}
			}
		}
	}

	public void InitCostume(Hashtable costume)
	{
	}

	private void FixedUpdate()
	{
		if (!this.IsZombie)
		{
			this.lastRandomEffect = 0L;
			return;
		}
		if (this.IsDead)
		{
			return;
		}
		if (this.lastRandomEffect + 200000000L < DateTime.Now.Ticks)
		{
			if (this.lastRandomEffect != 0L)
			{
				this.RandomZombieSound();
			}
			this.lastRandomEffect = DateTime.Now.Ticks;
		}
	}

	public void UpdateEnhancer(Hashtable enhancerData)
	{
		int num = (int)enhancerData[(byte)48];
		if (enhancerData.ContainsKey((byte)99))
		{
			int num2 = (int)enhancerData[(byte)99];
			this.energy += num2;
		}
		if (enhancerData.ContainsKey((byte)47))
		{
			int amount = (int)enhancerData[(byte)47];
			int weaponType = (int)((byte)enhancerData[(byte)89]);
			this.ShotController.AddAmmoToReserve(weaponType, amount);
		}
		if (enhancerData.ContainsKey((byte)88))
		{
			int num3 = (int)enhancerData[(byte)88];
			int num4 = (int)((byte)enhancerData[(byte)89]);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"WeaponDamage: ",
				num3,
				" weaponType = ",
				num4
			}));
			if (this.ShotController.CurrentWeapon.GetType() == num4)
			{
				if (num3 > 0)
				{
					this.ShotController.CurrentWeapon.SetEnhancerMode(EnhancerMode.Damage);
				}
				else
				{
					this.ShotController.CurrentWeapon.UnsetEnhancerMode(EnhancerMode.Damage);
				}
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					if (this.ShotController.Weapons[i] != null)
					{
						if (this.ShotController.Weapons[i].GetType() == num4)
						{
							if (num3 > 0)
							{
								this.ShotController.Weapons[i].SetEnhancerMode(EnhancerMode.Damage);
							}
							else
							{
								this.ShotController.Weapons[i].UnsetEnhancerMode(EnhancerMode.Damage);
							}
							break;
						}
					}
				}
			}
		}
		if (enhancerData.ContainsKey((byte)83))
		{
			int num5 = (int)enhancerData[(byte)83];
			int num6 = (int)((byte)enhancerData[(byte)89]);
			this.ShotController.AddRapidity(num6, num5);
			if (this.ShotController.CurrentWeapon.GetType() == num6)
			{
				if (num5 < 0)
				{
					this.ShotController.CurrentWeapon.SetEnhancerMode(EnhancerMode.Rapidity);
				}
				else
				{
					this.ShotController.CurrentWeapon.UnsetEnhancerMode(EnhancerMode.Rapidity);
				}
			}
			else
			{
				for (int j = 0; j < 5; j++)
				{
					if (this.ShotController.Weapons[j] != null)
					{
						if (this.ShotController.Weapons[j].GetType() == num6)
						{
							if (num5 < 0)
							{
								this.ShotController.Weapons[j].SetEnhancerMode(EnhancerMode.Rapidity);
							}
							else
							{
								this.ShotController.Weapons[j].UnsetEnhancerMode(EnhancerMode.Rapidity);
							}
							break;
						}
					}
				}
			}
		}
		if (enhancerData.ContainsKey((byte)95))
		{
			int num7 = (int)enhancerData[(byte)95];
			this.walkController.setSpeed(this.walkController.getIntSpeed() + num7);
			if (num7 < 0)
			{
			}
		}
		if (!enhancerData.ContainsKey((byte)31))
		{
		}
	}

	public void HideWeapons()
	{
		if (this.WeaponController != null)
		{
			this.WeaponController.HideWeapons();
		}
		if (this.ShotController != null)
		{
			this.ShotController.HideWeapons();
		}
	}

	public void ShowWeapons()
	{
		this.HideWeapons();
		if (this.weaponInfo != null)
		{
			for (int i = 0; i < 5; i++)
			{
				if (this.weaponInfo.ContainsKey(i))
				{
					Hashtable hashtable = this.weaponInfo[i];
					int index = (int)hashtable[(byte)98];
					string systemName = (string)hashtable[(byte)99];
					this.showHidePlayerWeapon(index, true, systemName);
				}
			}
		}
	}

	public void setPlayerShipVisible(bool visible)
	{
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter particleEmitter in componentsInChildren)
		{
			if (particleEmitter.gameObject.name != "Source")
			{
				particleEmitter.emit = false;
			}
		}
		Renderer[] componentsInChildren2 = base.transform.GetComponentsInChildren<Renderer>();
		if (this.info != null)
		{
			this.info.Visible = visible;
		}
		foreach (Renderer renderer in componentsInChildren2)
		{
			if (renderer.gameObject.name == "nemesis")
			{
				if (this.isDominator)
				{
					renderer.enabled = visible;
				}
				else
				{
					renderer.enabled = false;
				}
			}
			if (!(PlayerManager.Instance.LocalPlayer == this) || !renderer.gameObject.name.Contains("guybrush"))
			{
				if (renderer.gameObject.name != "Source")
				{
					renderer.enabled = visible;
				}
			}
		}
		Collider[] componentsInChildren3 = base.transform.GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren3)
		{
			collider.enabled = visible;
		}
	}

	public bool CanCheckNickName(CombatPlayer LocalPlayer)
	{
		return !this.IsDead && !(LocalPlayer == null) && LocalPlayer.Team >= 0 && !LocalPlayer.IsDead && this.Team >= 0 && (this.Team != LocalPlayer.Team || this.Team == 0);
	}

	public bool CheckNickname(CombatPlayer LocalPlayer)
	{
		return this.CheckEnemyInfo();
	}

	public bool CheckEnemyInfo()
	{
		return !this.info.userName.GetComponent<Renderer>().enabled;
	}

	public bool CheckNicknameCheating(bool nicknameCorrect)
	{
		return this.combatPlayerSecurity.CheckNicknameCheating(nicknameCorrect);
	}

	public Transform getWeaponTransformByName(Transform playerTransform, string weaponName)
	{
		Transform result = null;
		WeaponLook[] componentsInChildren = playerTransform.GetComponentsInChildren<WeaponLook>(true);
		foreach (WeaponLook weaponLook in componentsInChildren)
		{
			if (weaponLook.gameObject.name == weaponName)
			{
				result = weaponLook.transform;
				break;
			}
		}
		return result;
	}

	public CombatWeapon GetWeaponByType(WeaponType weaponType)
	{
		if (this == PlayerManager.Instance.LocalPlayer)
		{
			return this.ShotController.GetWeaponByType((int)weaponType);
		}
		return this.WeaponController.GetWeaponByType((int)weaponType);
	}

	public void showHidePlayerWeapon(int index, bool show, string systemName)
	{
		Transform[] array;
		if (this.WeaponTransfoms.ContainsKey(index))
		{
			array = this.WeaponTransfoms[index];
		}
		else
		{
			array = new Transform[2];
			string text = CombatWeapon.getName((WeaponType)index);
			Transform weaponTransformByName = this.getWeaponTransformByName(base.transform, text);
			if (weaponTransformByName != null)
			{
				array[0] = weaponTransformByName;
			}
			else
			{
				weaponTransformByName = this.getWeaponTransformByName(base.transform, text + "L");
				if (weaponTransformByName != null)
				{
					array[0] = weaponTransformByName;
				}
				weaponTransformByName = this.getWeaponTransformByName(base.transform, text + "R");
				if (weaponTransformByName != null)
				{
					array[1] = weaponTransformByName;
				}
			}
			this.WeaponTransfoms[index] = array;
		}
		foreach (Transform transform in array)
		{
			if (transform != null)
			{
				foreach (MeshRenderer meshRenderer in transform.GetComponentsInChildren<MeshRenderer>(true))
				{
					if (!show)
					{
						meshRenderer.enabled = show;
					}
					else if ((byte)index == 1 || ((byte)index == 2 && meshRenderer.gameObject.name == systemName))
					{
						meshRenderer.enabled = show;
					}
				}
			}
		}
	}

	public void SetAnimationState(byte state)
	{
		PlayerRemote component = base.transform.GetComponent<PlayerRemote>();
		if (component == null)
		{
			return;
		}
		if (Datameter.enabled)
		{
			if (component.CrouchStatus != ((state & 1) == 1) || component.InAir != ((state & 4) == 4))
			{
				Datameter.JumpCrouchCounter++;
			}
			else if (component.Walk != ((state & 2) == 2))
			{
				Datameter.AnimationCounter++;
			}
		}
		component.CrouchStatus = ((state & 1) == 1);
		component.Walk = ((state & 2) == 2);
		component.InAir = ((state & 4) == 4);
	}

	public void SetAnimationKey(byte key)
	{
		PlayerRemote component = base.transform.GetComponent<PlayerRemote>();
		if (component == null)
		{
			return;
		}
		component.keyState = (KeyState)key;
	}

	private const int RespawnDieDelay = 3500;

	private const long Random_Effect_Interval = 200000000L;

	public const int Zombie_Sound_Count = 6;

	private long spawnTime;

	private int ping;

	private CombatPlayerSecurity combatPlayerSecurity;

	private long lastRandomEffect;

	private Dictionary<int, Transform[]> weaponTransfoms;

	private new string name = string.Empty;

	private int authID = -1;

	private int health = -1;

	private int maxHealth = 100;

	private int energy = -1;

	private int maxEnergy = 100;

	public int playerID = -1;

	private short team = -1;

	public bool PlayerIsLocal;

	public int ShipTypeID = -1;

	public int ShipID = -1;

	public int PaintID = -1;

	public int Jump = -1;

	public int Speed = -1;

	private bool isDead = true;

	private bool isPremium;

	private bool isGuest;

	private bool isZombie;

	private ZombieType zombieType;

	private Dictionary<long, ItemTracer> registeredItems;

	private Dictionary<int, Hashtable> decoleInfo;

	private Dictionary<int, Hashtable> wearInfo;

	private Dictionary<int, Hashtable> weaponInfo;

	private Dictionary<int, Hashtable> moduleInfo;

	private Dictionary<int, int> taunts;

	public static bool isSendReliable;

	private WalkController walkController;

	private SoldierController soldierController;

	private ActorAnimator actorAnimator;

	private CharacterMotor characterMotor;

	private SoldierCamera soldierCamera;

	private ShotController shotController;

	private PlayerCustomisator playerCustomisator;

	private PlayerRemote playerRemote;

	private WeaponController weaponController;

	private LocalShotController localShotController;

	private Flame flame;

	private new Camera camera;

	private Camera railCamera;

	private Transform biped;

	private AnimationSynchronizer animationSynchronizer;

	private NetworkTransformReceiver networkTransformReceiver;

	private NetworkTransformSender networkTransformSender;

	private new AudioSource audio;

	private EnemyInfo info;

	private bool showingInfo;

	private bool isDominator;

	private int clanId;

	private string clanTag = string.Empty;

	private int clanArmId;

	private Dictionary<byte, Hashtable> enhancers = new Dictionary<byte, Hashtable>();
}
