// dnSpy decompiler from Assembly-CSharp.dll class: SoldierCamera
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierCamera : MonoBehaviour
{
	private void Start()
	{
		this.cShakeTimes = 0;
		this.cShake = 0f;
		this.cShakeSpeed = this.shakeSpeed;
		this.soldier = base.transform;
		Vector3 eulerAngles = this.camTransform.eulerAngles;
		this.x = eulerAngles.y;
		this.y = eulerAngles.x;
		this.originalSoldierRotation = this.soldier.rotation;
		this.targetDistance = this.normalDistance;
		this.cPos = this.soldier.position + new Vector3(0f, this.normalHeight, 0f);
	}

	private void GoToOrbitMode(bool state)
	{
		this.orbit = state;
		this.soldierController.idleTimer = 0f;
	}

	public void ResetRotation(float horizontal, float vertical)
	{
		this.y = vertical;
		this.x = horizontal;
	}

	private void Update()
	{
		if (this.orbit && (UnityEngine.Input.GetKeyDown(KeyCode.O) || UnityEngine.Input.GetAxis("Horizontal") != 0f || UnityEngine.Input.GetAxis("Vertical") != 0f || this.soldierController.aim || this.soldierController.fire))
		{
			this.GoToOrbitMode(false);
		}
		if (!this.orbit && this.soldierController.idleTimer > 0.1f)
		{
			this.GoToOrbitMode(true);
		}
		this.deltaTime = Time.deltaTime;
		this.LateUpdate2();
	}

	private void LateUpdate2()
	{
		this.GetInput();
		this.RotateSoldier();
	}

	private void CameraMovement()
	{
		if (this.soldierController.aim)
		{
			if (base.GetComponent<Camera>() == null)
			{
				return;
			}
			base.GetComponent<Camera>().fieldOfView = Mathf.Lerp(base.GetComponent<Camera>().fieldOfView, this.zoomFOV, this.deltaTime * this.lerpSpeed);
			if (this.soldierController.crouch)
			{
				this.camDir = this.aimCrouchDirection.x * this.soldier.forward + this.aimCrouchDirection.z * this.soldier.right;
				this.targetHeight = this.crouchAimHeight;
				this.targetDistance = this.crouchAimDistance;
			}
			else
			{
				this.camDir = this.aimDirection.x * this.soldier.forward + this.aimDirection.z * this.soldier.right;
				this.targetHeight = this.normalAimHeight;
				this.targetDistance = this.normalAimDistance;
			}
		}
		else if (this.soldierController.crouch)
		{
			this.camDir = this.crouchDirection.x * this.soldier.forward + this.crouchDirection.z * this.soldier.right;
			this.targetHeight = this.crouchHeight;
			this.targetDistance = this.crouchDistance;
		}
		else
		{
			this.camDir = this.normalDirection.x * this.soldier.forward + this.normalDirection.z * this.soldier.right;
			this.targetHeight = this.normalHeight;
			this.targetDistance = this.normalDistance;
		}
		this.camDir = this.camDir.normalized;
		this.HandleCameraShake();
		this.cPos = this.soldier.position + new Vector3(0f, this.targetHeight, 0f);
		RaycastHit raycastHit;
		if (Physics.Raycast(this.cPos, this.camDir, out raycastHit, this.targetDistance + 0.2f, this.hitLayer))
		{
			float num = raycastHit.distance - 0.1f;
			num -= this.minDistance;
			num /= this.targetDistance - this.minDistance;
			this.targetHeight = Mathf.Lerp(this.maxHeight, this.targetHeight, Mathf.Clamp(num, 0f, 1f));
			this.cPos = this.soldier.position + new Vector3(0f, this.targetHeight, 0f);
		}
		if (Physics.Raycast(this.cPos, this.camDir, out raycastHit, this.targetDistance + 0.2f, this.hitLayer))
		{
			this.targetDistance = raycastHit.distance - 0.1f;
		}
		if (this.radar != null)
		{
			this.radar.position = this.cPos;
			this.radarCamera.rotation = Quaternion.Euler(90f, this.x, 0f);
		}
		Vector3 vector = this.cPos;
		vector += this.soldier.right * Vector3.Dot(this.camDir * this.targetDistance, this.soldier.right);
		this.camTransform.position = this.cPos + this.camDir * this.targetDistance;
		this.camTransform.LookAt(vector);
	}

	private void HandleCameraShake()
	{
		if (this.shake)
		{
			this.cShake += this.cShakeSpeed * this.deltaTime;
			if (Mathf.Abs(this.cShake) > this.cShakePos)
			{
				this.cShakeSpeed *= -1f;
				this.cShakeTimes++;
				if (this.cShakeTimes >= this.shakeTimes)
				{
					this.shake = false;
				}
				if (this.cShake > 0f)
				{
					this.cShake = this.maxShake;
				}
				else
				{
					this.cShake = -this.maxShake;
				}
			}
			this.targetHeight += this.cShake;
		}
	}

	private void StartShake(float distance)
	{
		float num = distance / this.maxShakeDistance;
		if ((double)num > 1.0)
		{
			return;
		}
		num = Mathf.Clamp(num, 0f, 1f);
		num = 1f - num;
		this.cShakeSpeed = Mathf.Lerp(this.minShakeSpeed, this.maxShakeSpeed, num);
		this.shakeTimes = (int)Mathf.Lerp((float)this.minShakeTimes, (float)this.maxShakeTimes, num);
		this.cShakeTimes = 0;
		this.cShakePos = Mathf.Lerp(this.minShake, this.maxShake, num);
		this.shake = true;
	}

	private void GetInput()
	{
		switch (Configuration.MouseMode)
		{
		case CameraOrbitMode.Neutral:
			this.GetInputNeutral();
			break;
		case CameraOrbitMode.Normal:
			this.GetInputNormal();
			break;
		case CameraOrbitMode.Smooth:
			this.GetInputSmooth();
			break;
		}
	}

	public void RotateCameraMode()
	{
		switch (Configuration.MouseMode)
		{
		case CameraOrbitMode.Neutral:
			Configuration.MouseMode = CameraOrbitMode.Normal;
			break;
		case CameraOrbitMode.Normal:
			Configuration.MouseMode = CameraOrbitMode.Smooth;
			break;
		case CameraOrbitMode.Smooth:
			Configuration.MouseMode = CameraOrbitMode.Neutral;
			break;
		}
	}

	private void GetInputNormal()
	{
		Vector2 vector = (!this.soldierController.aim) ? this.speed : this.aimSpeed;
		this.x += UnityEngine.Input.GetAxis("Mouse X") * Time.deltaTime * OptionsManager.MouseSensityX * this.zoomFactor;
		this.y -= UnityEngine.Input.GetAxis("Mouse Y") * Time.deltaTime * OptionsManager.MouseSensityX * this.zoomFactor;
		this.y = SoldierCamera.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
	}

	private void GetInputNeutral()
	{
		Vector2 vector = (!this.soldierController.aim) ? this.speed : this.aimSpeed;
		float num = 0.02f;
		this.x += UnityEngine.Input.GetAxis("Mouse X") * OptionsManager.MouseSensityX * num * this.zoomFactor;
		this.y -= UnityEngine.Input.GetAxis("Mouse Y") * OptionsManager.MouseSensityX * num * this.zoomFactor;
		this.y = SoldierCamera.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
	}

	private void GetInputSmooth()
	{
		this.rotAverageY = 0f;
		this.rotAverageX = 0f;
		float num = 0.01f;
		this.rotationY += UnityEngine.Input.GetAxis("Mouse Y") * OptionsManager.MouseSensityX * num * this.zoomFactor;
		this.rotationX += UnityEngine.Input.GetAxis("Mouse X") * OptionsManager.MouseSensityX * num * this.zoomFactor;
		this.rotArrayY.Add(this.rotationY);
		this.rotArrayX.Add(this.rotationX);
		if ((float)this.rotArrayY.Count >= this.frameCounter)
		{
			this.rotArrayY.RemoveAt(0);
		}
		if ((float)this.rotArrayX.Count >= this.frameCounter)
		{
			this.rotArrayX.RemoveAt(0);
		}
		for (int i = 0; i < this.rotArrayY.Count; i++)
		{
			this.rotAverageY += this.rotArrayY[i];
		}
		for (int j = 0; j < this.rotArrayX.Count; j++)
		{
			this.rotAverageX += this.rotArrayX[j];
		}
		this.rotAverageY /= (float)this.rotArrayY.Count;
		this.rotAverageX /= (float)this.rotArrayX.Count;
		this.rotAverageY = SoldierCamera.ClampAngle(this.rotAverageY, this.yMinLimit, this.yMaxLimit);
		this.rotAverageX = SoldierCamera.ClampAngle(this.rotAverageX, this.minimumX, this.maximumX);
		this.x = this.rotAverageX;
		this.y = -this.rotAverageY;
	}

	private void RotateSoldier()
	{
		this.soldierController.targetYRotation = this.x;
		this.soldierController.targetXRotation = this.y;
		Transform transform = this.soldierController.transform;
		transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, this.x, transform.localRotation.eulerAngles.z);
		FPSCamera component = this.FPSCamera.GetComponent<FPSCamera>();
		if (component != null)
		{
			component.FPSRotation = new Vector3(this.y, 0f, 0f);
		}
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public Transform soldier;

	public Vector2 speed = new Vector2(135f, 135f);

	public Vector2 aimSpeed = new Vector2(70f, 70f);

	public Vector2 maxSpeed = new Vector2(100f, 100f);

	public float yMinLimit = -90f;

	public float yMaxLimit = 90f;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float normalFOV = 60f;

	public float zoomFOV = 30f;

	public float lerpSpeed = 8f;

	private float distance = 10f;

	private float x;

	public float y;

	public Transform camTransform;

	private Quaternion rotation;

	private Vector3 position;

	private float deltaTime;

	private Quaternion originalSoldierRotation;

	public SoldierController soldierController;

	public bool orbit;

	public LayerMask hitLayer;

	private Vector3 cPos;

	public Vector3 normalDirection;

	public Vector3 aimDirection;

	public Vector3 crouchDirection;

	public Vector3 aimCrouchDirection;

	public float positionLerp;

	public float normalHeight;

	public float crouchHeight;

	public float normalAimHeight;

	public float crouchAimHeight;

	public float minHeight;

	public float maxHeight;

	public float normalDistance;

	public float crouchDistance;

	public float normalAimDistance;

	public float crouchAimDistance;

	public float minDistance;

	public float maxDistance;

	private float targetDistance;

	private Vector3 camDir;

	private float targetHeight;

	public float minShakeSpeed;

	public float maxShakeSpeed;

	public float minShake;

	public float maxShake = 2f;

	public int minShakeTimes;

	public int maxShakeTimes;

	public float maxShakeDistance;

	private bool shake;

	private float shakeSpeed = 2f;

	private float cShakePos;

	private int shakeTimes = 8;

	private float cShake;

	private float cShakeSpeed;

	private int cShakeTimes;

	public Transform radar;

	public Transform radarCamera;

	public Transform FPSCamera;

	public float zoomFactor = 1f;

	private List<float> rotArrayX = new List<float>();

	private float rotAverageX;

	private List<float> rotArrayY = new List<float>();

	private float rotAverageY;

	private float rotationX;

	private float rotationY;

	public float frameCounter = 20f;
}
