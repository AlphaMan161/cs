// dnSpy decompiler from Assembly-CSharp.dll class: TurretTracer
using System;
using UnityEngine;

public class TurretTracer : ItemTracer
{
	public CombatPlayer Owner
	{
		get
		{
			return this.player;
		}
	}

	private void Start()
	{
	}

	public virtual void Launch(Shot shot, CombatPlayer player, bool control)
	{
		this.shot = shot;
		this.launchTime = shot.TimeStamp;
		this.landingTime = shot.LandingTimeStamp;
		this.fireTime = shot.TimeStamp + 2000L;
		base.transform.position = shot.Origin;
		base.transform.LookAt(shot.Origin + shot.Direction);
		this.setVisible(false);
		this.control = control;
		this.player = player;
	}

	public virtual void Launch(Shot shot)
	{
		this.shot = shot;
		this.launchTime = shot.TimeStamp;
		this.landingTime = shot.LandingTimeStamp;
		this.fireTime = shot.TimeStamp + 2000L;
		base.transform.LookAt(shot.Origin);
		this.setVisible(false);
	}

	public virtual void setVisible(bool visible)
	{
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter particleEmitter in componentsInChildren)
		{
			particleEmitter.emit = visible;
		}
		Renderer[] componentsInChildren2 = base.transform.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren2)
		{
			renderer.enabled = visible;
		}
	}

	private void FixedUpdate()
	{
		if (TimeManager.Instance.NetworkTime >= this.launchTime && TimeManager.Instance.NetworkTime < this.landingTime)
		{
			this.active = true;
			this.setVisible(true);
		}
		else if (TimeManager.Instance.NetworkTime > this.landingTime)
		{
			this.Blow(this.shot.Origin);
			return;
		}
		if (this.active)
		{
			long num = this.landingTime - TimeManager.Instance.NetworkTime;
			if (num <= 0L)
			{
				this.Blow(this.shot.Origin);
				return;
			}
			if (this.fireTime > TimeManager.Instance.NetworkTime)
			{
				return;
			}
			if ((this.lastScanTime < TimeManager.Instance.NetworkTime + 100L || this.lastScanTime == 0L) && this.control)
			{
				this.lastScanTime = TimeManager.Instance.NetworkTime;
				int targetID = -1;
				this.Shot(base.transform.position, targetID);
				return;
			}
		}
	}

	protected virtual void Shot(Vector3 position, int targetID)
	{
		if (!this.control)
		{
			return;
		}
		if (this.fire())
		{
			Shot shot = new Shot(position, this.shot.Direction, (byte)this.shot.WeaponType);
			shot.TimeStamp = this.shot.TimeStamp;
			shot.LaunchMode = LaunchModes.TURRET_SHOT;
			ShotCalculator.NearestRadialShot(this.player, shot, false);
			if (shot.HasTargets)
			{
				NetworkManager.Instance.SendShot(shot);
			}
		}
	}

	public virtual bool fire()
	{
		if (Time.time > this.canFireTime || this.canFireTime == 0f)
		{
			this.canFireTime = Time.time + this.shotTime / 1000f;
			return true;
		}
		return false;
	}

	protected virtual void Blow(Vector3 position)
	{
		if (this.control)
		{
			Shot shot = new Shot(position, this.shot.Direction, (byte)this.shot.WeaponType);
			shot.LaunchMode = LaunchModes.BLOW;
			shot.TimeStamp = this.shot.TimeStamp;
			NetworkManager.Instance.SendShot(shot);
			this.player.UnregisterItem(base.TimeStamp);
		}
		else
		{
			this.Destroy();
		}
	}

	public override void Destroy()
	{
		this.active = false;
		base.enabled = false;
		this.setVisible(false);
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter particleEmitter in componentsInChildren)
		{
			particleEmitter.emit = false;
		}
		UnityEngine.Object.Destroy(base.gameObject, (float)this.destroyDelay / 1000f);
	}

	protected Shot shot;

	public int launchDelay = 200;

	public int destroyDelay = 300;

	protected long landingTime;

	protected long fireTime;

	protected long lastScanTime;

	protected bool active;

	protected bool control;

	protected float touchDistance = 30f;

	protected CombatPlayer player;

	protected float shotTime = 250f;

	protected float canFireTime;
}
