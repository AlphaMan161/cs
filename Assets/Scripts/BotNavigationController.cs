// dnSpy decompiler from Assembly-CSharp.dll class: BotNavigationController
using System;
using UnityEngine;

public class BotNavigationController : MonoBehaviour
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
		this.sship = base.transform.Find("SShip");
		this.ljet = this.sship.Find("LJet");
		this.rjet = this.sship.Find("RJet");
		this.bljet = this.sship.Find("BLJet");
		this.brjet = this.sship.Find("BRJet");
		this.ljetlight = this.sship.Find("LJetLight").GetComponent<Light>();
		this.rjetlight = this.sship.Find("RJetLight").GetComponent<Light>();
		this.controller = base.GetComponent<CharacterController>();
		this.jetaudio = base.transform.GetComponentInChildren<Camera>().transform.FindChild("JetAudio").GetComponent<AudioSource>();
		switch (OptionsManager.ShipType)
		{
		case 0:
		case 3:
			this.explosionSpeed = 1.09090912f;
			this.SpeedInc = 1.36363626f;
			this.SpeedFric = 0.0400000028f;
			this.SpeedRInc = 0.2f;
			this.SpeedRFric = 0.2f;
			this._SpeedInc = 6f * this.SpeedInc;
			this._SpeedFric = 8f * this.SpeedFric;
			this._SpeedRInc = 0.4f;
			this._SpeedRFric = 0.3f;
			break;
		case 1:
			this.explosionSpeed = 0.818181753f;
			this.SpeedInc = 1.13636363f;
			this.SpeedFric = 0.0400000028f;
			this.SpeedRInc = 0.175f;
			this.SpeedRFric = 0.166666672f;
			this._SpeedInc = 6f * this.SpeedInc;
			this._SpeedFric = 8f * this.SpeedFric;
			this._SpeedRInc = 0.4f;
			this._SpeedRFric = 0.3f;
			break;
		case 2:
			this.explosionSpeed = 0.363636345f;
			this.SpeedInc = 0.9090909f;
			this.SpeedFric = 0.0400000028f;
			this.SpeedRInc = 0.15f;
			this.SpeedRFric = 0.142857149f;
			this._SpeedInc = 6f * this.SpeedInc;
			this._SpeedFric = 8f * this.SpeedFric;
			this._SpeedRInc = 0.4f;
			this._SpeedRFric = 0.3f;
			break;
		}
	}

	private void Start()
	{
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}
		this.originalRotation = base.transform.localRotation;
		this.shotController = base.transform.GetComponent<BotShotController>();
		this.moveDirection = base.transform.TransformPoint(new Vector3(0f, 0f, 1f)) - base.transform.position;
	}

	public void setSpeed(int speed, int jump)
	{
		this.SpeedInc = (float)speed / 1100f;
		this._SpeedInc = 6f * this.SpeedInc;
		this.SpeedRIncMax = this.SpeedRInc * 30f;
	}

	public void NextMode()
	{
		this.controlMode++;
		if (this.controlMode > 1)
		{
			this.controlMode = 0;
		}
		MonoBehaviour.print("Switch ControlMode: " + this.controlMode);
	}

	public float Speed
	{
		get
		{
			return this.speed;
		}
	}

	public Vector3 getSpeed()
	{
		return this.Speed3;
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
		float num = this.WalkUpdateMedium();
		this.RotateUpdateMedium(0f);
	}

	private float WalkUpdateMedium()
	{
		if (this.Trajectory.Nodes.Count <= 1)
		{
			return 0f;
		}
		if (this.shotController.IsPlayerTarget)
		{
			this.speed -= 0.025f;
			if (this.speed < 0.1f)
			{
				this.speed = 0.1f;
			}
		}
		else
		{
			this.speed += 0.025f;
			if (this.speed > 0.5f)
			{
				this.speed = 0.5f;
			}
		}
		Vector3 vector = this.trajectory.Move(base.transform.position, this.speed);
		this.moveDirection = vector - base.transform.position;
		base.transform.position = vector;
		return 0f;
	}

	private void RotateUpdateMedium(float incl)
	{
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		Vector3 worldPosition = base.transform.position + this.shotController.CurrentLookTarget;
		if (this.shotController.IsPlayerTarget)
		{
			worldPosition = this.shotController.CurrentLookTarget;
		}
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

	private float _SpeedInc = 1f;

	private float _SpeedFric = 0.0285714287f;

	private float _SpeedRInc = 1f;

	private float _SpeedRFric = 0.0333333351f;

	private float explosionSpeed = 1f;

	private float SpeedRInc = 1f;

	private float SpeedInc = 1f;

	private float SpeedRIncMax = 30f;

	private float SpeedFric = 0.0285714287f;

	private float SpeedRFric = 0.0333333351f;

	private readonly float GlideMax = 1.1f;

	private readonly float JumpMax = 9f;

	private float speed;

	private float Tim;

	private float speedR;

	private float SpeedR;

	private float SpeedRX;

	private float SpeedRZ;

	private float rotsp;

	private Vector3 Speed3 = Vector3.zero;

	public Vector3 SpeedVector = new Vector3(0f, 0f, 0f);

	private AudioSource jetaudio;

	private AudioSource effaudio;

	private CharacterController controller;

	private CapsuleCollider altimeter;

	private Transform sship;

	private Transform ljet;

	private Transform rjet;

	private Transform bljet;

	private Transform brjet;

	private Light ljetlight;

	private Light rjetlight;

	private bool _jump;

	private new MouseOverLook camera;

	private Quaternion originalRotation;

	public BotNavigationController.RotationAxes axes = BotNavigationController.RotationAxes.MouseX;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float JumpEnergy;

	public float JumpEnergyMax = 2f;

	private float rotationX;

	private Vector3 explosionForce = new Vector3(1f, 2f, 3f);

	private int controlMode = 1;

	private Trajectory trajectory;

	private Vector3 moveDirection;

	private BotShotController shotController;

	public enum RotationAxes
	{
		MouseXAndY,
		MouseX,
		MouseY
	}

	public enum ControlMode
	{
		Hard,
		Medium,
		Light
	}

	public enum MouseMode
	{
		Wide,
		Narrow
	}
}
