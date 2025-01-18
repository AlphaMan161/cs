// dnSpy decompiler from Assembly-CSharp.dll class: GrenadeTracer
using System;
using UnityEngine;

public class GrenadeTracer : ItemTracer
{
	public CombatPlayer OwnerPlayer
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
		base.transform.LookAt(shot.Origin);
		this.setRocketVisible(false);
		this.control = control;
		this.player = player;
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
        foreach (Renderer renderer in componentsInChildren2)
		{
			renderer.enabled = visible;
		}
        Renderer component = base.gameObject.GetComponent<Renderer>();
        //Debug.Log(componentsInChildren2.Length);
        componentsInChildren2[2].sharedMaterial.shader = Shader.Find("Particles/Additive");
        component.sharedMaterial.shader = Shader.Find("Particles/~Additive-Multiply");
        if (gameObject.name.StartsWith("SnowballPrefab")) {
            ParticleRenderer[] componentsInChildren3 = base.transform.GetComponentsInChildren<ParticleRenderer>();
            componentsInChildren3[0].sharedMaterial.shader = Shader.Find("Particles/Additive");
        }
    }

	private void FixedUpdate()
	{
		if (TimeManager.Instance.NetworkTime >= this.launchTime && TimeManager.Instance.NetworkTime < this.landingTime)
		{
			this.active = true;
			this.setRocketVisible(true);
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
				if (this.shot != null)
				{
					this.Blow(this.shot.Origin);
				}
				else
				{
					this.Destroy();
				}
			}
			else
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
			if ((this.lastScanTime < TimeManager.Instance.NetworkTime + 100L || this.lastScanTime == 0L) && this.control)
			{
				this.lastScanTime = TimeManager.Instance.NetworkTime;
				int num5 = PlayerManager.Instance.Scan(base.transform.position + new Vector3(0f, -3.5f, 0f), 6f);
				if (num5 > 0)
				{
					this.Blow(base.transform.position);
					return;
				}
			}
		}
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

	private bool active;

	private bool control;

	private CombatPlayer player;
}
