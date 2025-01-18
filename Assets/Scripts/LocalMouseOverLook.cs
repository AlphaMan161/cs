// dnSpy decompiler from Assembly-CSharp.dll class: LocalMouseOverLook
using System;
using UnityEngine;

[AddComponentMenu("Camera-Control/Local Mouse Over Look")]
public class LocalMouseOverLook : MonoBehaviour
{
	public Vector3 OriginalPosition
	{
		get
		{
			return this.originalPosition;
		}
	}

	private void Start()
	{
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}
		this.originalPosition.y = this.minY;
		this.originalPosition.z = this.minZ;
		this.originalRotation = base.transform.localRotation;
		this.originalPosition = base.transform.localPosition;
	}

	private void Update()
	{
		float min = this.minimumY * 2f;
		float max = this.maximumY * 2f;
		if (LocalShotController.Instance.IsZoomAllowed)
		{
			if (UnityEngine.Input.GetKeyDown(TRInput.Zoom))
			{
				UnityEngine.Debug.Log("Zoom GetKeyDown");
				if (this.zoomFactor == 1f)
				{
					this.zoomFactor = 0.1f;
					LocalGameHUD.Instance.Zoom(true);
				}
				else
				{
					this.zoomFactor = 1f;
					LocalGameHUD.Instance.Zoom(false);
				}
			}
		}
		else
		{
			this.zoomFactor = 1f;
		}
		Camera component = base.GetComponent<Camera>();
		component.fov = 55f * this.zoomFactor;
		if (Screen.lockCursor)
		{
			float num = 0f;
			float num2 = 0f;
			if (UnityEngine.Input.GetKeyDown(TRInput.RollIn))
			{
				num = 1f;
			}
			if (UnityEngine.Input.GetKeyDown(TRInput.RollOut))
			{
				num = -1f;
			}
			float num3 = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * 10f + num - num2;
			if (this.originalPosition.z + num3 < this.minZ + this.maxZ && this.originalPosition.z + num3 > this.minZ - this.maxZ)
			{
				this.originalPosition.z = this.originalPosition.z + num3;
				if (this.originalPosition.z < this.minZ)
				{
					this.originalPosition.y = this.originalPosition.y - num3 * 3f / 10f;
				}
				else
				{
					this.originalPosition.y = this.minY;
				}
			}
			float num4 = 1f;
			if (Screen.fullScreen)
			{
				num4 = this.screenWidth / (float)Screen.currentResolution.width;
				num4 = 1f;
			}
			if (base.transform.parent == null)
			{
				int num5 = 1;
				if (OptionsManager.InvertMouseY)
				{
					num5 = -1;
				}
				this.rotationX += UnityEngine.Input.GetAxis("Mouse X") * this.sensitivityX * Time.deltaTime * num4;
				this.rotationY += UnityEngine.Input.GetAxis("Mouse Y") * this.sensitivityY * (float)num5 * Time.deltaTime * num4;
				this.rotationX = LocalMouseOverLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
				this.rotationY = LocalMouseOverLook.ClampAngle(this.rotationY, min, max);
				Quaternion rhs = Quaternion.AngleAxis(this.rotationX, Vector3.up);
				Quaternion rhs2 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
				base.transform.localRotation = this.originalRotation * rhs * rhs2;
			}
			else
			{
				this.rotationY += UnityEngine.Input.GetAxis("Mouse Y") * OptionsManager.MouseSensityX * Time.deltaTime * this.zoomFactor * num4;
				this.rotationY = LocalMouseOverLook.ClampAngle(this.rotationY, min, max);
				Quaternion rhs3 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
				base.transform.localRotation = this.originalRotation * rhs3;
				base.transform.position = base.transform.parent.position + base.transform.TransformDirection(this.originalPosition);
			}
		}
		if (base.transform.parent)
		{
			this.Intersect();
		}
		if (PlayerManager.Instance != null && PlayerManager.Instance.water)
		{
			if ((double)base.transform.position.y < -12.5)
			{
				RenderSettings.fog = true;
				RenderSettings.fogColor = new Color(0.164705887f, 0.423529416f, 0.2784314f);
				RenderSettings.fogDensity = 0.02f;
			}
			else
			{
				RenderSettings.fog = true;
				RenderSettings.fogColor = new Color(0.5137255f, 0.6313726f, 0.7490196f);
				RenderSettings.fogDensity = 0.001f;
			}
		}
	}

	private void Intersect()
	{
		Vector3 direction = base.transform.TransformDirection(this.originalPosition) - new Vector3(0f, 1.5f, 0f);
		Vector3 vector = base.transform.parent.position + new Vector3(0f, 1.5f, 0f);
		Ray ray = new Ray(vector, direction);
		int num = 768;
		num = ~num;
		RaycastHit raycastHit;
		Physics.Raycast(ray, out raycastHit, direction.magnitude, num);
		if (raycastHit.transform)
		{
			Vector3 b = vector - raycastHit.point;
			b.Normalize();
			b.Scale(new Vector3(1f, 1f, 1f));
			base.transform.position = raycastHit.point + b;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
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

	public LocalMouseOverLook.RotationAxes axes;

	public float sensitivityX = 15f;

	public float sensitivityY = 15f;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float minimumY = -60f;

	public float maximumY = 60f;

	private float rotationX;

	private float rotationY;

	public float zoomFactor = 1f;

	public float maxZ = 11f;

	public float minZ = -8f;

	public float minY = 2f;

	private Quaternion originalRotation;

	private Vector3 originalPosition;

	public float screenWidth = 989f;

	public enum RotationAxes
	{
		MouseXAndY,
		MouseX,
		MouseY
	}
}
