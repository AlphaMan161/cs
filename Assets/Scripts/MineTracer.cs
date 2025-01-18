// dnSpy decompiler from Assembly-CSharp.dll class: MineTracer
using System;
using UnityEngine;

public class MineTracer : ItemTracer
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

	public void Launch(Shot shot, CombatPlayer player, bool control)
	{
		this.shot = shot;
		this.launchTime = shot.TimeStamp;
		this.landingTime = shot.LandingTimeStamp;
		base.transform.position = shot.Origin;
		base.transform.LookAt(shot.Origin + shot.Direction);
		this.setMineVisible(false);
		this.control = control;
		this.player = player;
	}

	public void Launch(Shot shot)
	{
		this.shot = shot;
		this.launchTime = shot.TimeStamp;
		this.landingTime = shot.LandingTimeStamp;
		base.transform.LookAt(shot.Origin);
		this.setMineVisible(false);
	}

	public void setMineVisible(bool visible)
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
			this.setMineVisible(true);
		}
		else if (TimeManager.Instance.NetworkTime > this.landingTime)
		{
			if (this.shot != null)
			{
				this.DeactivateMine(this.shot.Origin);
			}
			else
			{
				this.Destroy();
			}
		}
		if (this.active)
		{
			long num = this.landingTime - TimeManager.Instance.NetworkTime;
			if (num <= 0L)
			{
				this.DeactivateMine(this.shot.Origin);
				return;
			}
			if ((this.lastScanTime < TimeManager.Instance.NetworkTime + 100L || this.lastScanTime == 0L) && this.control)
			{
				this.lastScanTime = TimeManager.Instance.NetworkTime;
				int num2 = PlayerManager.Instance.TeamScan(base.transform.position, this.touchDistance, (int)this.player.Team, true);
				if (num2 > 0)
				{
					this.BlowMine(base.transform.position);
					return;
				}
			}
		}
	}

	protected void DeactivateMine(Vector3 position)
	{
		if (this.control)
		{
			Shot shot = new Shot(position, this.shot.Direction, (byte)this.shot.WeaponType);
			shot.TimeStamp = this.shot.TimeStamp;
			switch (this.weaponType)
			{
			case WeaponType.MINE_ELECTRIC_REMOTE:
			case WeaponType.MINE_TOUCH:
			case WeaponType.MINE_REMOTE:
			case WeaponType.MINE_ELECTRIC:
				shot.LaunchMode = LaunchModes.BLOW;
				break;
			case WeaponType.MINE_TIME:
				ShotCalculator.RadialShot(this.player, shot, false);
				break;
			}
			NetworkManager.Instance.SendShot(shot);
			this.player.UnregisterItem(base.TimeStamp);
		}
		else
		{
			this.Destroy();
		}
	}

	protected void BlowMine(Vector3 position)
	{
		if (this.control)
		{
			Shot shot = new Shot(position, this.shot.Direction, (byte)this.shot.WeaponType);
			shot.TimeStamp = this.shot.TimeStamp;
			switch (this.weaponType)
			{
			case WeaponType.MINE_ELECTRIC_REMOTE:
				ShotCalculator.ChainRadialShot(this.player, shot, false);
				break;
			case WeaponType.MINE_TIME:
			case WeaponType.MINE_TOUCH:
			case WeaponType.MINE_REMOTE:
			case WeaponType.MINE_ELECTRIC:
				ShotCalculator.RadialShot(this.player, shot, false);
				break;
			}
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
		base.transform.FindChild("MineModel").gameObject.GetComponent<Renderer>().enabled = false;
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

	protected long lastScanTime;

	protected bool active;

	protected bool control;

	protected float touchDistance = 10f;

	protected CombatPlayer player;
}
