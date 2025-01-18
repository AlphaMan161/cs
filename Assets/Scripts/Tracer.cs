// dnSpy decompiler from Assembly-CSharp.dll class: Tracer
using System;
using UnityEngine;

public class Tracer : MonoBehaviour
{
	public Vector3 Target
	{
		set
		{
			this.target = value;
			Vector3 vector = this.target - base.transform.position;
			vector.Normalize();
			vector.x *= this.flatSpeed;
			vector.y *= this.flatSpeed;
			vector.z *= this.flatSpeed;
			this.Speed.x = vector.x;
			this.Speed.y = vector.y;
			this.Speed.z = vector.z;
			vector = this.target - base.transform.position;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < this.flatSpeedSqr * 5f || this.Speed == Vector3.zero)
			{
				base.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this.lastDist = sqrMagnitude;
		}
	}

	public TrailRenderer Trail
	{
		get
		{
			if (this.trail == null)
			{
				this.trail = base.GetComponent<TrailRenderer>();
			}
			return this.trail;
		}
	}

	public float FlatSpeed
	{
		set
		{
			this.flatSpeed = value;
			this.flatSpeedSqr = this.flatSpeed * this.flatSpeed;
		}
	}

	private void Start()
	{
		this.flatSpeed = 1f;
		this.flatSpeedSqr = this.flatSpeed * this.flatSpeed;
	}

	private void FixedUpdate()
	{
		if (this.target != Vector3.zero)
		{
			base.transform.position = base.transform.position + this.Speed;
			float sqrMagnitude = (this.target - base.transform.position).sqrMagnitude;
			if (sqrMagnitude < this.flatSpeedSqr || sqrMagnitude > this.lastDist)
			{
				base.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this.lastDist = sqrMagnitude;
		}
	}

	public Vector3 target = Vector3.zero;

	private TrailRenderer trail;

	public float flatSpeed = 150f;

	public Vector3 Speed = Vector3.zero;

	private bool active = true;

	private float flatSpeedSqr = 300f;

	private float lastDist;
}
