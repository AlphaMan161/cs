// dnSpy decompiler from Assembly-CSharp.dll class: TripleRocketTracer
using System;
using UnityEngine;

public class TripleRocketTracer : RocketTracer
{
	private void Start()
	{
		this.rocket = base.transform.FindChild("Rocket").gameObject;
		this.trailRenderer = this.rocket.transform.GetComponent<TrailRenderer>();
		this.trailWidth = this.trailRenderer.startWidth;
		this.trailMaterial = this.trailRenderer.material;
		UnityEngine.Object.Destroy(this.trailRenderer);
	}

	public void Launch(Shot shot, CombatPlayer player, bool control, float rocketIndex)
	{
		this.shot = shot;
		this.start = shot.StartOrigin;
		this.launchTime = shot.TimeStamp + (long)((int)rocketIndex);
		this.landingTime = shot.LandingTimeStamp;
		base.transform.LookAt(shot.Origin);
		this.setRocketVisible(false);
		this.control = control;
		this.player = player;
		this.rocketIndex = rocketIndex;
	}

	public new void Launch(Shot shot)
	{
		this.shot = shot;
		this.start = shot.StartOrigin;
		this.launchTime = shot.TimeStamp;
		this.landingTime = shot.LandingTimeStamp;
		base.transform.LookAt(shot.Origin);
		this.setRocketVisible(false);
	}

	public new void setRocketVisible(bool visible)
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
			float num2 = (float)num / (float)(this.landingTime - this.launchTime);
			base.transform.position = this.start * num2 + this.shot.Origin * (1f - num2);
			float num3 = (float)num / 100f + this.rocketIndex * 2f * 3.14159274f / 3f;
			Vector3 localPosition = this.rocket.transform.localPosition;
			Vector3 vector = new Vector3(0f, 0f, num3 * 180f / 3.14159274f);
			localPosition.x = Mathf.Cos(num3) * 1.5f;
			localPosition.y = Mathf.Sin(num3) * 1.5f;
			this.rocket.transform.localPosition = localPosition;
			if (this.trailRenderer == null && num > 200L)
			{
				this.trailRenderer = this.rocket.AddComponent<TrailRenderer>();
				TrailRenderer trailRenderer = this.trailRenderer;
				float num4 = this.trailWidth;
				this.trailRenderer.endWidth = num4;
				trailRenderer.startWidth = num4;
				this.trailRenderer.material = this.trailMaterial;
			}
			if (this.lastScanTime < TimeManager.Instance.NetworkTime + 100L && this.control)
			{
				this.lastScanTime = TimeManager.Instance.NetworkTime;
				int num5 = PlayerManager.Instance.Scan(base.transform.position, 4f);
				if (num5 > 0)
				{
					this.Blow(base.transform.position);
					return;
				}
			}
		}
	}

	protected new void Blow(Vector3 position)
	{
		if (this.control)
		{
			Shot shot = new Shot(position, this.shot.Direction, (byte)this.shot.WeaponType);
			shot.TimeStamp = this.shot.TimeStamp;
			ShotCalculator.RadialShot(this.player, shot, false);
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

	private GameObject rocket;

	private TrailRenderer trailRenderer;

	private Material trailMaterial;

	private float trailWidth;
}
