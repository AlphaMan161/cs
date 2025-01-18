// dnSpy decompiler from Assembly-CSharp.dll class: MonsterBotNavigationController
using System;
using UnityEngine;

public class MonsterBotNavigationController : MonoBehaviour
{
	public Trajectory Trajectory
	{
		get
		{
			if (this.trajectory == null)
			{
				this.trajectory = new Trajectory();
			}
			return this.trajectory;
		}
	}

	public Vector3 MoveDirection
	{
		get
		{
			return this.moveDirection;
		}
	}

	private void Awake()
	{
		this.audio = base.transform.GetComponent<AudioSource>();
	}

	private void Start()
	{
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}
		this.moveDirection = base.transform.TransformPoint(new Vector3(0f, 0f, 1f)) - base.transform.position;
	}

	public void setSpeed(int speed)
	{
		this.speed = (float)speed;
	}

	public float Speed
	{
		get
		{
			return this.speed;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		while (angle < -360f)
		{
			angle += 360f;
		}
		while (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public void setExplosionForce(Vector3 ef, float power)
	{
		this.explosionForce = base.transform.position;
		this.explosionForce -= ef;
		this.explosionForce.Normalize();
		this.explosionForce = this.explosionForce * power * this.explosionSpeed;
	}

	private void FixedUpdate()
	{
		long networkTime = TimeManager.Instance.NetworkTime;
		if (this.launchTime != 0L && this.launchTime < networkTime)
		{
			this.RotateUpdateMedium(0f);
			float num = this.WalkUpdateMedium(networkTime);
		}
	}

	public void Launch(long launchTime, long landingTime)
	{
		this.launchTime = launchTime;
		float trajectoryLength = this.trajectory.TrajectoryLength;
		this.landingTime = landingTime;
	}

	private float WalkUpdateMedium(long time)
	{
		if (this.Trajectory.Nodes.Count <= 1)
		{
			return 0f;
		}
		float num = (float)((double)(time - this.launchTime) / (double)(this.landingTime - this.launchTime));
		if (num >= 1f)
		{
			return 0f;
		}
		Vector3 position = this.trajectory.GetPosition(num);
		this.moveDirection = position - base.transform.position;
		base.transform.position = position;
		return 0f;
	}

	private void RotateUpdateMedium(float incl)
	{
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		Vector3 worldPosition = base.transform.position + this.moveDirection;
		base.transform.LookAt(worldPosition);
		Vector3 newRotation = new Vector3(0f, base.transform.localEulerAngles.y, 0f);
		this.RotateTo(localEulerAngles, newRotation);
	}

	private void RotateTo(Vector3 currentRotation, Vector3 newRotation)
	{
		float num = this.AngleDifference(newRotation.y, currentRotation.y);
		float num2 = Mathf.Abs(1.75f / num);
		if (num2 > 1f)
		{
			num2 = num;
		}
		base.transform.localEulerAngles = this.Clerp(currentRotation, newRotation, num2);
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
		if (Mathf.Abs(finish.x - start.x) > Math.Abs(360f + Mathf.Min(finish.x, start.x) - Mathf.Max(finish.x, start.x)))
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
		if (Mathf.Abs(finish.y - start.y) > Math.Abs(360f + Mathf.Min(finish.y, start.y) - Mathf.Max(finish.y, start.y)))
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
		if (Mathf.Abs(finish.z - start.z) > Math.Abs(360f + Mathf.Min(finish.z, start.z) - Mathf.Max(finish.z, start.z)))
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

	public void Blow()
	{
		Shot shot = new Shot(base.transform.position, new Vector3(0f, 1f, 0f), 104);
		shot.TimeStamp = this.launchTime;
		EffectManager.Instance.launcherEffect(shot, PlayerManager.Instance.LocalPlayer, PlayerManager.Instance.LocalPlayer, true);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private float speed = 0.4f;

	private Vector3 explosionForce = new Vector3(0f, 0f, 0f);

	private float explosionSpeed = 1f;

	private Trajectory trajectory;

	private new AudioSource audio;

	private long launchTime;

	private long landingTime;

	private Vector3 moveDirection;
}
