// dnSpy decompiler from Assembly-CSharp.dll class: ShotController
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : WeaponController
{
	public static ShotController Instance
	{
		get
		{
			return ShotController.instance;
		}
	}

	public RaycastHit Hit
	{
		get
		{
			return this.currentHit;
		}
		set
		{
			this.currentHit = value;
			if (this.currentHit.transform != null && (this.currentHit.transform.name.StartsWith("Ship") || this.currentHit.transform.name.StartsWith("Bip")))
			{
				CombatPlayer component = this.currentHit.transform.parent.parent.GetComponent<CombatPlayer>();
				if (component != null)
				{
					component.IsTarget = true;
				}
			}
		}
	}

	public bool IsTaunt
	{
		get
		{
			return this.isTaunt;
		}
	}

	public bool Zoom
	{
		get
		{
			return this.zoom;
		}
		set
		{
			this.zoom = value;
			if (this.weapon.Type == WeaponType.SNIPER_RIFLE || !this.zoom)
			{
				GameHUD.Instance.Zoom(this.zoom);
			}
		}
	}

	public bool IsShooting
	{
		get
		{
			return this.shot;
		}
	}

	public bool IsShootingAlt
	{
		get
		{
			return this.shotAlt;
		}
	}

	public bool IsBlocked
	{
		get
		{
			return base.CurrentWeapon.IsWeaponBlocked();
		}
	}

	public string GetAmmoCountString()
	{
		if (this.weapon == null)
		{
			return "0/0";
		}
		return this.weapon.LoadedAmmo + "/" + this.weapon.AmmoReserve;
	}

	public int Ammo
	{
		get
		{
			if (this.weapon == null)
			{
				return 0;
			}
			return this.weapon.LoadedAmmo;
		}
	}

	public int MaxAmmo
	{
		get
		{
			if (this.weapon == null)
			{
				return 0;
			}
			return this.weapon.MaxLoadedAmmo;
		}
	}

	public int AmmoReserve
	{
		get
		{
			if (this.weapon == null)
			{
				return 0;
			}
			return this.weapon.AmmoReserve;
		}
	}

	public string GetWeaponString()
	{
		return this.weapon.GetTypeString();
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
		if (PlayerManager.Instance.LocalPlayer.IsZombie)
		{
			return;
		}
		if (this.weapons[weaponNum - 1] != null && this.weapons[weaponNum - 1] != this.weapon && (this.weapons[weaponNum - 1].AmmoReserve + this.weapons[weaponNum - 1].LoadedAmmo > 0 || weaponNum == 1 || this.weapons[weaponNum - 1].Type == WeaponType.BOMB_LAUNCHER))
		{
			if (this.weapon.Type == WeaponType.GATLING_GUN && this.weapon.Stop())
			{
				this.DoStop();
			}
			this.weapon.Change();
			this.NetworkManager.SendChange(weaponNum, (byte)this.weapons[weaponNum - 1].Type);
		}
		GameHUD.Instance.ShowWeapon();
	}

	public override WeaponType OnChangeWeapon(int weaponNum)
	{
		this.previousWeapon = this.weapon.GetIndex();
		this.weapon = this.weapons[weaponNum - 1];
		this.weapon.Change();
		base.StartCoroutine(this.Change(this.previousWeapon, weaponNum));
		if (this.weapon.LoadedAmmo == 0 && this.weapon.AmmoReserve > 0)
		{
			this.Reload(this.weapon);
		}
		this.SetCrossHair();
		return this.weapon.Type;
	}

	public void SetDiffuseShader(bool quality)
	{
		Shader shader2 = Shader.Find("Legacy Shaders/Diffuse");
		Shader shader = Shader.Find("Legacy Shaders/Diffuse");//Diffuse 2
	/*	if (quality)
		{
			shader = Shader.Find("Diffuse");//Diffuse
			shader2 = Shader.Find("Custom/HalfLambert");
		} */
		foreach (WeaponLook weaponLook in base.transform.GetComponentsInChildren<WeaponLook>(true))
		{
			foreach (Renderer renderer in weaponLook.transform.GetComponentsInChildren<Renderer>(true))
			{
				if ((renderer.material.name.StartsWith("hands_gloves") || renderer.material.name.StartsWith("hands_torso") || renderer.material.name.StartsWith("zombie_hands_Color")) && renderer.material.shader.name == shader.name)
				{
					//renderer.material.shader = shader; //2
					renderer.material.shader = Shader.Find ("Legacy Shaders/Diffuse");
				}
			}
		}
	}

	private void AnimateCrossHair(bool rotate)
	{
		if (this.CrossHair == null)
		{
			return;
		}
		Animation component = this.CrossHair.GetComponent<Animation>();
		if (component == null)
		{
			return;
		}
		if ((rotate && component["CrossHairRotationAnimation"].speed == 0f) || !component.IsPlaying("CrossHairRotationAnimation"))
		{
			component["CrossHairRotationAnimation"].speed = 0f;
			component.Blend("CrossHairRotationAnimation");
		}
		else if (!rotate && component["CrossHairRotationAnimation"].speed > 0f)
		{
			component["CrossHairRotationAnimation"].speed = 2f;
		}
		this.rotateCursor = rotate;
	}

	private void RotateCursor()
	{
		if (this.CrossHair == null)
		{
			return;
		}
		Animation component = this.CrossHair.GetComponent<Animation>();
		if (component == null)
		{
			return;
		}
		if (this.rotateCursor && component["CrossHairRotationAnimation"].speed < 2f)
		{
			component["CrossHairRotationAnimation"].speed += 0.025f;
		}
		else if (!this.rotateCursor && component["CrossHairRotationAnimation"].speed > 0f)
		{
			component["CrossHairRotationAnimation"].speed -= 0.04f;
			if (component["CrossHairRotationAnimation"].speed < 0f)
			{
				component["CrossHairRotationAnimation"].speed = 0f;
			}
		}
	}

	private void AnimateCrossHair()
	{
		if (this.CrossHair == null)
		{
			return;
		}
		Animation component = this.CrossHair.GetComponent<Animation>();
		if (component == null)
		{
			return;
		}
		component["CrossHairAnimation"].weight = 1f;
		component["CrossHairAnimation"].speed = 1f;
		component.Rewind("CrossHairAnimation");
		component.Blend("CrossHairAnimation");
	}

	private void SetCrossHairMode(CrossHairMode crossHairMode)
	{
		if (this.CrossHair == null)
		{
			return;
		}
		switch (crossHairMode)
		{
		case CrossHairMode.Ready:
			this.CrossHair.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
			break;
		case CrossHairMode.Blocked:
			this.CrossHair.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
			break;
		case CrossHairMode.ReadyToFire:
			this.CrossHair.GetComponent<Renderer>().material.color = new Color(1f, 0.25f, 0.25f, 1f);
			break;
		}
	}

	private void SetCrossHair()
	{
		if (base.transform.parent)
		{
			this.CrossHair = GameHUD.Instance.CrossHairs.GetChild(0);
			if (this.CrossHair == null)
			{
				return;
			}
			this.CrossHair.GetComponent<Renderer>().enabled = true;
			Texture texture = (Texture)Resources.Load("GUI/Battle/Crosshairs/crosshair_0" + this.weapon.GetType());
			int type = this.weapon.GetType();
			if (type != 6)
			{
				GameHUD.Instance.CrossHairs.localScale = new Vector3(0.65f, 0.65f, 0.65f);
			}
			else
			{
				GameHUD.Instance.CrossHairs.localScale = new Vector3(1f, 1f, 1f);
			}
			this.CrossHair.GetComponent<Renderer>().material.SetTexture("_MainTex", (!(texture != null)) ? ((Texture)Resources.Load("GUI/Battle/Crosshairs/crosshair_00")) : texture);
			Animation component = this.CrossHair.GetComponent<Animation>();
			if (component == null)
			{
				return;
			}
			component["CrossHairRotationAnimation"].speed = 0f;
			component["CrossHairRotationAnimation"].time = 0f;
			this.CrossHair.localEulerAngles = new Vector3(0f, 0f, 0f);
		}
	}

	public override void InitZombieWeapon(bool show)
	{
		if (!show)
		{
			Transform transform = null;
			foreach (WeaponLook weaponLook in base.transform.GetComponentsInChildren<WeaponLook>(true))
			{
				if (weaponLook.transform.name == "OneHandedColdArmsWeapon")
				{
					transform = weaponLook.transform;
					break;
				}
			}
			foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>(true))
			{
				renderer.enabled = false;
			}
		}
		else
		{
			base.StartCoroutine(this.ChangeZombieWeapon(new CombatWeapon(1, WeaponType.ONE_HANDED_COLD_ARMS, 700, 0, 0, 0, 0)
			{
				Distance = 8f,
				Angle = 2.05f,
				SystemName = "OHCA_Zombie"
			}));
		}
	}

	protected override IEnumerator ChangeZombieWeapon(CombatWeapon zombieWeapon)
	{
		this.Zoom = false;
		ActorAnimator animator = base.transform.GetComponent<ActorAnimator>();
		Transform weaponContainer = this.LocalPlayer.SoldierController.FPSCamera.transform.FindChild("Weapon");
		Animation weaponContainerAnimation = weaponContainer.GetComponent<Animation>();
		if (weaponContainerAnimation != null)
		{
			animator.ChangeWeaponAnimation(weaponContainerAnimation);
		}
		else
		{
			UnityEngine.Debug.Log("Weapon Container Animation is NULL");
		}
		Transform weaponTransform = this.weapon.Transform;
		if (weaponTransform != null)
		{
			animator.ResetReloadWeaponAnimation(weaponTransform);
		}
		yield return new WaitForSeconds(0.2f);
		this.HideWeapons();
		weaponTransform = this.weapon.Transform;
		foreach (Renderer g in weaponTransform.GetComponentsInChildren<Renderer>(true))
		{
			g.enabled = false;
		}
		this.InitWeapon(0, zombieWeapon);
		this.weapon = zombieWeapon;
		weaponTransform = this.weapon.Transform;
		if (PlayerManager.Instance.LocalPlayer.IsDead)
		{
			foreach (Renderer g2 in weaponTransform.GetComponentsInChildren<Renderer>(true))
			{
				g2.enabled = false;
			}
		}
		else
		{
			for (int i = 0; i < weaponTransform.GetChildCount(); i++)
			{
				Transform weaponChild = weaponTransform.GetChild(i);
				if (weaponChild.gameObject.name == this.weapon.SystemName)
				{
					base.setWeaponVisible(weaponChild, true);
				}
				else
				{
					base.setWeaponVisible(weaponChild, false);
					if (PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE)
					{
						if (NetworkDev.Destroy_Geometry)
						{
							UnityEngine.Object.Destroy(weaponChild.gameObject);
						}
					}
					else
					{
						weaponChild.gameObject.SetActive(false);
					}
				}
			}
		}
		yield break;
	}

	public override int InitWeapon(int index, Hashtable weaponData)
	{
		return this.InitWeapon(index, new CombatWeapon(index, weaponData));
	}

	public override int InitWeapon(int index, CombatWeapon new_weapon)
	{
		this.weapons[index] = new_weapon;
		foreach (WeaponLook weaponLook in base.transform.GetComponentsInChildren<WeaponLook>(true))
		{
			if (weaponLook.transform.name == this.weapons[index].GetName() || weaponLook.transform.name == this.weapons[index].GetName() + "L" || weaponLook.transform.name == this.weapons[index].GetName() + "R")
			{
				this.weapons[index].Transform = weaponLook.transform;
			}
		}
		if (this.weapons[index].Type == WeaponType.TWO_HANDED_COLD_ARMS)
		{
			this.weapons[index].SwitchWeaponTexture(0);
		}
		if (index == 0)
		{
			this.weapon = this.weapons[index];
			this.SetCrossHair();
			if (this.weapon.Transform != null)
			{
				for (int j = 0; j < this.weapon.Transform.GetChildCount(); j++)
				{
					Transform child = this.weapon.Transform.GetChild(j);
					if (child.gameObject.name == this.weapon.SystemName)
					{
						child.gameObject.SetActive(true);
						base.setWeaponVisible(child, true);
					}
					else
					{
						base.setWeaponVisible(child, false);
						if (PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE || (this.weapon.Type != WeaponType.ONE_HANDED_COLD_ARMS && this.weapon.Type != WeaponType.TWO_HANDED_COLD_ARMS))
						{
							if (NetworkDev.Destroy_Geometry)
							{
								UnityEngine.Object.Destroy(child.gameObject);
							}
						}
						else
						{
							child.gameObject.SetActive(false);
						}
					}
				}
			}
			CombatPlayer combatPlayer = this.LocalPlayer;
			ActorAnimator component = combatPlayer.transform.GetComponent<ActorAnimator>();
			component.ResetReloadWeaponAnimation(this.weapon.Transform);
		}
		else if (this.weapons[index].Transform != null)
		{
			for (int k = 0; k < this.weapons[index].Transform.GetChildCount(); k++)
			{
				Transform child2 = this.weapons[index].Transform.GetChild(k);
				if (child2.gameObject.name == this.weapons[index].SystemName)
				{
					child2.gameObject.SetActive(true);
					base.setWeaponVisible(child2, false);
				}
				else
				{
					base.setWeaponVisible(child2, false);
					if (PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE || (this.weapons[index].Type != WeaponType.ONE_HANDED_COLD_ARMS && this.weapons[index].Type != WeaponType.TWO_HANDED_COLD_ARMS))
					{
						if (NetworkDev.Destroy_Geometry)
						{
							UnityEngine.Object.Destroy(child2.gameObject);
						}
					}
					else
					{
						child2.gameObject.SetActive(false);
					}
				}
			}
		}
		this.shot = false;
		this.shotAlt = false;
		return this.weapons[index].GetType();
	}

	private void Awake()
	{
		ShotController.instance = this;
		this.weapons = new CombatWeapon[WeaponController.WeaponNum];
	}

	private void LateUpdate()
	{
		this.RotateCursor();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.F12))
		{
			foreach (ColorCorrectionCurves colorCorrectionCurves in base.transform.GetComponentsInChildren<ColorCorrectionCurves>())
			{
				colorCorrectionCurves.enabled = !colorCorrectionCurves.enabled;
			}
		}
		if (NetworkDev.CheckAim)
		{
			this.InitDummyPlayer();
		}
		if (UnityEngine.Input.GetKeyUp(TRInput.Fire1))
		{
			this.shot = false;
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse1))
		{
			this.shotAlt = false;
		}
		if (GameHUD.Instance == null)
		{
			return;
		}
		if (!GameHUD.Instance.isActive())
		{
			return;
		}
		if (this.isTaunt)
		{
			return;
		}
		this.weapon.CalcMode();
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon1))
		{
			this.SetWeapon(1);
			return;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon2))
		{
			this.SetWeapon(2);
			return;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon3))
		{
			this.SetWeapon(3);
			return;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon4))
		{
			this.SetWeapon(4);
			return;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon5))
		{
			this.SetWeapon(5);
			return;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon6))
		{
			this.SetWeapon(6);
			return;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Weapon7))
		{
			this.SetWeapon(7);
			return;
		}
		if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			this.SetPrevWeapon();
			return;
		}
		if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			this.SetNextWeapon();
			return;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.QuickChange))
		{
			this.QuickChange();
			return;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Reload))
		{
			this.Reload(this.weapon);
			return;
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Fire1))
		{
			this.shot = true;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1))
		{
			this.shotAlt = true;
		}
		if (KickManager.Instance.IsVoting())
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
			{
				KickManager.Instance.Vote(true);
				return;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.N))
			{
				KickManager.Instance.Vote(false);
				return;
			}
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Taunt1) && !this.shot)
		{
			this.LaunchTaunt(0, this.localPlayer.GetTauntID(0));
		}
		else if (UnityEngine.Input.GetKeyDown(TRInput.Taunt2) && !this.shot)
		{
			this.LaunchTaunt(1, this.localPlayer.GetTauntID(1));
		}
		else if (UnityEngine.Input.GetKeyDown(TRInput.Taunt3) && !this.shot)
		{
			this.LaunchTaunt(2, this.localPlayer.GetTauntID(2));
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Equals))
		{
			PlayerManager.Instance.TestEnemyAnimation();
		}
		if (this.shot)
		{
			if (this.weapon.IsWeaponBlocked())
			{
				this.EffectManager.weaponBlockEffect(this.localPlayer, this.weapon.Type);
				if (this.weapon.LoadedAmmo <= 0 && this.weapon.Type == WeaponType.GATLING_GUN && this.weapon.Stop())
				{
					this.DoStop();
				}
			}
			else if (this.weapon.LoadedAmmo > 0 || this.weapon.Type == WeaponType.ONE_HANDED_COLD_ARMS || this.weapon.Type == WeaponType.TWO_HANDED_COLD_ARMS)
			{
				if ((this.weapon.Type == WeaponType.GATLING_GUN || this.weapon.Type == WeaponType.FLAMER || this.weapon.Type == WeaponType.ACID_THROWER || this.weapon.Type == WeaponType.SNOW_GUN) && this.weapon.Launch())
				{
					this.DoLaunch();
				}
				else if (this.weapon.Fire())
				{
					this.DoShot();
				}
			}
		}
		else if ((this.weapon.Type == WeaponType.FLAMER || this.weapon.Type == WeaponType.ACID_THROWER || this.weapon.Type == WeaponType.SNOW_GUN) && this.weapon.Stop())
		{
			this.DoStop();
		}
		if (!this.shot && this.shotAlt && this.weapon.CanLaunch)
		{
			if (this.weapon.IsWeaponBlocked())
			{
				this.EffectManager.weaponBlockEffect(this.localPlayer, this.weapon.Type);
				if (this.weapon.LoadedAmmo <= 0 && this.weapon.Type == WeaponType.GATLING_GUN && this.weapon.Stop())
				{
					this.DoStop();
				}
			}
			else if ((this.weapon.LoadedAmmo > 0 || this.weapon.Type == WeaponType.ONE_HANDED_COLD_ARMS || this.weapon.Type == WeaponType.TWO_HANDED_COLD_ARMS) && this.weapon.Type == WeaponType.GATLING_GUN)
			{
				if (this.weapon.Launch())
				{
					this.DoLaunch();
				}
				else if (this.weapon.CanFire())
				{
					this.DoSpin();
				}
			}
		}
		if (this.shotAlt && this.weapon.Type == WeaponType.BOMB_LAUNCHER)
		{
			this.DoBlow();
		}
		if (!this.shot && (!this.shotAlt || !this.weapon.CanLaunch) && this.weapon.Type == WeaponType.GATLING_GUN && this.weapon.Stop())
		{
			this.DoStop();
		}
		if (UnityEngine.Input.GetKeyDown(TRInput.Zoom))
		{
			this.DoShot2x();
		}
		if (ShotController.lastCheckWeaponState + ShotController.CheckWeaponStateInterval < DateTime.Now.Ticks)
		{
			ShotController.lastCheckWeaponState = DateTime.Now.Ticks;
			this.CheckWeaponState();
		}
		if (this.dummyPlayer != null)
		{
			this.UpdateDummyPlayer();
		}
	}

	private void UpdateDummyPlayer()
	{
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		this.dummyPlayer.transform.position = ray.origin + ray.direction * 20f - new Vector3(0f, 6f, 0f);
	}

	private void InitDummyPlayer()
	{
		if (this.dummyPlayer != null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CharacterManager.Instance.GetPlayerEnemy());
		this.dummyPlayer = gameObject.AddComponent<CombatPlayer>();
		this.dummyPlayer.playerID = 66;
		this.dummyPlayer.Spawn(PlayerManager.Instance.transform, Vector3.zero, Vector3.zero, 2, 100, 100, true, ZombieType.Human);
		UnityEngine.Object.Destroy(this.dummyPlayer.PlayerCustomisator);
		UnityEngine.Object.Destroy(this.dummyPlayer.ActorAnimator);
		UnityEngine.Object.Destroy(this.dummyPlayer.GetComponent<Animation>());
		UnityEngine.Object.Destroy(this.dummyPlayer.Flame);
		UnityEngine.Object.Destroy(this.dummyPlayer.transform.FindChild("Bip01").gameObject);
		UnityEngine.Object.Destroy(this.dummyPlayer.transform.GetComponentInChildren<EnemyInfo>());
		foreach (Collider collider in this.dummyPlayer.transform.GetComponentsInChildren<Collider>())
		{
			collider.enabled = false;
		}
		Material material = new Material(Shader.Find("Transparent/Diffuse"));
		material.color = new Color(0f, 0f, 0f, 0f);
		foreach (Renderer renderer in this.dummyPlayer.transform.GetComponentsInChildren<Renderer>(true))
		{
			if (renderer.gameObject.name == "Heads_black01")
			{
				renderer.gameObject.SetActive(true);
			}
			else if (renderer.gameObject.name == "Pants_jeans01")
			{
				renderer.gameObject.SetActive(true);
			}
			else if (renderer.gameObject.name == "Shirts_shirt02")
			{
				renderer.gameObject.SetActive(true);
			}
			else if (renderer.gameObject.name == "Boots_sneak02")
			{
				renderer.gameObject.SetActive(true);
			}
			else if (renderer.gameObject.name == "Gloves_bint01")
			{
				renderer.gameObject.SetActive(true);
			}
			else
			{
				UnityEngine.Object.Destroy(renderer.gameObject);
			}
			renderer.material = material;
		}
	}

	private void LaunchTaunt(int tauntIndex, int tauntID)
	{
		this.isTaunt = true;
		this.shot = false;
		this.shotAlt = false;
		this.LocalPlayer.SoldierController.TPSCamera.transform.localPosition = new Vector3(0f, 0f, -10f);
		this.LocalPlayer.SoldierController.TPSCamera.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		GameHUD.Instance.Zoom(false);
		this.LocalPlayer.InitWear(true, PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE);
		this.LocalPlayer.InitTaunt(true, tauntID);
		this.LocalPlayer.ActorAnimator.ResetReloadWeaponAnimation(this.weapon.Transform);
		this.LocalPlayer.ActorAnimator.TauntAnimation(string.Format("Taunt{0}", tauntID));
		this.LocalPlayer.SoldierController.TPSCamera.GetComponent<TPSCamera>().OverLook = false;
		PlayerManager.Instance.CameraSwitch(this.LocalPlayer.SoldierController.FPSCamera, this.LocalPlayer.SoldierController.TPSCamera);
		base.StartCoroutine(this.FinishTaunt(tauntID));
		if (this.LocalPlayer.IsZombie)
		{
			this.EffectManager.PlayerSoundEffect(this.LocalPlayer.ActorAnimator.SpeakerAudio, "ZombieSound_3");
		}
		else
		{
			this.EffectManager.TauntEffect(this.LocalPlayer.ActorAnimator.SpeakerAudio, string.Format("Taunt{0}", tauntID));
		}
		this.NetworkManager.SendTaunt(tauntIndex, tauntID);
	}

	private IEnumerator FinishTaunt(int tauntID)
	{
		float seconds = 1.5f;
		if (tauntID == 9)
		{
			seconds = 3.5f;
		}
		else if (tauntID == 8)
		{
			seconds = 8.5f;
		}
		else if (tauntID == 7)
		{
			seconds = 3.5f;
		}
		else if (tauntID > 5)
		{
			seconds = 5.6f;
		}
		else if (tauntID > 1)
		{
			seconds = 2.8f;
		}
		yield return new WaitForSeconds(seconds);
		if (!this.localPlayer.IsDead)
		{
			PlayerManager.Instance.CameraSwitch(this.LocalPlayer.SoldierController.TPSCamera, this.LocalPlayer.SoldierController.FPSCamera);
		}
		this.Zoom = this.zoom;
		this.LocalPlayer.ActorAnimator.FinishTauntAnimation();
		this.LocalPlayer.SoldierController.TPSCamera.GetComponent<TPSCamera>().OverLook = true;
		this.LocalPlayer.InitWear(false, PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE);
		this.LocalPlayer.InitTaunt(false, tauntID);
		this.LocalPlayer.ActorAnimator.ResetReloadWeaponAnimation(this.weapon.Transform);
		this.isTaunt = false;
		yield break;
	}

	private void SetNextWeapon()
	{
		int num = base.CurrentWeapon.Index + 2;
		if (num > 7)
		{
			num = 1;
			this.SetWeapon(num);
			return;
		}
		while (this.weapons[num - 1].AmmoReserve + this.weapons[num - 1].LoadedAmmo == 0 && num != 1 && this.weapons[num - 1].Type != WeaponType.BOMB_LAUNCHER)
		{
			num++;
			if (num > 7)
			{
				num = 1;
				this.SetWeapon(num);
				return;
			}
		}
		this.SetWeapon(num);
	}

	private void SetPrevWeapon()
	{
		int num = base.CurrentWeapon.Index;
		if (num < 1)
		{
			num = 7;
		}
		while (this.weapons[num - 1].AmmoReserve + this.weapons[num - 1].LoadedAmmo == 0 && num != 1 && this.weapons[num - 1].Type != WeaponType.BOMB_LAUNCHER)
		{
			num--;
			if (num < 1)
			{
				num = 7;
			}
		}
		this.SetWeapon(num);
	}

	private void CheckWeaponState()
	{
		if (this.weapon == null)
		{
			return;
		}
		this.weaponBlocked = this.weapon.IsWeaponBlocked();
		if (this.weaponBlocked)
		{
			this.SetCrossHairMode(CrossHairMode.Blocked);
			return;
		}
		if (Camera.main == null)
		{
			return;
		}
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 20f));
		int num = ShotCalculator.StraightScan(ray, false);
		if (num == -1)
		{
			this.SetCrossHairMode(CrossHairMode.Ready);
			return;
		}
		this.SetCrossHairMode(CrossHairMode.ReadyToFire);
	}

	private void DoShot()
	{
		NetworkManager.Instance.RegisterShotBefore();
		System.Random random = new System.Random();
		Transform transform = this.weapon.Transform;
		if (transform != null)
		{
			transform = transform.GetChild(0);
			if (transform != null)
			{
				transform = transform.FindChild("Target");
			}
		}
		if (transform == null)
		{
			transform = base.transform;
		}
		WalkController component = base.transform.GetComponent<WalkController>();
		if (component)
		{
			float speed = component.Speed;
		}
		float num = 500f;
		float num2 = this.weapon.Deviation / num;
		if (this.deviationMultiplier != num)
		{
			PlayerManager.Instance.SendEnterBaseRequest(7);
			return;
		}
		if (this.weapon.Type == WeaponType.SNIPER_RIFLE && this.Zoom)
		{
			num2 = 0f;
		}
		if (num2 == 0f && !this.Zoom && (this.weapon.Type == WeaponType.GATLING_GUN || this.weapon.Type == WeaponType.HAND_GUN || this.weapon.Type == WeaponType.MACHINE_GUN))
		{
			PlayerManager.Instance.SendEnterBaseRequest(7);
			return;
		}
		float num3 = Convert.ToSingle(random.NextDouble() * (double)num2) - num2 / 2f;
		float num4 = Convert.ToSingle(random.NextDouble() * (double)num2) - num2 / 2f;
		if (Configuration.DebugEnableRTTX)
		{
			GameHUDFPS.Instance.SetDebugLine(string.Format("Deviation: {0} Radius: {1} rx: {2} ry: {3}", new object[]
			{
				this.weapon.Deviation,
				num2,
				num3,
				num4
			}), 2);
		}
		Ray cray = Camera.main.ViewportPointToRay(new Vector3(0.5f + num3, 0.5f + num4, 0f));
		CombatPlayer combatPlayer = this.LocalPlayer;
		ActorAnimator component2 = combatPlayer.transform.GetComponent<ActorAnimator>();
		if (Configuration.DebugVersion)
		{
			Configuration.DebugEnableFps = true;
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Vector3 vector = Camera.main.WorldToViewportPoint(cray.origin);
            vector.z = 0f;
            float num5 = (cray.origin - ray.origin).sqrMagnitude * 100000f;
			float num6 = Vector3.Angle(cray.direction, ray.direction);
			if (num5 > this.maxOriginDif)
			{
				this.maxOriginDif = num5;
			}
			if (num6 > this.maxDirAngleDif)
			{
				this.maxDirAngleDif = num6;
			}
			GameHUDFPS.Instance.SetDebugLine(string.Format("Version: {0}", 101, 1));
			GameHUDFPS.Instance.SetDebugLine(string.Format("Shot origin diff: {0} max: {1} rad: {2}", (cray.origin - ray.origin).sqrMagnitude * 100000f, this.maxOriginDif, num2 * num2 * 100000f), 2);
			GameHUDFPS.Instance.SetDebugLine(string.Format("Shot direction angle: {0} max: {1}", Vector3.Angle(cray.direction, ray.direction), this.maxDirAngleDif), 3);
		}
		NetworkManager.Instance.RegisterShotAfter();
		switch ((byte)this.weapon.GetType())
		{
		case 1:
		case 2:
		{
			Shot shot = new Shot(cray.origin + cray.direction * this.weapon.Distance, cray.direction, (byte)this.weapon.GetType());
			shot.Launch(cray.origin + cray.direction * this.weapon.Distance, cray.direction, shot.TimeStamp, shot.TimeStamp + 200L);
			this.NetworkManager.SendShot(shot);
			base.StartCoroutine(this.DelayedShot(transform, this.weapon));
			if (component2 != null && shot.LaunchMode == LaunchModes.LAUNCH)
			{
				component2.ShotWeaponAnimation("Shot", this.weapon.Transform);
			}
			this.AnimateCrossHair();
			break;
		}
		case 3:
		{
			Shot shot = ShotCalculator.StraightShot(cray, transform, this.weapon.GetType(), 0, false);
			if (shot == null)
			{
				return;
			}
			ShotCalculator.GunShot(this.LocalPlayer, shot);
			this.NetworkManager.SendShot(shot, cray.origin);
			this.EffectManager.machineGunEffect(shot, this.LocalPlayer, true);
			if (component2 != null)
			{
				foreach (WeaponLook weaponLook in combatPlayer.transform.GetComponentsInChildren<WeaponLook>(true))
				{
					if (weaponLook.gameObject.name == "HandGunWeapon")
					{
						component2.ShotWeaponAnimation("Shot", weaponLook.transform);
					}
				}
			}
			if (this.weapon.Attributes.Contains("S") || this.weapon.IsShaking)
			{
				FPSCamera componentInChildren = base.GetComponentInChildren<FPSCamera>();
				int num7 = this.random.Next(-1, 2);
				if (num7 == 0)
				{
					componentInChildren.Shake(new Vector3(-1f, 0f, 0f));
				}
				else
				{
					componentInChildren.Shake(new Vector3(0f, (float)num7, 0f));
				}
			}
			this.AnimateCrossHair();
			break;
		}
		case 4:
		{
			Shot shot = ShotCalculator.StraightShot(cray, transform, this.weapon.GetType(), 0, false);
			if (shot == null)
			{
				return;
			}
			ShotCalculator.GunShot(this.LocalPlayer, shot);
			this.NetworkManager.SendShot(shot, cray.origin);
			this.EffectManager.machineGunEffect(shot, this.LocalPlayer, true);
			if (component2 != null)
			{
				foreach (WeaponLook weaponLook2 in combatPlayer.transform.GetComponentsInChildren<WeaponLook>(true))
				{
					if (weaponLook2.gameObject.name == "MachineGunWeapon")
					{
						component2.ShotWeaponAnimation("Shot", weaponLook2.transform);
					}
				}
			}
			this.AnimateCrossHair();
			break;
		}
		case 5:
		case 11:
		case 12:
			if (!PlayerManager.Instance.WaterBlock)
			{
				Shot shot = ShotCalculator.StraightShot(cray, transform, this.weapon.GetType(), 0, false);
				if (shot == null)
				{
					return;
				}
				ShotCalculator.SegmentShot(this.LocalPlayer, shot);
				this.NetworkManager.SendShot(shot, cray.origin);
				this.EffectManager.flamerEffect(shot, this.LocalPlayer, true);
				this.AnimateCrossHair();
			}
			break;
		case 6:
		{
			Shot shot = ShotCalculator.StraightShot(cray, transform, this.weapon.GetType(), 0, false);
			if (shot == null)
			{
				return;
			}
			ShotCalculator.GunShot(this.LocalPlayer, shot);
			this.NetworkManager.SendShot(shot, cray.origin);
			this.EffectManager.machineGunEffect(shot, this.LocalPlayer, true);
			component2 = base.transform.GetComponent<ActorAnimator>();
			if (component2 != null)
			{
				Transform transform2 = this.weapon.Transform;
				if (this.weapon.Type == WeaponType.GATLING_GUN && transform2 != null)
				{
					component2.ShotWeaponAnimation("Shot", transform2);
				}
			}
			this.AnimateCrossHair();
			break;
		}
		case 7:
		{
			Shot shot = ShotCalculator.StraightShot(cray, transform, this.weapon.GetType(), 0, false);
			ShotCalculator.SegmentShot(this.LocalPlayer, shot);
			this.NetworkManager.SendShot(shot, cray.origin);
			this.EffectManager.shotGunEffect(shot, this.LocalPlayer, true);
			if (component2 != null)
			{
				foreach (WeaponLook weaponLook3 in combatPlayer.transform.GetComponentsInChildren<WeaponLook>(true))
				{
					if (weaponLook3.gameObject.name == "ShotGunWeapon")
					{
						component2.ShotWeaponAnimation("Shot", weaponLook3.transform);
					}
				}
			}
			this.AnimateCrossHair();
			CharacterMotor componentInChildren2 = combatPlayer.GetComponentInChildren<CharacterMotor>();
			if (!componentInChildren2.grounded && !this.weapon.IsWeaponBlockedChange())
			{
				float num8 = (float)this.LocalPlayer.Jump;
				if (this.weapon.SystemName.Contains("SG_Novapump"))
				{
					num8 *= 0.7f;
				}
				componentInChildren2.SetExplosionForce(shot.Origin, num8);
			}
			FPSCamera componentInChildren = base.GetComponentInChildren<FPSCamera>();
			componentInChildren.Shake(new Vector3(-2f, 0f, 0f));
			break;
		}
		case 8:
		{
			Shot shot = ShotCalculator.RocketShot(cray, transform, this.weapon.GetType(), (float)this.weapon.Velocity);
			if (shot == null)
			{
				return;
			}
			this.NetworkManager.SendShot(shot);
			this.EffectManager.rocketLauncherEffect(shot, this.LocalPlayer, true);
			this.AnimateCrossHair();
			break;
		}
		case 9:
		{
			cray = Camera.main.ViewportPointToRay(new Vector3(0.5f + num3, 0.65f + num4, 20f));
			Shot shot = ShotCalculator.GrenadeShot(cray, transform, this.weapon.GetType(), (float)this.weapon.Velocity, (float)this.weapon.Life, false);
			if (shot == null)
			{
				return;
			}
			this.NetworkManager.SendShot(shot);
			this.EffectManager.grenadeLauncherEffect(shot, this.LocalPlayer, true);
			this.AnimateCrossHair();
			break;
		}
		case 10:
		{
			Shot shot;
			if (this.weapon.SystemName.StartsWith("SR_Wildcat"))
			{
				shot = ShotCalculator.CrossbowShot(cray, transform, this.weapon.GetType(), 0, false);
				if (shot == null)
				{
					return;
				}
			}
			else
			{
				shot = ShotCalculator.StraightShot(cray, transform, this.weapon.GetType(), 0, false);
				if (shot == null)
				{
					return;
				}
				ShotCalculator.GunShot(this.LocalPlayer, shot);
			}
			this.NetworkManager.SendShot(shot, cray.origin);
			this.EffectManager.machineGunEffect(shot, this.LocalPlayer, true);
			if (component2 != null)
			{
				foreach (WeaponLook weaponLook4 in combatPlayer.transform.GetComponentsInChildren<WeaponLook>(true))
				{
					if (weaponLook4.gameObject.name == "SniperRifleWeapon")
					{
						component2.ShotWeaponAnimation("Shot", weaponLook4.transform);
					}
				}
			}
			this.AnimateCrossHair();
			FPSCamera componentInChildren = base.GetComponentInChildren<FPSCamera>();
			componentInChildren = base.GetComponentInChildren<FPSCamera>();
			int num9 = this.random.Next(-3, 0);
			int num10 = this.random.Next(-2, 0);
			componentInChildren.Shake(new Vector3((float)num9, (float)num10, 0f), true);
			break;
		}
		case 15:
		{
			this.CheckBlow();
			cray = Camera.main.ViewportPointToRay(new Vector3(0.5f + num3, 0.65f + num4, 20f));
			Shot shot = ShotCalculator.GrenadeShot(cray, transform, this.weapon.GetType(), (float)this.weapon.Velocity, (float)this.weapon.Life, true);
			this.NetworkManager.SendShot(shot);
			this.EffectManager.bombLauncherEffect(shot, this.weapon, this.LocalPlayer, true, false);
			this.AnimateCrossHair();
			break;
		}
		}
	}

	private IEnumerator DelayedShot(Transform TWeapon, CombatWeapon weapon)
	{
		yield return new WaitForSeconds(0.2f);
		NetworkManager.Instance.RegisterShotBefore();
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 20f));
		this.AnimateCrossHair();
		NetworkManager.Instance.RegisterShotAfter();
		WeaponType weaponType = (WeaponType)weapon.GetType();
		if (weaponType == WeaponType.ONE_HANDED_COLD_ARMS || weaponType == WeaponType.TWO_HANDED_COLD_ARMS)
		{
			Shot shot = ShotCalculator.StraightShot(ray, Camera.main.transform, weapon.GetType(), 0, false);
			shot.Targets.Clear();
			ShotCalculator.SegmentShot(this.LocalPlayer, shot);
			this.NetworkManager.SendShot(shot);
		}
		yield break;
	}

	private void DoLaunch()
	{
		if (this.weapon.Type == WeaponType.GATLING_GUN)
		{
			Shot shot = new Shot(base.transform.position, new Vector3(0f, 0f, 0f), (byte)this.weapon.Type);
			shot.LaunchMode = LaunchModes.LAUNCH;
			this.AnimateCrossHair(true);
			this.NetworkManager.SendShot(shot);
		}
		ActorAnimator component = base.transform.GetComponent<ActorAnimator>();
		if (component != null)
		{
			Transform transform = this.weapon.Transform;
			if (transform != null)
			{
				if (this.weapon.Type == WeaponType.GATLING_GUN)
				{
					component.ShotWeaponAnimation("Load", transform);
					this.EffectManager.gatlingGunEffect(LaunchModes.LAUNCH, this.LocalPlayer, true);
				}
				else if (this.weapon.Type == WeaponType.FLAMER || this.weapon.Type == WeaponType.ACID_THROWER || this.weapon.Type == WeaponType.SNOW_GUN)
				{
					component.ShotWeaponAnimation("Load", transform);
					SoundManager.Instance.Play(this.LocalPlayer.Audio, "Flamer_Launch", AudioPlayMode.Play);
				}
			}
		}
	}

	private void DoBlow()
	{
		if (this.weapon.Type == WeaponType.BOMB_LAUNCHER)
		{
			List<BombTracer> list = null;
			foreach (ItemTracer itemTracer in this.LocalPlayer.RegisteredItems.Values)
			{
				if (itemTracer.GetType() == typeof(BombTracer))
				{
					if (list == null)
					{
						list = new List<BombTracer>();
					}
					list.Add((BombTracer)itemTracer);
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (BombTracer bombTracer in list)
			{
				bombTracer.InitBlow();
			}
		}
	}

	private void CheckBlow()
	{
		if (this.weapon.Type == WeaponType.BOMB_LAUNCHER)
		{
			List<BombTracer> list = null;
			BombTracer bombTracer = null;
			foreach (ItemTracer itemTracer in this.LocalPlayer.RegisteredItems.Values)
			{
				if (itemTracer.GetType() == typeof(BombTracer))
				{
					if (list == null)
					{
						list = new List<BombTracer>();
					}
					list.Add((BombTracer)itemTracer);
					if (bombTracer == null || bombTracer.TimeStamp > itemTracer.TimeStamp)
					{
						bombTracer = (BombTracer)itemTracer;
					}
				}
			}
			if (list == null || list.Count < 3)
			{
				return;
			}
			bombTracer.InitBlow();
		}
	}

	private void DoSpin()
	{
		if (this.weapon.Type == WeaponType.GATLING_GUN)
		{
			Shot shot = new Shot(base.transform.position, new Vector3(0f, 0f, 0f), (byte)this.weapon.Type);
			shot.LaunchMode = LaunchModes.LAUNCH;
			this.AnimateCrossHair(true);
		}
		ActorAnimator component = base.transform.GetComponent<ActorAnimator>();
		if (component != null)
		{
			Transform transform = this.weapon.Transform;
			if (transform != null && this.weapon.Type == WeaponType.GATLING_GUN)
			{
				component.ShotWeaponAnimation("Spin", transform);
				this.EffectManager.gatlingGunEffect(LaunchModes.SPIN, this.LocalPlayer, true);
			}
		}
	}

	private void DoStop()
	{
		if (this.weapon == null)
		{
			return;
		}
		if (this.weapon.Type == WeaponType.GATLING_GUN)
		{
			Shot shot = new Shot(base.transform.position, new Vector3(0f, 0f, 0f), (byte)this.weapon.Type);
			shot.LaunchMode = LaunchModes.BLOW;
			this.AnimateCrossHair(false);
			this.NetworkManager.SendShot(shot);
		}
		ActorAnimator component = base.transform.GetComponent<ActorAnimator>();
		if (component != null)
		{
			Transform transform = this.weapon.Transform;
			if (transform != null)
			{
				if (this.weapon.Type == WeaponType.GATLING_GUN)
				{
					component.ShotWeaponAnimation("Stop", transform);
					this.EffectManager.gatlingGunEffect(LaunchModes.BLOW, this.LocalPlayer, true);
				}
				else if (this.weapon.Type == WeaponType.FLAMER || this.weapon.Type == WeaponType.ACID_THROWER || this.weapon.Type == WeaponType.SNOW_GUN)
				{
					component.ShotWeaponAnimation("Stop", transform);
				}
			}
		}
	}

	private void DoShot2x()
	{
	}

	private new void Reload(CombatWeapon reloadWeapon)
	{
		if (reloadWeapon.Type == WeaponType.ONE_HANDED_COLD_ARMS || reloadWeapon.Type == WeaponType.TWO_HANDED_COLD_ARMS)
		{
			return;
		}
		if (!reloadWeapon.CanReload())
		{
			return;
		}
		this.NetworkManager.SendReload(reloadWeapon.Type);
		int num = reloadWeapon.Reload();
		WeaponLook[] componentsInChildren = base.transform.GetComponentsInChildren<WeaponLook>(true);
		if (componentsInChildren.Length > 0)
		{
			Transform parent = componentsInChildren[0].transform.parent;
			Animation component = parent.GetComponent<Animation>();
			if (component != null)
			{
				ActorAnimator component2 = base.transform.GetComponent<ActorAnimator>();
				component2.ReloadWeaponAnimation(reloadWeapon.Transform, reloadWeapon.ComplexReload, num);
			}
		}
		EffectManager.Instance.reloadEffect(reloadWeapon.Type, this.LocalPlayer, reloadWeapon.SystemName, num);
	}

	private void QuickChange()
	{
		if (this.previousWeapon != this.weapon.GetIndex())
		{
			int index = this.weapon.GetIndex();
			this.SetWeapon(this.previousWeapon);
			this.previousWeapon = index;
		}
	}

	private new IEnumerator Change(int previousWeaponNum, int weaponNum)
	{
		this.Zoom = false;
		ActorAnimator animator = base.transform.GetComponent<ActorAnimator>();
		Transform weaponContainer = this.LocalPlayer.SoldierController.FPSCamera.transform.FindChild("Weapon");
		Animation weaponContainerAnimation = weaponContainer.GetComponent<Animation>();
		if (weaponContainerAnimation != null)
		{
			animator.ChangeWeaponAnimation(weaponContainerAnimation);
		}
		else
		{
			UnityEngine.Debug.Log("Weapon Container Animation is NULL");
		}
		Transform weaponTransform = this.weapons[weaponNum - 1].Transform;
		if (weaponTransform != null)
		{
			animator.ResetReloadWeaponAnimation(weaponTransform);
		}
		yield return new WaitForSeconds(0.2f);
		weaponTransform = this.weapons[previousWeaponNum - 1].Transform;
		foreach (Renderer g in weaponTransform.GetComponentsInChildren<Renderer>(true))
		{
			g.enabled = false;
		}
		weaponTransform = this.weapons[weaponNum - 1].Transform;
		if (this.weapons[weaponNum - 1].Type == WeaponType.TWO_HANDED_COLD_ARMS)
		{
			this.weapons[weaponNum - 1].SwitchWeaponTexture(0);
		}
		for (int i = 0; i < weaponTransform.GetChildCount(); i++)
		{
			Transform weaponChild = weaponTransform.GetChild(i);
			if (weaponChild.gameObject.name == this.weapons[weaponNum - 1].SystemName)
			{
				base.setWeaponVisible(weaponChild, true);
			}
			else
			{
				base.setWeaponVisible(weaponChild, false);
				if (PlayerManager.Instance.RoomSettings.GameMode != MapMode.MODE.ZOMBIE)
				{
					if (NetworkDev.Destroy_Geometry)
					{
						UnityEngine.Object.Destroy(weaponChild.gameObject);
					}
				}
				else
				{
					weaponChild.gameObject.SetActive(false);
				}
			}
		}
		yield break;
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

	public void AddAmmoToReserve(int weaponType, int amount, bool allWeapons)
	{
		foreach (CombatWeapon combatWeapon in this.weapons)
		{
			if (combatWeapon != null)
			{
				combatWeapon.AddAmmoToReserve(combatWeapon.MaxAmmoReserve * amount / 100, allWeapons);
				if (combatWeapon == this.weapon && combatWeapon.LoadedAmmo == 0 && combatWeapon.AmmoReserve > 0)
				{
					this.Reload(combatWeapon);
				}
			}
		}
	}

	public void AddAmmoToReserve(int weaponType, int amount)
	{
		foreach (CombatWeapon combatWeapon in this.weapons)
		{
			if (combatWeapon != null && (combatWeapon.GetType() == weaponType || (combatWeapon.GetType() == 6 && weaponType == 4)))
			{
				combatWeapon.AddAmmoToReserve(amount);
				if (combatWeapon == this.weapon && combatWeapon.LoadedAmmo == 0 && combatWeapon.AmmoReserve > 0)
				{
					this.Reload(combatWeapon);
				}
			}
		}
	}

	public void SwitchWeaponTexture(WeaponType weaponType, int textureID)
	{
		if (weaponType == WeaponType.TWO_HANDED_COLD_ARMS)
		{
			CombatWeapon weaponByType = base.GetWeaponByType((int)weaponType);
			if (weaponByType == null)
			{
				return;
			}
			weaponByType.SwitchWeaponTexture(textureID);
		}
	}

	public void OnShot(WeaponType weaponType)
	{
		if (weaponType == WeaponType.ONE_HANDED_COLD_ARMS || weaponType == WeaponType.TWO_HANDED_COLD_ARMS)
		{
			return;
		}
		foreach (CombatWeapon combatWeapon in this.weapons)
		{
			if (combatWeapon != null && combatWeapon.Type == weaponType)
			{
				combatWeapon.ShootAmmo();
				if (combatWeapon.LoadedAmmo == 0)
				{
					if (combatWeapon == this.weapon && combatWeapon.AmmoReserve > 0)
					{
						this.Reload(combatWeapon);
					}
					else if (weaponType != WeaponType.BOMB_LAUNCHER)
					{
						this.SetNextWeapon();
					}
				}
			}
		}
	}

	public void OnReload(int index, int loadedAmmo, int ammo)
	{
		CombatWeapon combatWeapon = this.weapons[index];
		if (combatWeapon != null)
		{
			combatWeapon.UpdateAmmoCount(loadedAmmo, ammo);
			if (combatWeapon == this.weapon && combatWeapon.LoadedAmmo == 0 && combatWeapon.AmmoReserve > 0)
			{
				this.Reload(combatWeapon);
			}
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
		return this.weapon != null && (this.weapon.Type == WeaponType.SNIPER_RIFLE || this.weapon.Attributes.Contains("O"));
	}

	public float GetSniperZoomFactor()
	{
		if (this.weapon.Type == WeaponType.SNIPER_RIFLE)
		{
			if (this.weapon.Attributes.Contains("D"))
			{
				return 0.15f;
			}
			return 0.2f;
		}
		else
		{
			if (this.weapon.Attributes.Contains("O"))
			{
				return 0.5f;
			}
			return 1f;
		}
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
		return string.Empty;
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
		return 9;
	}

	public override void ResetWeapon()
	{
		if (this.weapon != null)
		{
			if (this.weapon.Type == WeaponType.GATLING_GUN)
			{
				this.EffectManager.gatlingGunEffect(LaunchModes.BLOW, this.LocalPlayer, true);
			}
			if (this.weapon.Type == WeaponType.FLAMER || this.weapon.Type == WeaponType.ACID_THROWER || this.weapon.Type == WeaponType.SNOW_GUN)
			{
				this.EffectManager.flamerEffect(LaunchModes.BLOW, this.LocalPlayer, true);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("[ShotController.cs] Weapon in null!");
		}
		base.ResetWeapon();
		this.DoStop();
		Animation component = this.CrossHair.GetComponent<Animation>();
		if (component == null)
		{
			return;
		}
		component["CrossHairRotationAnimation"].speed = 0f;
		component["CrossHairRotationAnimation"].time = 0f;
		this.CrossHair.localEulerAngles = new Vector3(0f, 0f, 0f);
	}

	public float GetSpeedMultiplier()
	{
		float result = 1f;
		if (this.weapon == null)
		{
			return result;
		}
		result = this.weapon.SpeedValue;
		if (this.weapon.Type == WeaponType.ONE_HANDED_COLD_ARMS)
		{
			result = this.weapon.SpeedValue * 1.25f;
		}
		if (this.weapon.Attributes.Contains("R"))
		{
			result = this.weapon.SpeedValue * 1.25f;
		}
		return result;
	}

	public bool CheckSpeedMultiplier(float checkSpeedMultiplier)
	{
		float num = 1.25f;
		if (this.weapon == null)
		{
			return true;
		}
		float num2 = this.weapon.SpeedValue;
		if (this.weapon.Type == WeaponType.ONE_HANDED_COLD_ARMS)
		{
			num2 = this.weapon.SpeedValue * num;
		}
		if (this.weapon.Attributes.Contains("R"))
		{
			num2 = this.weapon.SpeedValue * num;
		}
		return num2 == checkSpeedMultiplier;
	}

	private float deviationMultiplier = 500f;

	private static readonly long CheckWeaponStateInterval = 500000L;

	private static long lastCheckWeaponState = DateTime.Now.Ticks;

	private System.Random random = new System.Random();

	private CombatPlayer dummyPlayer;

	private static ShotController instance;

	private RaycastHit currentHit;

	private bool rotateCursor;

	private bool isTaunt;

	private bool zoom;

	private bool shot;

	private bool shotAlt;

	private bool weaponBlocked;

	private int previousWeapon = 1;

	private CombatPlayer localPlayer;

	private EffectManager effectManager;

	private NetworkManager networkManager;

	private Transform CrossHair;

	private GameObject montageObject;

	private float maxOriginDif;

	private float maxDirAngleDif;
}
