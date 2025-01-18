// dnSpy decompiler from Assembly-CSharp.dll class: ServerNavigationController
using System;
using UnityEngine;

public class ServerNavigationController : MonoBehaviour
{
	public long InterpolationBackTime
	{
		set
		{
			this.interpolationBackTime = value;
		}
	}

	public void StartReceiving()
	{
		this.running = true;
	}

	public Vector3 InterpolatedSpeed
	{
		get
		{
			return this.interpolatedSpeed;
		}
	}

	public float InterpolatedRotationSpeed
	{
		get
		{
			return this.interpolatedRotationSpeed;
		}
	}

	public void ReceiveTransform(NetworkTransform ntransform)
	{
		if (!this.running)
		{
			return;
		}
		for (int i = this.bufferedStates.Length - 1; i >= 1; i--)
		{
			this.bufferedStates[i] = this.bufferedStates[i - 1];
		}
		this.bufferedStates[0] = ntransform;
		this.statesCount = Mathf.Min(this.statesCount + 1, this.bufferedStates.Length);
		for (int j = 0; j < this.statesCount - 1; j++)
		{
			if (this.bufferedStates[j].TimeStamp < this.bufferedStates[j + 1].TimeStamp)
			{
			}
		}
	}

	public void ReceiveTransform(NetworkTransform ntransform, bool follow)
	{
		if (!this.running)
		{
			return;
		}
		for (int i = this.bufferedStates.Length - 1; i >= 1; i--)
		{
			this.bufferedStates[i] = this.bufferedStates[i - 1];
		}
		object obj = this.positionLock;
		lock (obj)
		{
			if (this.statesCount > 1)
			{
				Vector3 position = base.transform.position;
				Vector3 localEulerAngles = base.transform.localEulerAngles;
				base.transform.position = this.bufferedStates[1].Position;
				base.transform.LookAt(ntransform.Position);
				ntransform.Speed = base.transform.localEulerAngles;
				base.transform.position = position;
				base.transform.localEulerAngles = localEulerAngles;
			}
			else
			{
				ntransform.Speed = base.transform.localEulerAngles;
			}
		}
		this.bufferedStates[0] = ntransform;
		this.statesCount = Mathf.Min(this.statesCount + 1, this.bufferedStates.Length);
	}

	private void Update()
	{
		if (!this.running)
		{
			return;
		}
		if (this.statesCount == 0)
		{
			return;
		}
		if (TimeManager.Instance == null)
		{
			return;
		}
		long networkTime = TimeManager.Instance.NetworkTime;
		long num = networkTime - this.interpolationBackTime;
		float magnitude2;
		if (this.mode == ServerNavigationController.InterpolationMode.BEZIER_LINEAR_IN_EX && this.bufferedStates[0].TimeStamp > num)
		{
			for (int i = 0; i < this.statesCount; i++)
			{
				if (this.bufferedStates[i].TimeStamp <= num || i == this.statesCount - 1)
				{
					NetworkTransform networkTransform = this.bufferedStates[Mathf.Max(i - 1, 0)];
					NetworkTransform networkTransform2 = this.bufferedStates[i];
					object obj = this.positionLock;
					lock (obj)
					{
						float num2 = (float)(networkTransform.TimeStamp - networkTransform2.TimeStamp);
						float num3 = 0f;
						if ((double)num2 > 0.0001)
						{
							num3 = (float)(num - networkTransform2.TimeStamp) / num2;
						}
						Vector3 vector = Vector3.Lerp(networkTransform2.Position, networkTransform.Position, num3);
						Vector3 vector2 = vector - base.transform.position;
						if (vector2.magnitude > this.maxSpeed)
						{
							Vector3 vector3 = vector2 - this.interpolatedSpeed;
							float magnitude = vector3.magnitude;
							if (magnitude > this.maxAcceleration)
							{
								vector3 *= this.maxAcceleration / magnitude;
							}
							this.interpolatedSpeed += vector3;
						}
						magnitude2 = this.interpolatedSpeed.magnitude;
						if (magnitude2 > this.maxSpeed)
						{
							this.interpolatedSpeed = vector2.normalized * this.maxSpeed;
						}
						else
						{
							this.interpolatedSpeed = vector2.normalized * magnitude2;
						}
						base.transform.position += this.interpolatedSpeed;
						if (!this.IgnoreAngles)
						{
							Vector3 vector4 = this.Clerp(networkTransform2.Speed, networkTransform.Speed, num3);
							Transform transform = base.transform.Find("SShip");
							if (transform != null)
							{
								this.interpolatedRotationSpeed = this.AngleDifference(vector4.y, base.transform.localEulerAngles.y);
								base.transform.localEulerAngles = this.RotateTo(base.transform.localEulerAngles, new Vector3(0f, vector4.y, 0f), 0.01f);
								Quaternion identity = Quaternion.identity;
								Vector3 vector5 = base.transform.InverseTransformDirection(this.interpolatedSpeed);
								this.speedRX = (this.speedRX * 9f + 100f * vector5.x / 7f + this.interpolatedRotationSpeed * 3f) / 10f;
								this.speedRZ = (this.speedRZ * 9f + 100f * vector5.z / 7f) / 10f;
								identity.eulerAngles = new Vector3(-this.speedRZ, 0f, -this.speedRX);
								transform.localRotation = identity;
							}
							else
							{
								this.interpolatedRotationSpeed = this.AngleDifference(vector4.y, base.transform.localEulerAngles.y);
								base.transform.localEulerAngles = this.RotateTo(base.transform.localEulerAngles, new Vector3(0f, vector4.y, 0f), 0.01f);
							}
						}
					}
					return;
				}
			}
		}
		if (this.interpolatedSpeed == Vector3.zero)
		{
			return;
		}
		this.interpolatedSpeed *= 0.95f;
		base.transform.position += this.interpolatedSpeed;
		magnitude2 = this.interpolatedSpeed.magnitude;
		if (magnitude2 < 0.001f)
		{
			this.interpolatedSpeed = Vector3.zero;
			return;
		}
	}

	private Vector3 RotateTo(Vector3 currentRotation, Vector3 newRotation, float maxAngleSpeed)
	{
		float num = Mathf.Abs(this.AngleDifference(newRotation.y, currentRotation.y));
		if (num > maxAngleSpeed)
		{
			num = maxAngleSpeed;
		}
		return this.Clerp(currentRotation, newRotation, num);
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

	private void UpdateValues()
	{
		double num = (double)TimeManager.Instance.AveragePing;
		if (num < 50.0)
		{
			this.interpolationBackTime = 50L;
		}
		else if (num < 100.0)
		{
			this.interpolationBackTime = 100L;
		}
		else if (num < 200.0)
		{
			this.interpolationBackTime = 200L;
		}
		else if (num < 400.0)
		{
			this.interpolationBackTime = 400L;
		}
		else if (num < 600.0)
		{
			this.interpolationBackTime = 600L;
		}
		else
		{
			this.interpolationBackTime = 1000L;
		}
	}

	public void Reset()
	{
		this.statesCount = 0;
	}

	public ServerNavigationController.InterpolationMode mode = ServerNavigationController.InterpolationMode.BEZIER_LINEAR_IN_EX;

	public bool IgnoreAngles;

	public float speed;

	public float maxSpeed = 4f;

	public float maxAcceleration = 0.001f;

	private long interpolationBackTime = 700L;

	private long extrapolationForwardTime = 400L;

	private bool running;

	private NetworkTransform[] bufferedStates = new NetworkTransform[20];

	private int statesCount;

	private Vector3 interpolatedSpeed = new Vector3(0f, 0f, 0f);

	private float interpolatedRotationSpeed;

	private float speedRX;

	private float speedRZ;

	private object positionLock = new object();

	public enum InterpolationMode
	{
		INTERPOLATION,
		EXTRAPOLATION,
		BEZIER_LINEAR_IN_EX,
		BEZIER_CUBIC_IN_EX
	}
}
