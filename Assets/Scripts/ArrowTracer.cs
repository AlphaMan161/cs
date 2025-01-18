// dnSpy decompiler from Assembly-CSharp.dll class: ArrowTracer
using System;
using UnityEngine;

public class ArrowTracer : MonoBehaviour
{
	public Vector3 Target
	{
		set
		{
			this.target = value;
			Vector3 vector = this.target - base.transform.position;
			this.initDir = vector;
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
				if (this.destroy)
				{
					base.gameObject.SetActive(false);
					UnityEngine.Object.Destroy(base.gameObject);
					return;
				}
				Vector3 normalized = this.initDir.normalized;
				normalized.Scale(new Vector3(this.deepToTarget, this.deepToTarget, this.deepToTarget));
				base.transform.position = this.target - normalized;
				UnityEngine.Object.Destroy(this);
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
				if (this.destroy)
				{
					base.gameObject.SetActive(false);
					UnityEngine.Object.Destroy(base.gameObject);
					return;
				}
				Vector3 normalized = this.initDir.normalized;
				normalized.Scale(new Vector3(this.deepToTarget, this.deepToTarget, this.deepToTarget));
				base.transform.position = this.target - normalized;
				UnityEngine.Object.Destroy(this);
			}
			this.lastDist = sqrMagnitude;
		}
	}

	public Vector3 target = Vector3.zero;

	public Vector3 initDir = Vector3.zero;

	private TrailRenderer trail;

	public float flatSpeed = 150f;

	public Vector3 Speed = Vector3.zero;

	private bool active = true;

	private float flatSpeedSqr = 300f;

	private float deepToTarget = 2f;

	public bool destroy;

	private float lastDist;
}
