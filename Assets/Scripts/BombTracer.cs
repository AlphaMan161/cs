// dnSpy decompiler from Assembly-CSharp.dll class: BombTracer
using System;
using UnityEngine;

public class BombTracer : ItemTracer
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

	public void Launch(Shot shot, CombatWeapon weapon, CombatPlayer player, bool control, bool gameState)
	{
		this.shot = shot;
		this.launchTime = shot.TimeStamp;
		this.landingTime = shot.LandingTimeStamp;
		this.life = weapon.Life;
		base.transform.LookAt(shot.Origin);
		this.setRocketVisible(false);
		this.control = control;
		this.weaponType = shot.WeaponType;
		this.player = player;
		if (gameState)
		{
			base.transform.position = (Vector3)shot.Trajectory[shot.Trajectory.Count - 1];
			this.setRocketVisible(true);
		}
	}

	public void Launch(Shot shot)
	{
		this.shot = shot;
		this.launchTime = shot.TimeStamp;
		this.landingTime = shot.LandingTimeStamp;
		base.transform.LookAt(shot.Origin);
		this.setRocketVisible(false);
	}

	public void setRocketVisible(bool visible)
	{
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter particleEmitter in componentsInChildren)
		{
			particleEmitter.emit = visible;
		}
		Renderer[] componentsInChildren2 = base.transform.GetComponentsInChildren<Renderer>();
        TrailRenderer componentsT = base.gameObject.GetComponent<TrailRenderer>();
        componentsT.sharedMaterial.shader = Shader.Find("Particles/~Additive-Multiply");
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
			this.setRocketVisible(true);
		}
		else
		{
			if (TimeManager.Instance.NetworkTime > this.landingTime + (long)this.life)
			{
				this.Blow(this.shot.Origin);
				return;
			}
			if (TimeManager.Instance.NetworkTime > this.landingTime)
			{
				return;
			}
		}
		if (this.active)
		{
			long num = this.landingTime - TimeManager.Instance.NetworkTime;
			if (num > 0L)
			{
				float num2 = (float)num / (float)(this.landingTime - this.launchTime);
				int num3 = Mathf.FloorToInt((float)(this.shot.Trajectory.Count - 1) * (1f - num2));
				float num4 = (float)(this.shot.Trajectory.Count - 1) * (1f - num2) - (float)num3;
				Vector3 vector = (Vector3)this.shot.Trajectory[num3];
				Vector3 vector2 = (Vector3)this.shot.Trajectory[num3 + 1];
				base.transform.position = vector2 * num4 + vector * (1f - num4);
				if (vector != vector2)
				{
					base.transform.LookAt(vector2);
				}
			}
		}
	}

	public void InitBlow()
	{
		this.Blow(base.transform.position);
	}

	protected void Blow(Vector3 position)
	{
		if (this.control)
		{
			NetworkManager.Instance.RegisterShotBefore();
			Shot shot = new Shot(position, this.shot.Direction, (byte)this.shot.WeaponType);
			shot.TimeStamp = this.shot.TimeStamp;
			ShotCalculator.RadialShot(this.player, shot, false);
			NetworkManager.Instance.RegisterShotAfter();
			NetworkManager.Instance.SendShot(shot);
			this.player.UnregisterItem(base.TimeStamp);
			this.Destroy();
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
		base.transform.FindChild("Grenade").gameObject.GetComponent<Renderer>().enabled = false;
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter particleEmitter in componentsInChildren)
		{
			particleEmitter.emit = false;
		}
		UnityEngine.Object.Destroy(base.gameObject, (float)this.destroyDelay / 1000f);
	}

	private const float ActivateRadius = 6f;

	private Shot shot;

	public float flatSpeed = 15f;

	private Vector3 Speed;

	public int launchDelay = 200;

	public int destroyDelay = 300;

	private long landingTime;

	private long lastScanTime;

	private int life = 30000;

	private bool active;

	private bool control;

	private CombatPlayer player;
}
