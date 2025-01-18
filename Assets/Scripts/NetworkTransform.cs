// dnSpy decompiler from Assembly-CSharp.dll class: NetworkTransform
using System;
using System.Collections;
using UnityEngine;

public class NetworkTransform
{
	private NetworkTransform()
	{
	}

	public Vector3 Position
	{
		get
		{
			return this.position;
		}
		set
		{
			this.position = value;
		}
	}

	public Vector3 Speed
	{
		get
		{
			return this.speed;
		}
		set
		{
			this.speed = value;
		}
	}

	public Vector3 Rotation
	{
		get
		{
			return this.rotation;
		}
		set
		{
			this.rotation = value;
		}
	}

	public long TimeStamp
	{
		get
		{
			return this.timeStamp;
		}
		set
		{
			this.timeStamp = value;
		}
	}

	public bool IsDifferent(Transform transform, float accuracy)
	{
		float num = Vector3.Distance(this.position, transform.position);
		float num2 = Vector3.Distance(this.rotation, transform.localEulerAngles);
		return num > accuracy || num2 > accuracy;
	}

	public Hashtable ToHashtable(bool sendHeight)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)1] = this.position.x;
		hashtable[(byte)2] = this.position.y;
		hashtable[(byte)3] = this.position.z;
		hashtable[(byte)5] = this.speed.y;
		hashtable[(byte)8] = this.timeStamp;
		hashtable[(byte)4] = this.speed.x;
		if (sendHeight)
		{
			hashtable[(byte)6] = this.speed.z;
		}
		return hashtable;
	}

	public Hashtable ToHashtable()
	{
		return this.ToHashtable(false);
	}

	public static Hashtable Vector3ToHashtable(Vector3 vector)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)1] = vector.x;
		hashtable[(byte)2] = vector.y;
		hashtable[(byte)3] = vector.z;
		return hashtable;
	}

	public static NetworkTransform FromHashtable(Hashtable data)
	{
		NetworkTransform networkTransform = new NetworkTransform();
		if (data[(byte)1].GetType() != typeof(float))
		{
			return null;
		}
		if (data[(byte)2].GetType() != typeof(float))
		{
			return null;
		}
		if (data[(byte)3].GetType() != typeof(float))
		{
			return null;
		}
		float num = (float)data[(byte)1];
		float num2 = (float)data[(byte)2];
		float num3 = (float)data[(byte)3];
		if (float.IsNaN(num) || float.IsInfinity(num))
		{
			return null;
		}
		if (float.IsNaN(num2) || float.IsInfinity(num2))
		{
			return null;
		}
		if (float.IsNaN(num3) || float.IsInfinity(num3))
		{
			return null;
		}
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		if (data.ContainsKey((byte)4))
		{
			if (data[(byte)4].GetType() != typeof(float))
			{
				return null;
			}
			num4 = (float)data[(byte)4];
			if (float.IsNaN(num4) || float.IsInfinity(num4))
			{
				return null;
			}
		}
		if (data.ContainsKey((byte)5))
		{
			if (data[(byte)5].GetType() != typeof(float))
			{
				return null;
			}
			num5 = (float)data[(byte)5];
			if (float.IsNaN(num5) || float.IsInfinity(num5))
			{
				return null;
			}
		}
		if (data.ContainsKey((byte)6))
		{
			if (data[(byte)6].GetType() != typeof(float))
			{
				return null;
			}
			num6 = (float)data[(byte)6];
			if (float.IsNaN(num6) || float.IsInfinity(num6))
			{
				return null;
			}
		}
		if (data.ContainsKey((byte)7))
		{
			if (data[(byte)7].GetType() != typeof(float))
			{
				return null;
			}
			num7 = (float)data[(byte)7];
			if (float.IsNaN(num7) || float.IsInfinity(num7))
			{
				return null;
			}
		}
		networkTransform.rotation = new Vector3(0f, num7, 0f);
		networkTransform.speed = new Vector3(num4, num5, num6);
		networkTransform.position = new Vector3(num, num2, num3);
		networkTransform.speed = new Vector3(num4, num5, num6);
		if (data.ContainsKey((byte)8))
		{
			if (data[(byte)8].GetType() != typeof(long))
			{
				return null;
			}
			networkTransform.TimeStamp = (long)data[(byte)8];
		}
		else
		{
			networkTransform.TimeStamp = 0L;
		}
		return networkTransform;
	}

	public void Load(NetworkTransform ntransform)
	{
		this.position = ntransform.position;
		this.speed = ntransform.speed;
		this.rotation = ntransform.rotation;
		this.timeStamp = ntransform.timeStamp;
	}

	public void Update(Transform trans)
	{
		trans.position = this.Position;
		trans.localEulerAngles = this.rotation;
	}

	public static NetworkTransform FromTransform(Transform transform)
	{
		NetworkTransform networkTransform = new NetworkTransform();
		networkTransform.position = transform.position;
		WalkController walkController = (WalkController)transform.GetComponent("WalkController");
		if (walkController != null)
		{
			networkTransform.speed = walkController.getSpeed();
		}
		else
		{
			new Vector3(0f, 0f, 0f);
		}
		networkTransform.rotation = transform.localEulerAngles;
		return networkTransform;
	}

	public static NetworkTransform FromTransform(Transform transform, Transform speedtransform)
	{
		return new NetworkTransform
		{
			position = transform.position,
			speed = speedtransform.position,
			rotation = transform.localEulerAngles
		};
	}

	public static NetworkTransform FromTransform(Transform transform, Quaternion angletransform)
	{
		return new NetworkTransform
		{
			position = transform.position,
			speed = angletransform.eulerAngles
		};
	}

	public static NetworkTransform FromPoint(Vector3 pos, Vector3 angle, Vector3 speed)
	{
		return new NetworkTransform
		{
			position = pos,
			speed = speed,
			rotation = angle
		};
	}

	public static NetworkTransform FromPoint(Vector3 pos, Vector3 speed)
	{
		return new NetworkTransform
		{
			position = pos,
			speed = speed
		};
	}

	public static NetworkTransform Clone(NetworkTransform ntransform)
	{
		NetworkTransform networkTransform = new NetworkTransform();
		networkTransform.Load(ntransform);
		return networkTransform;
	}

	private Vector3 position;

	private Vector3 speed;

	private Vector3 rotation;

	private long timeStamp;
}
