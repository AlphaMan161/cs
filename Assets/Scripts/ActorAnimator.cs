// dnSpy decompiler from Assembly-CSharp.dll class: ActorAnimator
using System;
using UnityEngine;

public class ActorAnimator : MonoBehaviour
{
	public bool IsRunning
	{
		get
		{
			return this.keyState == KeyState.Runing || this.keyState == KeyState.RuningBack || this.keyState == KeyState.RunStrafeLeft || this.keyState == KeyState.RunStrafeRight;
		}
	}

	public bool IsWalking
	{
		get
		{
			return this.keyState == KeyState.Walking || this.keyState == KeyState.WalkBack || this.keyState == KeyState.WalkStrafeLeft || this.keyState == KeyState.WalkStrafeRight;
		}
	}

	public string IdleAnimationName
	{
		get
		{
			return this.idleAnimationName;
		}
		set
		{
			if (!(PlayerManager.Instance != null) || PlayerManager.Instance.LocalPlayer.ActorAnimator != this)
			{
			}
			this.idleAnimationName = value;
		}
	}

	public string BendAnimationName
	{
		get
		{
			return this.bendAnimationName;
		}
		set
		{
			if (PlayerManager.Instance.LocalPlayer.ActorAnimator != this)
			{
			}
			this.bendAnimationName = value;
		}
	}

	public AudioSource LegsAudio
	{
		get
		{
			if (this.legsAudio == null)
			{
				foreach (AudioSource audioSource in base.transform.GetComponentsInChildren<AudioSource>())
				{
					if (audioSource.gameObject.name == "Legs")
					{
						this.legsAudio = audioSource;
					}
				}
			}
			return this.legsAudio;
		}
	}

	public AudioSource SpeakerAudio
	{
		get
		{
			if (this.speakerAudio == null)
			{
				foreach (AudioSource audioSource in base.transform.GetComponentsInChildren<AudioSource>())
				{
					if (audioSource.gameObject.name == "Speaker")
					{
						this.speakerAudio = audioSource;
					}
				}
			}
			return this.speakerAudio;
		}
	}

	public void Refresh()
	{
		this.shopAnimator = false;
	}

	public void SetZombieAnimation(bool isZombie)
	{
		this.isZombie = isZombie;
	}

	public void PlayAnimation(string name, bool loop)
	{
		this.shopAnimator = true;
		if (loop)
		{
			base.GetComponent<Animation>()[name].wrapMode = WrapMode.Loop;
		}
		else
		{
			base.GetComponent<Animation>()[name].wrapMode = WrapMode.Once;
		}
		base.GetComponent<Animation>().Play(name, PlayMode.StopAll);
	}

	private void Start()
	{
		this.SetAnimationProperties();
	}

	private void SetAnimationProperties()
	{
		if (!this.setFlag)
		{
			this.setFlag = true;
			base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>()[this.JumpAnimationName].weight = 0f;
			base.GetComponent<Animation>()[this.JumpAnimationName].speed = 0f;
			base.GetComponent<Animation>()[this.JumpAnimationName].enabled = true;
			base.GetComponent<Animation>()[this.JumpAnimationName].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Bend"].clip, "ShotgunBendClip");
			this.SetupAdditiveAiming("ShotgunBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Bat_Bend"].clip, "BatBendClip");
			this.SetupHeadAiming("BatBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Katana_Bend"].clip, "KatanaBendClip");
			this.SetupAdditiveAiming("KatanaBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Makarov_Bend"].clip, "MakarovBendClip");
			this.SetupAdditiveAiming("MakarovBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["AK47_Bend"].clip, "AK47BendClip");
			this.SetupAdditiveAiming("AK47BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["UMP45_Bend"].clip, "UMP45BendClip");
			this.SetupAdditiveAiming("UMP45BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["SteyrAUG_Bend"].clip, "SteyrAUGBendClip");
			this.SetupAdditiveAiming("SteyrAUGBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Wildcat_Bend"].clip, "WildcatBendClip");
			this.SetupAdditiveAiming("WildcatBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["TMP_Bend"].clip, "TMPBendClip");
			this.SetupAdditiveAiming("TMPBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["M134_Bend"].clip, "M134BendClip");
			this.SetupAdditiveAiming("M134BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Winchester1887_Bend"].clip, "Winchester1887BendClip");
			this.SetupAdditiveAiming("Winchester1887BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Novapump_Bend"].clip, "NovapumpBendClip");
			this.SetupAdditiveAiming("NovapumpBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["RPG26_Bend"].clip, "RPG26BendClip");
			this.SetupAdditiveAiming("RPG26BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["RPG7_Bend"].clip, "RPG7BendClip");
			this.SetupAdditiveAiming("RPG7BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["M202A1_Bend"].clip, "M202A1BendClip");
			this.SetupAdditiveAiming("M202A1BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Milkor_Bend"].clip, "MilkorBendClip");
			this.SetupAdditiveAiming("MilkorBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["SnowLauncher_Bend"].clip, "SnowLauncherBendClip");
			this.SetupAdditiveAiming("SnowLauncherBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["SVD_Bend"].clip, "SVDBendClip");
			this.SetupAdditiveAiming("SVDBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Spas_Bend"].clip, "SpasBendClip");
			this.SetupAdditiveAiming("SpasBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["FNMAG_Bend"].clip, "FNMAGBendClip");
			this.SetupAdditiveAiming("FNMAGBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Vintorez_Bend"].clip, "VintorezBendClip");
			this.SetupAdditiveAiming("VintorezBendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["M249_Bend"].clip, "M249BendClip");
			this.SetupAdditiveAiming("M249BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["AssaultRifle02_Bend"].clip, "AssaultRifle02BendClip");
			this.SetupAdditiveAiming("AssaultRifle02BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["AssaultRifle03_Bend"].clip, "AssaultRifle03BendClip");
			this.SetupAdditiveAiming("AssaultRifle03BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["SniperRifle03_Bend"].clip, "SniperRifle03BendClip");
			this.SetupAdditiveAiming("SniperRifle03BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Shotgun02_Bend"].clip, "Shotgun02BendClip");
			this.SetupAdditiveAiming("Shotgun02BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["GrenadeLauncher03_Bend"].clip, "GrenadeLauncher03BendClip");
			this.SetupAdditiveAiming("GrenadeLauncher03BendClip", true);
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["ShotgunChange"].clip, "Change");
			this.SetupAdditiveAiming("Change", true);
			base.GetComponent<Animation>()["Change"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["Change"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["ShotgunReload"].clip, "ShotgunReloadClip");
			this.SetupAdditiveAiming("ShotgunReloadClip", true);
			base.GetComponent<Animation>()["ShotgunReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["ShotgunReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["AK47_Reload"].clip, "MachineGunWeaponReloadClip");
			this.SetupAdditiveAiming("MachineGunWeaponReloadClip", true);
			base.GetComponent<Animation>()["MachineGunWeaponReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["MachineGunWeaponReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["UMP45_Reload"].clip, "UMP45ReloadClip");
			this.SetupAdditiveAiming("UMP45ReloadClip", true);
			base.GetComponent<Animation>()["UMP45ReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["UMP45ReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["SteyrAUG_Reload"].clip, "SteyrAUGReloadClip");
			this.SetupAdditiveAiming("SteyrAUGReloadClip", true);
			base.GetComponent<Animation>()["SteyrAUGReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["SteyrAUGReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["FNMAG_Reload"].clip, "FNMAGReloadClip");
			this.SetupAdditiveAiming("FNMAGReloadClip", true);
			base.GetComponent<Animation>()["FNMAGReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["FNMAGReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Wildcat_Reload"].clip, "WildcatReloadClip");
			this.SetupAdditiveAiming("WildcatReloadClip", true);
			base.GetComponent<Animation>()["WildcatReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["WildcatReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["TMP_Reload"].clip, "TMPReloadClip");
			this.SetupAdditiveAiming("TMPReloadClip", true);
			base.GetComponent<Animation>()["TMPReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["TMPReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Makarov_Reload"].clip, "HandGunWeaponReloadClip");
			this.SetupAdditiveAiming("HandGunWeaponReloadClip", true);
			base.GetComponent<Animation>()["HandGunWeaponReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["HandGunWeaponReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["RPG26_Reload"].clip, "RocketLauncherWeaponReloadClip");
			this.SetupAdditiveAiming("RocketLauncherWeaponReloadClip", true);
			base.GetComponent<Animation>()["RocketLauncherWeaponReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["RocketLauncherWeaponReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["M202A1_Reload"].clip, "M202A1ReloadClip");
			this.SetupAdditiveAiming("M202A1ReloadClip", true);
			base.GetComponent<Animation>()["M202A1ReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["M202A1ReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Milkor_Reload"].clip, "GrenadeLauncherWeaponReloadClip");
			this.SetupAdditiveAiming("GrenadeLauncherWeaponReloadClip", true);
			base.GetComponent<Animation>()["GrenadeLauncherWeaponReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["GrenadeLauncherWeaponReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["SnowLauncher_Reload"].clip, "SnowLauncherWeaponReloadClip");
			this.SetupAdditiveAiming("SnowLauncherWeaponReloadClip", true);
			base.GetComponent<Animation>()["SnowLauncherWeaponReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["SnowLauncherWeaponReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Winchester1887_Reload"].clip, "ShotGunWeaponReloadClip");
			this.SetupAdditiveAiming("ShotGunWeaponReloadClip", true);
			base.GetComponent<Animation>()["ShotGunWeaponReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["ShotGunWeaponReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["AssaultRifle02_Reload"].clip, "AssaultRifle02ReloadClip");
			this.SetupAdditiveAiming("AssaultRifle02ReloadClip", true);
			base.GetComponent<Animation>()["AssaultRifle02ReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["AssaultRifle02ReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["AssaultRifle03_Reload"].clip, "AssaultRifle03ReloadClip");
			this.SetupAdditiveAiming("AssaultRifle03ReloadClip", true);
			base.GetComponent<Animation>()["AssaultRifle03ReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["AssaultRifle03ReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["SniperRifle03_Reload"].clip, "SniperRifle03ReloadClip");
			this.SetupAdditiveAiming("SniperRifle03ReloadClip", true);
			base.GetComponent<Animation>()["SniperRifle03ReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["SniperRifle03ReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Shotgun02_Reload"].clip, "Shotgun02ReloadClip");
			this.SetupAdditiveAiming("Shotgun02ReloadClip", true);
			base.GetComponent<Animation>()["Shotgun02ReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["Shotgun02ReloadClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["GrenadeLauncher03_Reload"].clip, "GrenadeLauncher03ReloadClip");
			this.SetupAdditiveAiming("GrenadeLauncher03ReloadClip", true);
			base.GetComponent<Animation>()["GrenadeLauncher03ReloadClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["GrenadeLauncher03ReloadClip"].wrapMode = WrapMode.Once;
			bool[] array = new bool[]
			{
				true,
				true,
				false,
				true,
				true,
				true,
				true,
				true,
				true
			};
			for (int i = 1; i <= array.Length; i++)
			{
				string name = string.Format("Taunt{0}", i);
				string text = string.Format("Taunt{0}Clip", i);
				if (!(base.GetComponent<Animation>()[name] == null))
				{
					base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()[name].clip, text);
					this.SetupAdditiveAiming(text, array[i - 1]);
					base.GetComponent<Animation>()[text].blendMode = AnimationBlendMode.Blend;
					base.GetComponent<Animation>()[text].wrapMode = WrapMode.Once;
				}
			}
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Bat_Shot"].clip, "BatShotClip");
			this.SetupAdditiveAiming("BatShotClip", true);
			base.GetComponent<Animation>()["BatShotClip"].blendMode = AnimationBlendMode.Blend;
			base.GetComponent<Animation>()["BatShotClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Zombie_Shot"].clip, "ZombieShotClip");
			this.SetupAdditiveAiming("ZombieShotClip", true);
			base.GetComponent<Animation>()["ZombieShotClip"].blendMode = AnimationBlendMode.Blend;
			base.GetComponent<Animation>()["ZombieShotClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Bat_Idle"].clip, "BatIdleClip");
			this.SetupAdditiveAiming("BatIdleClip", true);
			base.GetComponent<Animation>()["BatIdleClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["BatIdleClip"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Katana_Shot"].clip, "KatanaShotClip");
			this.SetupAdditiveAiming("KatanaShotClip", true);
			base.GetComponent<Animation>()["KatanaShotClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["KatanaShotClip"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["Katana_Idle"].clip, "KatanaIdleClip");
			this.SetupAdditiveAiming("KatanaIdleClip", true);
			base.GetComponent<Animation>()["KatanaIdleClip"].blendMode = AnimationBlendMode.Additive;
			base.GetComponent<Animation>()["KatanaIdleClip"].wrapMode = WrapMode.Loop;
		}
	}

	private bool IsBendAnimation()
	{
		string text = this.BendAnimationName;
		switch (text)
		{
		case "BatBendClip":
			if (base.GetComponent<Animation>().IsPlaying("BatShotClip"))
			{
				return false;
			}
			break;
		case "KatanaBendClip":
			if (base.GetComponent<Animation>().IsPlaying("KatanaShotClip"))
			{
				return false;
			}
			break;
		case "MakarovBendClip":
			if (base.GetComponent<Animation>().IsPlaying("HandGunWeaponReloadClip"))
			{
				return false;
			}
			break;
		case "UMP45BendClip":
			if (base.GetComponent<Animation>().IsPlaying("UMP45ReloadClip"))
			{
				return false;
			}
			break;
		case "SteyrAUGBendClip":
			if (base.GetComponent<Animation>().IsPlaying("SteyrAUGReloadClip"))
			{
				return false;
			}
			break;
		case "WildcatBendClip":
			if (base.GetComponent<Animation>().IsPlaying("WildcatReloadClip"))
			{
				return false;
			}
			break;
		case "TMPBendClip":
			if (base.GetComponent<Animation>().IsPlaying("TMPReloadClip"))
			{
				return false;
			}
			break;
		case "AK47BendClip":
			if (base.GetComponent<Animation>().IsPlaying("MachineGunWeaponReloadClip"))
			{
				return false;
			}
			break;
		case "SpasBendClip":
		case "Winchester1887BendClip":
		case "NovapumpBendClip":
			if (base.GetComponent<Animation>().IsPlaying("ShotGunWeaponReloadClip"))
			{
				return false;
			}
			break;
		case "RPG7BendClip":
		case "RPG26BendClip":
			if (base.GetComponent<Animation>().IsPlaying("RocketLauncherWeaponReloadClip"))
			{
				return false;
			}
			break;
		case "M202A1BendClip":
			if (base.GetComponent<Animation>().IsPlaying("M202A1ReloadClip"))
			{
				return false;
			}
			break;
		case "MilkorBendClip":
			if (base.GetComponent<Animation>().IsPlaying("GrenadeLauncherWeaponReloadClip"))
			{
				return false;
			}
			break;
		case "SnowLauncherBendClip":
			if (base.GetComponent<Animation>().IsPlaying("SnowLauncherWeaponReloadClip"))
			{
				return false;
			}
			break;
		case "VintorezBendClip":
		case "SVDBendClip":
			if (base.GetComponent<Animation>().IsPlaying("MachineGunWeaponReloadClip"))
			{
				return false;
			}
			break;
		case "SniperRifle03BendClip":
			if (base.GetComponent<Animation>().IsPlaying("SniperRifle03ReloadClip"))
			{
				return false;
			}
			break;
		case "AssaultRifle03BendClip":
			if (base.GetComponent<Animation>().IsPlaying("AssaultRifle03ReloadClip"))
			{
				return false;
			}
			break;
		case "AssaultRifle02BendClip":
			if (base.GetComponent<Animation>().IsPlaying("AssaultRifle02ReloadClip"))
			{
				return false;
			}
			break;
		case "Shotgun02BendClip":
			if (base.GetComponent<Animation>().IsPlaying("Shotgun02ReloadClip"))
			{
				return false;
			}
			break;
		case "GrenadeLauncher03BendClip":
			if (base.GetComponent<Animation>().IsPlaying("GrenadeLauncher03ReloadClip"))
			{
				return false;
			}
			break;
		}
		return true;
	}

	private void SetupHeadAiming(string anim, bool mixingTransform)
	{
		if (mixingTransform)
		{
			base.GetComponent<Animation>()[anim].AddMixingTransform(base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Spine3/Bip01 Neck"), false);
			base.GetComponent<Animation>()[anim].AddMixingTransform(base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Spine3/Bip01 Neck/Bip01 Head"), false);
		}
		base.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Blend;
		base.GetComponent<Animation>()[anim].enabled = true;
		base.GetComponent<Animation>()[anim].weight = 0f;
		base.GetComponent<Animation>()[anim].layer = 1;
		base.GetComponent<Animation>()[anim].time = 0f;
		base.GetComponent<Animation>()[anim].speed = 0f;
		base.GetComponent<Animation>()[anim].wrapMode = WrapMode.Once;
	}

	private void SetupAdditiveAiming(string anim, bool mixingTransform)
	{
		if (mixingTransform)
		{
			base.GetComponent<Animation>()[anim].AddMixingTransform(base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1"));
		}
		base.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Blend;
		base.GetComponent<Animation>()[anim].enabled = true;
		base.GetComponent<Animation>()[anim].weight = 0f;
		base.GetComponent<Animation>()[anim].layer = 1;
		base.GetComponent<Animation>()[anim].time = 0f;
		base.GetComponent<Animation>()[anim].speed = 0f;
		base.GetComponent<Animation>()[anim].wrapMode = WrapMode.Once;
	}

	private void CheckSoldierState()
	{
		this.aim = this.playerRemote.Aim;
		this.fire = this.playerRemote.Fire;
		if (NetworkDev.Remote_Animation || this == PlayerManager.Instance.LocalPlayer.ActorAnimator)
		{
			this.walk = this.playerRemote.Walk;
		}
		this.crouch = this.playerRemote.CrouchStatus;
		this.reloading = this.playerRemote.Reload;
		this.currentWeapon = (int)this.playerRemote.currentWeapon;
		this.inAir = this.playerRemote.InAir;
		this.currentWeaponName = ((this.playerRemote.currentWeapon != 0) ? "M203" : "M4");
		this.aimTarget = this.playerRemote.targetpos;
	}

	private float CrossFadeUp(float weight, float fadeTime)
	{
		return Mathf.Clamp01(weight + Time.deltaTime / fadeTime);
	}

	private float CrossFadeDown(float weight, float fadeTime)
	{
		return Mathf.Clamp01(weight - Time.deltaTime / fadeTime);
	}

	private void AnimationRemote()
	{
		this.walk = this.playerRemote.Walk;
		this.keyState = this.playerRemote.keyState;
	}

	private void AnimationAdaptive()
	{
		float num = (float)(DateTime.Now.Ticks - this.lastPositionTime) / 1E+07f;
		float num2 = (base.transform.localEulerAngles.y - this.lastRotation.y) / num;
		Vector3 direction = base.transform.position - this.lastPosition;
		if (direction.sqrMagnitude < this.IDLE_THRESHOLD && Mathf.Abs(num2) < this.ROTATION_IDLE_THRESHOLD)
		{
			if (DateTime.Now.Ticks - this.lastPositionTime > (long)this.IDLE_PERIOD)
			{
				this.keyState = KeyState.Still;
			}
			this.deltaZ = 0f; this.deltaR = (this.deltaX = (this.deltaZ ));
			return;
		}
		this.lastPositionTime = DateTime.Now.Ticks;
		if (Mathf.Abs(num2) < this.ROTATION_IDLE_THRESHOLD)
		{
			this.deltaR = 0f;
		}
		this.lastRotation = base.transform.localEulerAngles;
		this.deltaR = Mathf.LerpAngle(this.deltaR, num2, this.deltaRotationSmooth);
		if (direction.sqrMagnitude < this.IDLE_THRESHOLD)
		{
			if (this.deltaR > this.ROTATION_WALK_THRESHOLD)
			{
				this.keyState = KeyState.RunStrafeRight;
				this.walk = false;
			}
			else if (this.deltaR > 0f)
			{
				this.keyState = KeyState.WalkStrafeRight;
				this.walk = true;
			}
			else if (this.deltaR < -this.ROTATION_WALK_THRESHOLD)
			{
				this.keyState = KeyState.RunStrafeLeft;
				this.walk = false;
			}
			else if (this.deltaR < 0f)
			{
				this.keyState = KeyState.WalkStrafeLeft;
				this.walk = true;
			}
			this.deltaZ = 0f; this.deltaX = (this.deltaZ );
			return;
		}
		this.lastPosition = base.transform.position;
		Vector3 vector = base.transform.InverseTransformDirection(direction);
		float b = vector.x / num;
		float b2 = vector.z / num;
		this.deltaX = Mathf.Lerp(this.deltaX, b, this.deltaSmooth);
		this.deltaZ = Mathf.Lerp(this.deltaZ, b2, this.deltaSmooth);
		if (Mathf.Abs(this.deltaX) > Mathf.Abs(this.deltaZ) + this.STRAFE_THRESHOLD)
		{
			if (this.deltaX > this.WALK_THRESHOLD)
			{
				this.keyState = KeyState.RunStrafeRight;
				this.walk = false;
			}
			else if (this.deltaX > 0f)
			{
				this.keyState = KeyState.WalkStrafeRight;
				this.walk = true;
			}
			else if (this.deltaX < -this.WALK_THRESHOLD)
			{
				this.keyState = KeyState.RunStrafeLeft;
				this.walk = false;
			}
			else if (this.deltaX < 0f)
			{
				this.keyState = KeyState.WalkStrafeLeft;
				this.walk = true;
			}
			return;
		}
		if (this.deltaZ > this.WALK_THRESHOLD)
		{
			this.keyState = KeyState.Runing;
			this.walk = false;
		}
		else if (this.deltaZ > 0f)
		{
			this.keyState = KeyState.Walking;
			this.walk = true;
		}
		else if (this.deltaZ < -this.WALK_THRESHOLD)
		{
			this.keyState = KeyState.RuningBack;
			this.walk = false;
		}
		else if (this.deltaZ < 0f)
		{
			this.keyState = KeyState.WalkBack;
			this.walk = true;
		}
	}

	private void AnimationBlendKey()
	{
		switch (this.keyState)
		{
		case KeyState.Still:
			this.Idle();
			break;
		case KeyState.Walking:
			this.WalkFoward();
			break;
		case KeyState.Runing:
			this.Run();
			break;
		case KeyState.WalkBack:
			this.WalkBack();
			break;
		case KeyState.RuningBack:
			this.RunBack();
			break;
		case KeyState.WalkStrafeLeft:
			this.WalkStrafeLeft();
			break;
		case KeyState.WalkStrafeRight:
			this.WalkStrafeRight();
			break;
		case KeyState.RunStrafeLeft:
			this.RunStrafeLeft();
			break;
		case KeyState.RunStrafeRight:
			this.RunStrafeRight();
			break;
		}
	}

	private void FixedUpdate()
	{
		if (this.shopAnimator)
		{
			return;
		}
		if (!NetworkDev.Remote_Animation && (PlayerManager.Instance.LocalPlayer == null || this != PlayerManager.Instance.LocalPlayer.ActorAnimator))
		{
			this.AnimationAdaptive();
		}
	}

	private void Update()
	{
		if (this.shopAnimator || PlayerManager.Instance == null)
		{
			return;
		}
		if (NetworkDev.Remote_Animation || this == PlayerManager.Instance.LocalPlayer.ActorAnimator)
		{
			this.AnimationRemote();
		}
		if (this.isRunning != this.IsRunning)
		{
			this.isRunning = this.IsRunning;
			WeaponLook[] componentsInChildren = base.transform.GetComponentsInChildren<WeaponLook>();
			if (componentsInChildren.Length > 0)
			{
				Transform parent = componentsInChildren[0].transform.parent;
				Animation component = parent.GetComponent<Animation>();
				if (component != null)
				{
					if (this.isRunning)
					{
						this.RunWeaponAnimation(component);
					}
					else
					{
						this.IdleWeaponAnimation(component);
					}
				}
			}
		}
		if (this.LegsAudio != null)
		{
			bool flag = this.isRunning && !this.inAir;
			if (PlayerManager.Instance.LocalPlayer != null && PlayerManager.Instance.LocalPlayer.ActorAnimator != this)
			{
				flag = (flag && (this.playerRemote.IsEnemy || !PlayerManager.Instance.LocalPlayer.ContainsEnhancer(EnhancerType.ClanReducedSoundFriend)));
			}
			if (flag)
			{
				SoundManager.Instance.Play(this.LegsAudio, "Run_Concrete", AudioPlayMode.PlayLoop);
			}
			else
			{
				SoundManager.Instance.Play(this.LegsAudio, "Run_Concrete", AudioPlayMode.Stop);
			}
		}
		if (this.isWalking != this.IsWalking)
		{
			this.isWalking = this.IsWalking;
		}
		this.CheckSoldierState();
		if (this.crouch)
		{
			this.crouchWeight = this.CrossFadeUp(this.crouchWeight, 0.4f);
		}
		else if (this.inAir && this.jumpLandCrouchAmount > 0f)
		{
			this.crouchWeight = this.CrossFadeUp(this.crouchWeight, 1f / this.jumpLandCrouchAmount);
		}
		else
		{
			this.crouchWeight = this.CrossFadeDown(this.crouchWeight, 0.45f);
		}
		float num = 1f - this.crouchWeight;
		if (this.fire)
		{
			this.aimWeight = this.CrossFadeUp(this.aimWeight, 0.2f);
			this.fireWeight = this.CrossFadeUp(this.fireWeight, 0.2f);
		}
		else if (this.aim)
		{
			this.aimWeight = this.CrossFadeUp(this.aimWeight, 0.3f);
			this.fireWeight = this.CrossFadeDown(this.fireWeight, 0.3f);
		}
		else
		{
			this.aimWeight = this.CrossFadeDown(this.aimWeight, 0.5f);
			this.fireWeight = this.CrossFadeDown(this.fireWeight, 0.5f);
		}
		float num2 = 1f - this.aimWeight;
		if (this.inAir)
		{
			this.groundedWeight = this.CrossFadeDown(this.groundedWeight, 0.1f);
		}
		else
		{
			this.groundedWeight = this.CrossFadeUp(this.groundedWeight, 0.2f);
		}
		if (this.soldierController != null)
		{
			this.BendAnimationAngle = this.soldierController.targetXRotation / 180f + 0.5f;
		}
		if (this.BendAnimationAngle < 0f)
		{
			this.BendAnimationAngle = 0f;
		}
		if (this.BendAnimationAngle > 0.96f)
		{
			this.BendAnimationAngle = 0.96f;
		}
		base.GetComponent<Animation>()[this.BendAnimationName].time = this.BendAnimationAngle;
		if (!this.taunting && !this.isZombie)
		{
			if (this.IsBendAnimation())
			{
				base.GetComponent<Animation>()[this.BendAnimationName].weight = 1f;
				base.GetComponent<Animation>().CrossFade(this.BendAnimationName, 0.2f);
			}
		}
		else
		{
			base.GetComponent<Animation>()[this.BendAnimationName].weight = 0f;
		}
		if (this.reloading)
		{
		}
		if (!this.inAir)
		{
			if (this.isZombie)
			{
				if (this.RunAnimationName != "Zombie_Run")
				{
					this.RunAnimationName = "Zombie_Run";
					this.StrafeRunLeftAnimationName = "Zombie_RunStrafeLeft";
					this.StrafeRunRightAnimationName = "Zombie_RunStrafeRight";
					this.RunBackwardsAnimationName = "Zombie_RunBackwards";
					this.IdleAnimationName = "Zombie_Idle";
					this.JumpAnimationName = "Zombie_Jump";
					base.GetComponent<Animation>()[this.RunAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunLeftAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunRightAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.RunBackwardsAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.IdleAnimationName].speed = this.idleAnimationSpeed;
					base.GetComponent<Animation>()[this.BendAnimationName].weight = 0f;
				}
			}
			else if (this.crouch)
			{
				if (this.RunAnimationName != "CroachWalk")
				{
					this.RunAnimationName = "CroachWalk";
					this.StrafeRunLeftAnimationName = "CroachStrafeLeftWalk";
					this.StrafeRunRightAnimationName = "CroachStrafeRightWalk";
					this.RunBackwardsAnimationName = "CroachWalkBackwards";
					this.IdleAnimationName = "CroachIdle";
					this.JumpAnimationName = "Jump";
					base.GetComponent<Animation>()[this.RunAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunLeftAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunRightAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.RunBackwardsAnimationName].speed = this.crouchAnimationSpeed;
					base.GetComponent<Animation>()[this.IdleAnimationName].speed = this.idleAnimationSpeed;
				}
			}
			else if (this.walk)
			{
				if (this.RunAnimationName != "Walk")
				{
					this.RunAnimationName = "Walk";
					this.StrafeRunLeftAnimationName = "StrafeLeftWalk";
					this.StrafeRunRightAnimationName = "StrafeRightWalk";
					this.RunBackwardsAnimationName = "WalkBackwards";
					this.IdleAnimationName = "Idle";
					this.JumpAnimationName = "Jump";
					base.GetComponent<Animation>()[this.RunAnimationName].speed = this.walkAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunLeftAnimationName].speed = this.walkAnimationSpeed;
					base.GetComponent<Animation>()[this.StrafeRunRightAnimationName].speed = this.walkAnimationSpeed;
					base.GetComponent<Animation>()[this.RunBackwardsAnimationName].speed = this.walkAnimationSpeed;
					base.GetComponent<Animation>()[this.IdleAnimationName].speed = this.idleAnimationSpeed;
				}
			}
			else if (this.RunAnimationName != "Run2")
			{
				this.RunAnimationName = "Run2";
				this.StrafeRunLeftAnimationName = "StrafeRunLeft2";
				this.StrafeRunRightAnimationName = "StrafeRunRight2";
				this.RunBackwardsAnimationName = "RunBackwards";
				this.IdleAnimationName = "Idle";
				this.JumpAnimationName = "Jump";
				base.GetComponent<Animation>()[this.RunAnimationName].speed = this.runAnimationSpeed;
				base.GetComponent<Animation>()[this.StrafeRunLeftAnimationName].speed = this.runAnimationSpeed;
				base.GetComponent<Animation>()[this.StrafeRunRightAnimationName].speed = this.runAnimationSpeed;
				base.GetComponent<Animation>()[this.RunBackwardsAnimationName].speed = this.runAnimationSpeed;
				base.GetComponent<Animation>()[this.IdleAnimationName].speed = this.idleAnimationSpeed;
			}
			if (this.jump)
			{
				this.JumpFinish(this.JumpAnimationName);
			}
			this.jump = false;
			this.AnimationBlendKey();
		}
		else
		{
			if (!this.jump)
			{
				if (this.playerRemote.keyState == KeyState.Runing || this.playerRemote.keyState == KeyState.RunStrafeLeft || this.playerRemote.keyState == KeyState.RunStrafeRight)
				{
					this.JumpStart(this.JumpAnimationName);
				}
				else
				{
					this.JumpStart(this.JumpAnimationName);
				}
			}
			this.jump = true;
			this.Jump();
		}
	}

	public void TestAnimation()
	{
		base.GetComponent<Animation>()[this.BendAnimationName].weight = 1f;
		UnityEngine.Debug.Log(string.Format("Animation Test", new object[0]));
		foreach (object obj in base.GetComponent<Animation>())
		{
			AnimationState animationState = (AnimationState)obj;
			if (animationState.weight != 0f)
			{
				UnityEngine.Debug.Log(string.Format("Animation[\"{0}\"] w:{1} s:{2} t: {3}", new object[]
				{
					animationState.name,
					animationState.weight,
					animationState.speed,
					animationState.time
				}));
			}
		}
	}

	public void Fire(int Gun)
	{
	}

	public void RunWeaponAnimation(Animation weaponContainerAnimation)
	{
		if (weaponContainerAnimation.IsPlaying("WeaponChangeAnimation"))
		{
			weaponContainerAnimation.Play("WeaponAnimation_ShotgunRun", AnimationPlayMode.Queue);
		}
		else
		{
			weaponContainerAnimation.CrossFade("WeaponAnimation_ShotgunRun", 0.2f);
		}
	}

	public void IdleWeaponAnimation(Animation weaponContainerAnimation)
	{
		if (weaponContainerAnimation.IsPlaying("WeaponChangeAnimation"))
		{
			weaponContainerAnimation.Play("WeaponAnimation_ShotgunIdle", AnimationPlayMode.Queue);
		}
		else
		{
			weaponContainerAnimation.CrossFade("WeaponAnimation_ShotgunIdle", 0.2f);
		}
	}

	public void ShotAnimationReset(string weaponName)
	{
	}

	public void ChangeWeaponAnimation(string weaponName)
	{
		this.SetAnimationProperties();
		if (this.BendAnimationName != weaponName + "BendClip")
		{
			base.GetComponent<Animation>().Blend(this.BendAnimationName, 0f, 0.3f);
			this.BendAnimationName = weaponName + "BendClip";
		}
		if (weaponName == "Bat")
		{
			if (!this.crouch)
			{
			}
			base.GetComponent<Animation>().CrossFade(this.BendAnimationName, 0.3f);
		}
		if (weaponName == "Katana")
		{
			if (!this.crouch)
			{
			}
			base.GetComponent<Animation>().CrossFade(this.BendAnimationName, 0.3f);
		}
		if (!this.crouch)
		{
			this.IdleAnimationName = "Idle";
		}
		base.GetComponent<Animation>().CrossFade(this.BendAnimationName, 0.3f);
	}

	public void TauntAnimation(string tauntName)
	{
		if (base.GetComponent<Animation>()[tauntName + "Clip"] == null)
		{
			UnityEngine.Debug.LogError(string.Format("No taunt Animtion: \"{0}Clip\"", tauntName));
			return;
		}
		base.GetComponent<Animation>()[tauntName + "Clip"].speed = 1f;
		base.GetComponent<Animation>().Blend(tauntName + "Clip", 1f, 0.2f);
		this.taunting = true;
		base.GetComponent<Animation>().Blend(this.BendAnimationName, 0f, 0.2f);
	}

	public void FinishTauntAnimation()
	{
		this.taunting = false;
		base.GetComponent<Animation>().Blend(this.BendAnimationName, 1f, 0.2f);
	}

	public void FinishTauntAnimation(bool bend)
	{
		this.taunting = false;
		if (bend)
		{
			base.GetComponent<Animation>().Blend(this.BendAnimationName, 1f, 0.2f);
		}
	}

	public void ReloadWeaponAnimation(string weaponName, CombatWeapon weapon)
	{
		if (weaponName == "MachineGunWeapon" || weaponName == "HandGunWeapon" || weaponName == "RocketLauncherWeapon" || weaponName == "GrenadeLauncherWeapon" || weaponName == "BombLauncherWeapon" || weaponName == "ShotGunWeapon")
		{
			if (weapon.SystemName.StartsWith("GL_GrenadeLauncher03"))
			{
				base.GetComponent<Animation>()["GrenadeLauncher03ReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("GrenadeLauncher03ReloadClip", 1f, 0.3f);
			}
			else if (weapon.SystemName.StartsWith("SG_Shotgun02"))
			{
				base.GetComponent<Animation>()["Shotgun02ReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("Shotgun02ReloadClip", 1f, 0.3f);
			}
			else if (weapon.SystemName.StartsWith("MG_AssaultRifle03"))
			{
				base.GetComponent<Animation>()["AssaultRifle03ReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("AssaultRifle03ReloadClip", 1f, 0.3f);
			}
			else if (weapon.SystemName.StartsWith("MG_AssaultRifle02"))
			{
				base.GetComponent<Animation>()["AssaultRifle02ReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("AssaultRifle02ReloadClip", 1f, 0.3f);
			}
			else if (weapon.SystemName.StartsWith("MG_AUG"))
			{
				base.GetComponent<Animation>()["SteyrAUGReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("SteyrAUGReloadClip", 1f, 0.3f);
			}
			else if (weapon.SystemName.StartsWith("MG_UMP45D") || weapon.SystemName.StartsWith("MG_UMP45V"))
			{
				base.GetComponent<Animation>()["UMP45ReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("UMP45ReloadClip", 1f, 0.3f);
			}
			else if (weapon.SystemName.StartsWith("MG_UMP45"))
			{
				base.GetComponent<Animation>()["TMPReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("TMPReloadClip", 1f, 0.3f);
			}
			else if (weapon.SystemName.StartsWith("BL_Sticky"))
			{
				base.GetComponent<Animation>()["GrenadeLauncherWeaponReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("GrenadeLauncherWeaponReloadClip", 1f, 0.3f);
			}
			else
			{
				base.GetComponent<Animation>()[weaponName + "ReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend(weaponName + "ReloadClip", 1f, 0.3f);
			}
		}
		else if (weaponName == "SniperRifleWeapon")
		{
			if (weapon.SystemName.StartsWith("SR_SniperRifle03"))
			{
				base.GetComponent<Animation>()["SniperRifle03ReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("SniperRifle03ReloadClip", 1f, 0.3f);
			}
			else if (weapon.SystemName.StartsWith("SR_Wildcat"))
			{
				base.GetComponent<Animation>()["WildcatReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("WildcatReloadClip", 1f, 0.3f);
			}
			else
			{
				base.GetComponent<Animation>()["MachineGunWeaponReloadClip"].speed = 1f;
				base.GetComponent<Animation>().Blend("MachineGunWeaponReloadClip", 1f, 0.3f);
			}
		}
		else
		{
			base.GetComponent<Animation>()["ShotgunReloadClip"].speed = 1f;
			base.GetComponent<Animation>().Blend("ShotgunReloadClip", 1f, 0.3f);
		}
	}

	public void ReloadWeaponAnimation(Transform weaponTransform, bool complexReload, int ammoCount)
	{
		if (weaponTransform == null)
		{
			return;
		}
		Animation componentInChildren = weaponTransform.GetComponentInChildren<Animation>();
		if (complexReload)
		{
			if (componentInChildren["ReloadStart"] == null)
			{
				return;
			}
			componentInChildren["ReloadStart"].speed = 1f;
			componentInChildren["ReloadAmmo"].speed = 1f;
			componentInChildren["ReloadEnd"].speed = 1f;
			componentInChildren.PlayQueued("ReloadStart", QueueMode.CompleteOthers);
			while (ammoCount-- > 0)
			{
				componentInChildren.PlayQueued("ReloadAmmo", QueueMode.CompleteOthers);
			}
			componentInChildren.PlayQueued("ReloadEnd", QueueMode.CompleteOthers);
		}
		else
		{
			if (componentInChildren["Reload"] == null)
			{
				return;
			}
			componentInChildren["Reload"].speed = 1f;
			componentInChildren.Play("Reload", AnimationPlayMode.Queue);
		}
	}

	public void ResetReloadWeaponAnimation(Transform weaponTransform)
	{
		if (weaponTransform == null)
		{
			return;
		}
		Animation animation = weaponTransform.GetComponentInChildren<Animation>();
		if (animation != null)
		{
			animation.Play("Reset");
		}
		else
		{
			animation = weaponTransform.GetComponent<Animation>();
			if (animation != null)
			{
				animation.Play("Reset");
			}
		}
		if (animation == null)
		{
			Animation[] componentsInChildren = weaponTransform.GetComponentsInChildren<Animation>();
			for (int i = 0; i < weaponTransform.childCount; i++)
			{
				Transform child = weaponTransform.GetChild(i);
				animation = child.GetComponent<Animation>();
				animation.Play("Reset");
			}
		}
	}

	public void ShotWeaponAnimation(string animationName, Transform weaponTransform)
	{
		if (weaponTransform == null)
		{
			return;
		}
		Animation componentInChildren = weaponTransform.GetComponentInChildren<Animation>();
		if (componentInChildren == null)
		{
			return;
		}
		if (animationName == "Stop")
		{
			if (componentInChildren["End"] != null)
			{
				componentInChildren["End"].speed = 1.5f;
				componentInChildren.Play("End", PlayMode.StopSameLayer);
			}
			return;
		}
		if (animationName == "Load")
		{
			if (componentInChildren["Start"] != null)
			{
				componentInChildren["Start"].speed = 1.5f;
				componentInChildren.Play("Start", PlayMode.StopSameLayer);
			}
			if (componentInChildren["Load"] != null)
			{
				componentInChildren["Load"].speed = 1.5f;
				componentInChildren.Play("Load", AnimationPlayMode.Queue);
			}
			return;
		}
		if (animationName == "Spin")
		{
			if (componentInChildren["Load"] != null)
			{
				componentInChildren["Load"].speed = 1.5f;
				componentInChildren.Play("Load", PlayMode.StopSameLayer);
			}
			return;
		}
		componentInChildren[animationName].speed = 1.5f;
		componentInChildren[animationName].time = 0f;
		componentInChildren.Play(animationName, PlayMode.StopSameLayer);
		if (componentInChildren["Idle"] != null)
		{
			componentInChildren["Idle"].wrapMode = WrapMode.Loop;
			componentInChildren.Play("Idle", AnimationPlayMode.Queue);
		}
	}

	public void ShotWeaponAnimation(string weaponName)
	{
		if (!this.setFlag)
		{
			return;
		}
		if (weaponName == "Bat")
		{
			base.GetComponent<Animation>()[weaponName + "ShotClip"].speed = 2f;
		}
		else if (weaponName == "Katana")
		{
			base.GetComponent<Animation>()[weaponName + "ShotClip"].speed = 1.75f;
		}
		else
		{
			base.GetComponent<Animation>()[weaponName + "ShotClip"].speed = 1.5f;
		}
		base.GetComponent<Animation>().Blend(weaponName + "ShotClip", 1f, 0.1f);
	}

	public void ChangeWeaponAnimation(Animation weaponContainerAnimation)
	{
		if (weaponContainerAnimation.IsPlaying("WeaponChangeAnimation"))
		{
		}
		weaponContainerAnimation.Play("WeaponChangeAnimation", PlayMode.StopSameLayer);
		if (this.isRunning)
		{
			weaponContainerAnimation.Play("WeaponAnimation_ShotgunRun", AnimationPlayMode.Queue);
		}
		else
		{
			weaponContainerAnimation.Play("WeaponAnimation_ShotgunIdle", AnimationPlayMode.Queue);
		}
	}

	public void Idle()
	{
		base.GetComponent<Animation>()[this.IdleAnimationName].speed = this.idleAnimationSpeed;
		base.GetComponent<Animation>().CrossFade(this.IdleAnimationName);
	}

	public void Run()
	{
		base.GetComponent<Animation>()[this.RunAnimationName].speed = this.runAnimationSpeed;
		base.GetComponent<Animation>().CrossFade(this.RunAnimationName);
	}

	public void RunBack()
	{
		base.GetComponent<Animation>()[this.RunBackwardsAnimationName].speed = this.runAnimationSpeed;
		base.GetComponent<Animation>().CrossFade(this.RunBackwardsAnimationName);
	}

	public void RunStrafeLeft()
	{
		base.GetComponent<Animation>()[this.StrafeRunLeftAnimationName].speed = this.runAnimationSpeed;
		base.GetComponent<Animation>().CrossFade(this.StrafeRunLeftAnimationName);
	}

	public void RunStrafeRight()
	{
		base.GetComponent<Animation>()[this.StrafeRunRightAnimationName].speed = this.runAnimationSpeed;
		base.GetComponent<Animation>().CrossFade(this.StrafeRunRightAnimationName);
	}

	public void WalkFoward()
	{
		base.GetComponent<Animation>().CrossFade(this.RunAnimationName);
	}

	public void WalkBack()
	{
		base.GetComponent<Animation>().CrossFade(this.RunBackwardsAnimationName);
	}

	public void WalkStrafeLeft()
	{
		base.GetComponent<Animation>().CrossFade(this.StrafeRunLeftAnimationName);
	}

	public void WalkStrafeRight()
	{
		base.GetComponent<Animation>().CrossFade(this.StrafeRunRightAnimationName);
	}

	public void JumpStart(string jumpType)
	{
		base.GetComponent<Animation>()[jumpType].time = 0f;
		base.GetComponent<Animation>()[jumpType].weight = 1f;
		base.GetComponent<Animation>()[jumpType].speed = this.jumpAnimationSpeed;
		base.GetComponent<Animation>()[jumpType].time = 0f;
		base.GetComponent<Animation>().CrossFade(jumpType, 0.3f);
	}

	public void JumpFinish(string jumpType)
	{
	}

	public void Jump()
	{
	}

	public void Defeat(Vector3 shotImpulse)
	{
		base.GetComponent<Animation>().enabled = false;
		foreach (Rigidbody rigidbody in base.GetComponentsInChildren<Rigidbody>(true))
		{
			rigidbody.isKinematic = false;
			rigidbody.useGravity = true;
			rigidbody.sleepThreshold = this.sleepVelocity;
			Rigidbody rigidbody2 = rigidbody;
			float num = 1f;
			rigidbody.angularDrag = num;
			rigidbody2.drag = num;
			RigidBodyForce rigidBodyForce = rigidbody.GetComponent<RigidBodyForce>();
			if (rigidBodyForce == null)
			{
				rigidBodyForce = rigidbody.gameObject.AddComponent<RigidBodyForce>();
			}
			rigidBodyForce.Vector = new Vector3(0f, -100f, 0f);
			rigidBodyForce.ImpulseVector = shotImpulse;
			if (this == PlayerManager.Instance.LocalPlayer.ActorAnimator)
			{
				rigidbody.GetComponent<Collider>().enabled = true;
			}
		}
	}

	public void Ressurrect()
	{
		this.jump = false;
		this.inAir = false;
		this.playerRemote.CrouchStatus = false;
		this.playerRemote.InAir = false;
		foreach (Rigidbody rigidbody in base.GetComponentsInChildren<Rigidbody>(true))
		{
			if (rigidbody.gameObject.name == "Bip01")
			{
				rigidbody.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
			rigidbody.isKinematic = true;
			rigidbody.useGravity = true;
			RigidBodyForce component = rigidbody.GetComponent<RigidBodyForce>();
			if (component != null)
			{
				component.Vector = new Vector3(0f, 0f, 0f);
			}
			ConstantForce component2 = rigidbody.GetComponent<ConstantForce>();
			if (component2 != null)
			{
				component2.force = new Vector3(0f, 0f, 0f);
			}
			if (this == PlayerManager.Instance.LocalPlayer.ActorAnimator)
			{
				rigidbody.GetComponent<Collider>().enabled = false;
			}
		}
		base.GetComponent<Animation>().enabled = true;
	}

	private Vector3 lastPosition;

	private Vector3 lastRotation;

	private long lastPositionTime = DateTime.Now.Ticks;

	public int IDLE_PERIOD = 1000000;

	public float IDLE_THRESHOLD = 0.0001f;

	public float WALK_THRESHOLD = 10f;

	public float STRAFE_THRESHOLD;

	public float ROTATION_WALK_THRESHOLD = 100f;

	public float ROTATION_IDLE_THRESHOLD = 4f;

	public float deltaX;

	public float deltaZ;

	public float deltaR;

	public float deltaSmooth = 0.2f;

	public float deltaRotationSmooth = 0.2f;

	public PlayerRemote playerRemote;

	public string currentWeaponName;

	public float jumpLandCrouchAmount = 1.6f;

	public float sleepVelocity = 0.15f;

	public bool aim;

	public bool fire;

	public bool walk;

	public bool crouch;

	public bool reloading;

	public int currentWeapon;

	public bool inAir;

	public Vector3 aimTarget;

	public Transform aimPivot;

	private float aimAngleY;

	private float groundedWeight = 1f;

	private float crouchWeight;

	private float aimWeight;

	private float fireWeight;

	public bool isRunning;

	public float runAnimationSpeed = 1.3f;

	public float crouchAnimationSpeed = 1.1f;

	public float walkAnimationSpeed = 1.1f;

	public float jumpAnimationSpeed = 1f;

	public float idleAnimationSpeed = 1f;

	private bool taunting;

	public bool isWalking;

	private KeyState keyState;

	private bool jump;

	public string RunAnimationName = "Run2";

	public string StrafeRunLeftAnimationName = "StrafeRunLeft2";

	public string StrafeRunRightAnimationName = "StrafeRunRight2";

	public string RunBackwardsAnimationName = "RunBackwards";

	public string JumpAnimationName = "Jump";

	private string idleAnimationName = "Idle";

	private string bendAnimationName = "BatBendClip";

	public int RunAnimationType = 2;

	public float BendAnimationAngle = 0.5f;

	public SoldierController soldierController;

	private bool setFlag;

	private bool isZombie;

	private AudioSource legsAudio;

	private AudioSource speakerAudio;

	private bool shopAnimator;
}
