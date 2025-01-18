// dnSpy decompiler from Assembly-CSharp.dll class: WalkController
using System;
using UnityEngine;

public class WalkController : MonoBehaviour
{
	public float RotationX
	{
		get
		{
			return this.rotationX;
		}
		set
		{
			this.rotationX = value;
		}
	}

	public float RotationY
	{
		get
		{
			return this.rotationY;
		}
		set
		{
			this.rotationY = value;
		}
	}

	public int EnhacerModes
	{
		get
		{
			return this.enhancerModes;
		}
	}

	public void SetEnhancerMode(EnhancerMode mode)
	{
		this.enhancerModes |= (int)mode;
	}

	public void UnsetEnhancerMode(EnhancerMode mode)
	{
		this.enhancerModes ^= (int)mode;
	}

	private void Awake()
	{
	}

	public void setSpeed(int speed, int jump)
	{
		this.intSpeed = speed;
		this.SpeedInc = (float)speed / 1100f;
		this._SpeedInc = 6f * this.SpeedInc;
		this.SpeedRIncMax = this.SpeedRInc * 30f;
	}

	public void setSpeed(int speed)
	{
		this.intSpeed = speed;
		this.SpeedInc = (float)speed / 1100f;
		this._SpeedInc = 6f * this.SpeedInc;
		this.SpeedRIncMax = this.SpeedRInc * 30f;
	}

	public int getIntSpeed()
	{
		return this.intSpeed;
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

	private void Start()
	{
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
	}

	private void Update()
	{
	}

	public float VolumeLevel = 50f;

	public float VolumeCoefficient = 50f;

	private float volumeCoefficient = 0.02f;

	public float PitchCoefficient = 50f;

	private float pitchCoefficient = 0.02f;

	public float PitchLevel = 50f;

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

	private static readonly float JumpMax = 9f;

	private readonly float JumpDenominator = 5f / WalkController.JumpMax;

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

	public WalkController.RotationAxes axes = WalkController.RotationAxes.MouseX;

	public float sensitivityX = 15f;

	public float sensitivityY = 15f;

	public float minimumX = -18f;

	public float maximumX = 24f;

	public float minimumY = -360f;

	public float maximumY = 360f;

	public float JumpEnergy;

	public float JumpEnergyMax = 2f;

	private float rotationX;

	private float rotationY;

	private Vector3 explosionForce = new Vector3(1f, 2f, 3f);

	private int controlMode = 1;

	public float screenWidth = 989f;

	private int enhancerModes;

	private int intSpeed;

	private bool paused;

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
