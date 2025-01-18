// dnSpy decompiler from Assembly-CSharp.dll class: RocketTracer
using System;
using UnityEngine;

public class RocketTracer : ItemTracer
{
	private void Start()
	{
		this.weaponType = WeaponType.ROCKET_LAUNCHER;
	}

	public void Launch(Shot shot, CombatPlayer player, bool control)
	{
		this.shot = shot;
		this.start = shot.StartOrigin;
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
		this.start = shot.StartOrigin;
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
      //  Debug.Log(componentsInChildren2.Length);
        componentsInChildren2[0].sharedMaterial.shader = Shader.Find("Particles/Alpha Blended");
        componentsInChildren2[1].sharedMaterial.shader = Shader.Find("Mobile/Diffuse"); //3 rocket
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
			if (this.shot != null)
			{
				this.Blow(this.shot.Origin);
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
				this.Blow(this.shot.Origin);
				return;
			}
			float num2 = (float)num / (float)(this.landingTime - this.launchTime);
			base.transform.position = this.start * num2 + this.shot.Origin * (1f - num2);
			if ((this.lastScanTime < TimeManager.Instance.NetworkTime + 100L || this.lastScanTime == 0L) && this.control)
			{
				this.lastScanTime = TimeManager.Instance.NetworkTime;
				int num3 = PlayerManager.Instance.Scan(base.transform.position, 4f);
				if (num3 > 0)
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
		base.transform.FindChild("Rocket").gameObject.GetComponent<Renderer>().enabled = false;
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter particleEmitter in componentsInChildren)
		{
			particleEmitter.emit = false;
		}
		UnityEngine.Object.Destroy(base.gameObject, (float)this.destroyDelay / 1000f);
	}

	protected Vector3 start;

	protected Shot shot;

	public float flatSpeed = 15f;

	protected Vector3 Speed;

	public int launchDelay = 200;

	public int destroyDelay = 600;

	protected long landingTime;

	protected long lastScanTime;

	protected bool active;

	protected bool control;

	protected CombatPlayer player;

	protected float rocketIndex;
}
