// dnSpy decompiler from Assembly-CSharp.dll class: LocalTurretTracer
using System;
using UnityEngine;

public class LocalTurretTracer : TurretTracer
{
	public int Life
	{
		get
		{
			return this.life;
		}
	}

	public new WeaponType WeaponType
	{
		get
		{
			return this.weaponType;
		}
	}

	public new CombatPlayer Owner
	{
		get
		{
			return this.player;
		}
	}

	private void Start()
	{
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter particleEmitter in componentsInChildren)
		{
			particleEmitter.emit = false;
		}
		ParticleRenderer[] componentsInChildren2 = base.transform.GetComponentsInChildren<ParticleRenderer>();
		foreach (ParticleRenderer particleRenderer in componentsInChildren2)
		{
			particleRenderer.enabled = false;
		}
		this.launchDelay = 200;
	}

	public void Launch(Vector3 position, WeaponType weaponType, int life, int launchDelayAdd)
	{
		this.weaponType = weaponType;
		this.life = life;
		this.launchTime = TimeManager.Instance.NetworkTime + (long)this.launchDelay + (long)launchDelayAdd;
		base.transform.position = position;
		this.lastScanTime = TimeManager.Instance.NetworkTime;
	}

	public void Launch(Vector3 position, WeaponType weaponType, int life)
	{
		this.weaponType = weaponType;
		this.life = life;
		this.launchTime = TimeManager.Instance.NetworkTime + (long)this.launchDelay;
		base.transform.position = position;
		this.lastScanTime = TimeManager.Instance.NetworkTime;
	}

	public override void setVisible(bool visible)
	{
		MeshRenderer[] componentsInChildren = base.transform.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			meshRenderer.enabled = visible;
		}
	}

	private void FixedUpdate()
	{
		if (this.player == null)
		{
			this.player = LocalPlayerManager.Instance.LocalPlayer;
		}
		if (TimeManager.Instance.NetworkTime >= this.launchTime || this.launchTime == 0L)
		{
			this.active = true;
		}
		if (this.active)
		{
			if (this.fireTime > TimeManager.Instance.NetworkTime && this.fireTime != 0L)
			{
				return;
			}
			if (this.lastScanTime < TimeManager.Instance.NetworkTime + 100L || this.lastScanTime == 0L)
			{
				this.lastScanTime = TimeManager.Instance.NetworkTime;
				if (LocalPlayerManager.Instance.LocalScan(base.transform.position, this.touchDistance))
				{
					this.Trigger();
					return;
				}
			}
		}
	}

	protected void Trigger()
	{
		this.Shot(base.transform.position, 1);
	}

	protected override void Shot(Vector3 position, int targetID)
	{
		if (this.fire())
		{
			Shot shot = new Shot(position, new Vector3(0f, 1f, 0f), (byte)this.WeaponType);
			ShotTarget shotTarget = new ShotTarget();
			shotTarget.TargetID = targetID;
			shotTarget.EnergyDamage = 1;
			shotTarget.TargetTransform = LocalPlayerManager.Instance.LocalPlayer.transform;
			shot.Targets.Add(shotTarget);
			shot.LaunchMode = LaunchModes.TURRET_SHOT;
			LocalPlayerManager.Instance.LocalPlayer.Energy -= shotTarget.EnergyDamage;
			LocalPlayerManager.Instance.turretTeslaEffect(shot, LocalPlayerManager.Instance.LocalPlayer, this);
		}
	}

	public override bool fire()
	{
		if (Time.time > this.canFireTime || this.canFireTime == 0f)
		{
			this.canFireTime = Time.time + this.shotTime / 1000f;
			return true;
		}
		return false;
	}

	protected override void Blow(Vector3 position)
	{
		this.player.UnregisterItem(base.TimeStamp);
	}

	public override void Destroy()
	{
		this.active = false;
		base.enabled = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public bool RemoveHealth(int healthDamage)
	{
		this.life -= healthDamage;
		if (this.life <= 0)
		{
			this.life = 0;
			return true;
		}
		return false;
	}

	private int life;

	private new WeaponType weaponType;
}
