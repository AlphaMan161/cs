// dnSpy decompiler from Assembly-CSharp.dll class: TurretMachineGunTracer
using System;
using UnityEngine;

public class TurretMachineGunTracer : TurretTracer
{
	private void Start()
	{
		if (this.head != null)
		{
			return;
		}
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
		this.head = base.transform.FindChild("Head");
	}

	public override void Launch(Shot shot, CombatPlayer player, bool control)
	{
		if (this.head == null)
		{
			this.Start();
		}
		this.shot = shot;
		this.launchTime = shot.TimeStamp;
		this.landingTime = shot.LandingTimeStamp;
		this.fireTime = shot.TimeStamp + 2000L;
		base.transform.position = shot.Origin;
		base.transform.LookAt(shot.Origin + shot.Direction);
		base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x + 90f, base.transform.localEulerAngles.y, base.transform.localEulerAngles.z);
		this.setVisible(false);
		this.control = control;
		this.player = player;
		if (control)
		{
			Transform transform = PlayerManager.Instance.LocalPlayer.transform;
			Vector3 lookTarget = transform.position + transform.TransformDirection(new Vector3(0f, 0f, 1000f));
			this.SetLookTarget(lookTarget, false);
			this.sendLookTargetShot();
		}
	}

	public override void setVisible(bool visible)
	{
		MeshRenderer[] componentsInChildren = base.transform.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			meshRenderer.enabled = visible;
		}
	}

	protected override void Shot(Vector3 position, int targetID)
	{
		if (!this.control)
		{
			return;
		}
		if (!this.isTargetTracked)
		{
			if (this.isPlayerTarget && this.fire())
			{
				this.sendLookTargetShot();
			}
			return;
		}
		if (this.fire())
		{
			Ray cray = new Ray(this.head.position, this.head.TransformDirection(new Vector3(0f, 0f, 1f)));
			Shot shot = ShotCalculator.StraightShot(cray, this.head, 102, 0, false);
			ShotCalculator.GunShot(PlayerManager.Instance.LocalPlayer, shot);
			shot.TimeStamp = this.shot.TimeStamp;
			shot.LaunchMode = LaunchModes.TURRET_SHOT;
			NetworkManager.Instance.SendShot(shot);
			EffectManager.Instance.turretMachineGunShotEffect(shot, PlayerManager.Instance.LocalPlayer, ShotEffectType.ME_BEFORE_SERVER);
			UnityEngine.Debug.Log("[Turret] Send SHOT.");
		}
	}

	private void sendLookTargetShot()
	{
		Ray ray = new Ray(this.head.position + this.head.TransformDirection(new Vector3(0f, 0f, 1f)), this.head.TransformDirection(new Vector3(0f, 0f, 1f)));
		Shot shot = new Shot(ray.origin, ray.direction, 102);
		shot.TimeStamp = this.shot.TimeStamp;
		shot.LaunchMode = LaunchModes.TURRET_CONTROL;
		NetworkManager.Instance.SendShot(shot);
		UnityEngine.Debug.Log("[Turret] Send CONTROL.");
	}

	private float ScanForEnemies(Ray weaponRay)
	{
		float num = 180f;
		float num2 = 10000f;
		UnityEngine.Object obj = this.lookTargetLock;
		float result;
		lock (obj)
		{
			Vector3 position = this.currentLookTarget;
			foreach (CombatPlayer combatPlayer in PlayerManager.Instance.Players.Values)
			{
				if (!combatPlayer.IsDead)
				{
					if (base.Owner.Team == 0 || combatPlayer.Team != base.Owner.Team)
					{
						if (combatPlayer != PlayerManager.Instance.LocalPlayer)
						{
							Vector3 from = combatPlayer.transform.position - weaponRay.origin;
							float num3 = Mathf.Abs(TurretMachineGunTracer.ClampAngle180(Mathf.Abs(Vector3.Angle(from, weaponRay.direction)), -180f, 180f));
							if (num3 < num)
							{
								Ray ray = new Ray(weaponRay.origin, combatPlayer.transform.position - weaponRay.origin);
								int num4 = ShotCalculator.StraightScan(ray, true);
								if (num4 != -1)
								{
									num = num3;
									num2 = from.magnitude;
									position = combatPlayer.transform.position;
								}
							}
						}
					}
				}
			}
			if (num > 90f || num2 > 100f)
			{
				this.isPlayerTarget = false;
				this.currentLookTargetDistance = 0f;
				result = num;
			}
			else
			{
				this.currentLookTargetDistance = num2;
				this.currentLookTarget = position;
				this.isPlayerTarget = true;
				result = num;
			}
		}
		return result;
	}

	private void LateUpdate()
	{
		if (this.control)
		{
			float num = this.ScanForEnemies(new Ray(this.head.position, this.head.TransformDirection(new Vector3(0f, 0f, 1f))));
			if (num < 30f && this.isPlayerTarget)
			{
				this.isTargetTracked = true;
			}
			else
			{
				this.isTargetTracked = false;
			}
		}
	}

	private void Update()
	{
		if (this.control)
		{
			bool flag = this.RotateUpdateMedium(0f);
		}
		else if (!this.isPlayerTarget || !this.RotateUpdateMedium(0f))
		{
		}
	}

	public void SetLookTarget(Vector3 lookTarget, bool isPlayerTarget)
	{
		UnityEngine.Object obj = this.lookTargetLock;
		lock (obj)
		{
			this.currentLookTarget = lookTarget;
			this.isPlayerTarget = isPlayerTarget;
			if (!isPlayerTarget)
			{
				this.RotateToLookTarget(lookTarget);
			}
		}
	}

	private void RotateToLookTarget(Vector3 lookTarget)
	{
		this.head.LookAt(lookTarget);
		float num = this.head.localEulerAngles.x;
		float num2 = this.head.localEulerAngles.y;
		this.minimumX = -20f;
		this.maximumX = 25f;
		this.minimumY = -360f;
		this.maximumY = 360f;
		num = TurretMachineGunTracer.ClampAngle180(num, this.minimumX, this.maximumX);
		num2 = TurretMachineGunTracer.ClampAngle180(num2, this.minimumY, this.maximumY);
		Vector3 localEulerAngles = new Vector3(num, num2, 0f);
		this.head.localEulerAngles = localEulerAngles;
	}

	private bool RotateUpdateMedium(float incl)
	{
		UnityEngine.Object obj = this.lookTargetLock;
		bool result;
		lock (obj)
		{
			Vector3 localEulerAngles = this.head.localEulerAngles;
			Vector3 newRotation = localEulerAngles;
			if (this.isPlayerTarget)
			{
				Vector3 worldPosition = this.currentLookTarget;
				this.head.LookAt(worldPosition);
				float num = this.head.localEulerAngles.x;
				float num2 = this.head.localEulerAngles.y;
				this.minimumX = -20f;
				this.maximumX = 25f;
				this.minimumY = -360f;
				this.maximumY = 360f;
				num = TurretMachineGunTracer.ClampAngle180(num, this.minimumX, this.maximumX);
				num2 = TurretMachineGunTracer.ClampAngle180(num2, this.minimumY, this.maximumY);
				newRotation = new Vector3(num, num2, 0f);
			}
			result = this.RotateToX(localEulerAngles, newRotation);
		}
		return result;
	}

	private bool RotateToX(Vector3 currentRotation, Vector3 newRotation)
	{
		float num = this.AngleDifference(newRotation.x, currentRotation.x) + this.AngleDifference(newRotation.y, currentRotation.y);
		float num2 = Mathf.Abs(1.75f / num);
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		if (num2 < 0.01f)
		{
			return false;
		}
		this.head.localEulerAngles = this.Clerp(currentRotation, newRotation, num2);
		return true;
	}

	public static float ClampAngle180(float angle, float min, float max)
	{
		while (angle > 180f)
		{
			angle -= 360f;
		}
		while (angle < -180f)
		{
			angle += 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	private float AngleDifference(float newAngle, float currentAngle)
	{
		float num = newAngle - currentAngle;
		if (num > 180f)
		{
			num = currentAngle + 360f - newAngle;
		}
		if (num < -180f)
		{
			num = currentAngle - 360f - newAngle;
		}
		return num;
	}

	private Vector3 Clerp(Vector3 start, Vector3 finish, float t)
	{
		Vector3 result = start * (1f - t) + finish * t;
		if (Mathf.Abs(finish.x - start.x) > Mathf.Abs(360f + Mathf.Min(finish.x, start.x) - Mathf.Max(finish.x, start.x)))
		{
			if (finish.x > start.x)
			{
				result.x = (360f + start.x) * (1f - t) + finish.x * t;
			}
			else
			{
				result.x = start.x * (1f - t) + (360f + finish.x) * t;
			}
			if (result.x > 360f)
			{
				result.x -= 360f;
			}
		}
		if (Mathf.Abs(finish.y - start.y) > Mathf.Abs(360f + Mathf.Min(finish.y, start.y) - Mathf.Max(finish.y, start.y)))
		{
			if (finish.y > start.y)
			{
				result.y = (360f + start.y) * (1f - t) + finish.y * t;
			}
			else
			{
				result.y = start.y * (1f - t) + (360f + finish.y) * t;
			}
			if (result.y > 360f)
			{
				result.y -= 360f;
			}
		}
		if (Mathf.Abs(finish.z - start.z) > Mathf.Abs(360f + Mathf.Min(finish.z, start.z) - Mathf.Max(finish.z, start.z)))
		{
			if (finish.z > start.z)
			{
				result.z = (360f + start.z) * (1f - t) + finish.z * t;
			}
			else
			{
				result.z = start.z * (1f - t) + (360f + finish.z) * t;
			}
			if (result.z > 360f)
			{
				result.z -= 360f;
			}
		}
		return result;
	}

	private float currentLookTargetDistance;

	private Vector3 currentLookTarget;

	private bool isPlayerTarget;

	private Transform head;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float minimumY = -13f;

	public float maximumY = 25f;

	private bool isTargetTracked;

	private UnityEngine.Object lookTargetLock = new UnityEngine.Object();
}
