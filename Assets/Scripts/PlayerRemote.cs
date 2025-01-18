// dnSpy decompiler from Assembly-CSharp.dll class: PlayerRemote
using System;
using System.Collections;
using UnityEngine;

public class PlayerRemote : MonoBehaviour
{
	public bool IsEnemy
	{
		get
		{
			return this.isEnemy;
		}
		set
		{
			this.isEnemy = value;
			if (!this.isEnemy)
			{
				Transform transform = base.transform.FindChild("Capsule");
				if (transform == null)
				{
					return;
				}
				this.capsule = transform.GetComponent<CapsuleCollider>();
				this.capsule.enabled = false;
			}
		}
	}

	public bool Dead
	{
		get
		{
			return this.dead;
		}
		set
		{
			this.dead = value;
			if (this.capsule == null)
			{
				return;
			}
			if (!this.isEnemy)
			{
				this.capsule.enabled = false;
				return;
			}
			if (this.dead)
			{
				this.capsule.enabled = false;
			}
			else
			{
				this.capsule.enabled = true;
			}
		}
	}

	public bool CrouchStatus
	{
		get
		{
			return this.crouchStatus;
		}
		set
		{
			this.crouchStatus = value;
			if (this.capsule == null)
			{
				return;
			}
			if (this.crouchStatus)
			{
				this.capsule.center = new Vector3(this.capsule.center.x, 2.1f, this.capsule.center.z);
				this.capsule.height = 5.2f;
			}
			else
			{
				this.capsule.center = new Vector3(this.capsule.center.x, 3.5f, this.capsule.center.z);
				this.capsule.height = 7f;
			}
		}
	}

	private void Start()
	{
		this.DeadSpalsh = GameObject.Find("DeadSoldier");
		Transform transform = base.transform.FindChild("Capsule");
		if (transform == null)
		{
			return;
		}
		this.capsule = transform.GetComponent<CapsuleCollider>();
	}

	internal void SetProperties(Hashtable properties)
	{
		if (properties.ContainsKey("N"))
		{
			this.Name = (string)properties["N"];
		}
		if (properties.ContainsKey("W"))
		{
			this.currentWeapon = (byte)properties["W"];
		}
		if (properties.ContainsKey("C"))
		{
			this.CrouchStatus = (bool)properties["C"];
		}
		if (properties.ContainsKey("R"))
		{
			this.InAir = (bool)properties["R"];
		}
		if (properties.ContainsKey("A"))
		{
			this.Aim = (bool)properties["A"];
		}
	}

	public void SetCrouch(bool crouch)
	{
		this.CrouchStatus = crouch;
	}

	public static Vector3 GetPosition(float[] result)
	{
		Vector3 result2 = new Vector3(result[0], result[1], result[2]);
		return result2;
	}

	public static Quaternion GetRotation(float[] result)
	{
		Quaternion result2 = new Quaternion(result[0], result[1], result[2], result[3]);
		return result2;
	}

	internal void SetSpawnPosition(Hashtable evData)
	{
		base.transform.eulerAngles = new Vector3(90f, base.transform.rotation.y, 0f);
		this._DeadSplash = (GameObject)UnityEngine.Object.Instantiate(this.DeadSpalsh, base.transform.position + new Vector3(0f, 1f, 0f), base.transform.rotation);
		UnityEngine.Object.Destroy(this._DeadSplash, 8f);
		Vector3 position = PlayerRemote.GetPosition((float[])evData[43]);
		Quaternion rotation = PlayerRemote.GetRotation((float[])evData[46]);
		this.pos = position;
		base.transform.position = position;
		this.rot = rotation;
		this.realPos = position;
		this.lastUpdateTime = Time.time;
		this.targetpos = PlayerRemote.GetPosition((float[])evData[45]);
		base.SendMessage("SetTargetPos", this.targetpos);
		base.transform.position = this.pos;
		base.transform.localRotation = this.rot;
		this.Dead = false;
	}

	internal void SetPosition(Hashtable evData)
	{
		Vector3 position = PlayerRemote.GetPosition((float[])evData[43]);
		Quaternion rotation = PlayerRemote.GetRotation((float[])evData[46]);
		float num = Time.time - this.lastUpdateTime;
		if (num > 0.2f)
		{
			this.pos = position;
		}
		else
		{
			this.pos = position + position - this.realPos;
		}
		this.rot = rotation;
		this.realPos = position;
		this.lastUpdateTime = Time.time;
		this.targetpos = PlayerRemote.GetPosition((float[])evData[45]);
		base.SendMessage("SetTargetPos", this.targetpos);
	}

	private void Update()
	{
		if (this.lastUpdateTime > 1f)
		{
			this.pos = this.realPos;
			this.lastUpdateTime = Time.time;
		}
	}

	internal void SetAnim(Hashtable evData)
	{
		this.keyState = (KeyState)((byte)evData[49]);
	}

	internal void SetFire(Hashtable evData)
	{
		Hashtable hashtable = new Hashtable();
		if (evData.Contains("reload"))
		{
			this.Reload = (bool)evData["reload"];
		}
		else
		{
			this.Reload = false;
		}
		this.Fire = (bool)evData[53];
		hashtable.Add("Fire", this.Fire);
		hashtable.Add("Weapon", this.currentWeapon);
		hashtable.Add("reload", this.Reload);
		Vector3 position = PlayerRemote.GetPosition(new float[]
		{
			(float)evData[55],
			(float)evData[56],
			(float)evData[57]
		});
		hashtable.Add("Point", position);
		if (evData.Contains(61))
		{
			Vector3 position2 = PlayerRemote.GetPosition(new float[]
			{
				(float)evData[61],
				(float)evData[62],
				(float)evData[63]
			});
			hashtable.Add("OutPutPoint", position2);
		}
	}

	private float lastUpdateTime = -1f;

	private Vector3 realPos;

	public GameObject DeadSpalsh;

	public Vector3 pos = Vector3.zero;

	public Quaternion rot = Quaternion.identity;

	public Vector3 targetpos = Vector3.zero;

	public bool PlayerIsLocal;

	public bool leftright;

	public bool Fire;

	private bool dead = true;

	public Camera TPSCamera;

	public GameObject ResurrectShield;

	private bool isEnemy;

	public byte currentWeapon;

	public bool crouchStatus;

	public bool Aim;

	public bool InAir;

	public bool Walk = true;

	public bool Reload;

	public KeyState keyState;

	public string Name = string.Empty;

	private GameObject _DeadSplash;

	private CapsuleCollider capsule;
}
