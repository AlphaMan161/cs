// ILSpyBased#2
using System;
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY,
        MouseX,
        MouseY
    }

    public RotationAxes axes;

    public float sensitivityX = 15f;

    public float sensitivityY = 15f;

    public float minimumX = -360f;

    public float maximumX = 360f;

    public float minimumY = -60f;

    public float maximumY = 60f;

    private float rotationX;

    private float rotationY;

    public Transform target;

    private long targetLookTime;

    private Quaternion originalRotation;

    private new Camera camera;

    public Transform Target
    {
        get
        {
            return this.target;
        }
        set
        {
            this.target = value;
            this.targetLookTime = DateTime.Now.Ticks / 10000 + 5000;
        }
    }

    private void Update()
    {
        if ((UnityEngine.Object)this.Target != (UnityEngine.Object)null)
        {
            float magnitude = (this.Target.position - base.transform.position).magnitude;
            base.transform.LookAt(this.Target.position + new Vector3(0f, 4f, 0f));
            if (magnitude < 5f)
            {
                this.camera.fov = 120f;
            }
            else
            {
                this.camera.fov = 600f / magnitude;
            }
            long num = DateTime.Now.Ticks / 10000;
            if (num > this.targetLookTime)
            {
                this.Target = null;
                this.camera.fov = 60f;
            }
        }
        else if (Screen.lockCursor)
        {
            this.camera.fov = 60f;
            if (this.axes == RotationAxes.MouseXAndY)
            {
                int num2 = 1;
                if (OptionsManager.InvertMouseY)
                {
                    num2 = -1;
                }
                this.rotationX += UnityEngine.Input.GetAxis("Mouse X") * this.sensitivityX;
                this.rotationY += UnityEngine.Input.GetAxis("Mouse Y") * this.sensitivityY * (float)num2;
                this.rotationX = MouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
                this.rotationY = MouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
                Quaternion rhs = Quaternion.AngleAxis(this.rotationX, Vector3.up);
                Quaternion rhs2 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
                base.transform.localRotation = this.originalRotation * rhs * rhs2;
            }
            else if (this.axes == RotationAxes.MouseX)
            {
                this.rotationX += UnityEngine.Input.GetAxis("Mouse X") * this.sensitivityX;
                this.rotationX = MouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
                Quaternion rhs3 = Quaternion.AngleAxis(this.rotationX, Vector3.up);
                base.transform.localRotation = this.originalRotation * rhs3;
            }
            else
            {
                this.rotationY += UnityEngine.Input.GetAxis("Mouse Y") * this.sensitivityY;
                this.rotationY = MouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
                Quaternion rhs4 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
                base.transform.localRotation = this.originalRotation * rhs4;
            }
        }
    }

    private void Start()
    {
        this.camera = base.GetComponent<Camera>();
        if ((bool)base.GetComponent<Rigidbody>())
        {
            base.GetComponent<Rigidbody>().freezeRotation = true;
        }
        this.originalRotation = base.transform.localRotation;
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
}


