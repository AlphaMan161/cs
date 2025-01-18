// ILSpyBased#2
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Over Look")]
public class MouseOverLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY,
        MouseX,
        MouseY,
        None
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

    public float zoomFactor = 1f;

    public float maxZ = 11f;

    public float minZ = -8f;

    public float minY = 2f;

    public float screenWidth = 989f;

    public bool RotationEnabled = true;

    private Quaternion originalRotation;

    private Vector3 originalPosition;

    private void Start()
    {
        if ((bool)base.GetComponent<Rigidbody>())
        {
            base.GetComponent<Rigidbody>().freezeRotation = true;
        }
        this.originalPosition.y = this.minY;
        this.originalPosition.z = this.minZ;
        this.originalRotation = base.transform.localRotation;
        this.originalPosition = base.transform.localPosition;
        if (Configuration.SType == ServerType.VK)
        {
            this.screenWidth = 827f;
        }
    }

    public void Reset()
    {
        this.originalPosition.y = this.minY;
        this.originalPosition.z = this.minZ;
        this.originalRotation = base.transform.localRotation;
        this.originalPosition = base.transform.localPosition;
        this.rotationX = 0f; this.rotationY = (this.rotationX );
    }

    private void Update()
    {
        float min = this.minimumY * 2f;
        float max = this.maximumY * 2f;
        if (ShotController.Instance.zoomAllowed())
        {
            if (UnityEngine.Input.GetKeyDown(TRInput.Zoom))
            {
                if (this.zoomFactor == 1f)
                {
                    this.zoomFactor = 0.5f;
                    GameHUD.Instance.Zoom(true);
                }
                else
                {
                    this.zoomFactor = 1f;
                    GameHUD.Instance.Zoom(false);
                }
            }
        }
        else
        {
            this.zoomFactor = 1f;
        }
        if (Screen.lockCursor && this.RotationEnabled)
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
                this.originalPosition.z += num3;
                if (this.originalPosition.z < this.minZ)
                {
                    this.originalPosition.y -= num3 * 3f / 10f;
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
            int num5 = 1;
            if (OptionsManager.InvertMouseY)
            {
                num5 = -1;
            }
            if ((Object)base.transform.parent == (Object)null || true)
            {
                this.rotationX += UnityEngine.Input.GetAxis("Mouse X") * this.sensitivityX * Time.deltaTime * num4;
                this.rotationY += UnityEngine.Input.GetAxis("Mouse Y") * this.sensitivityY * (float)num5 * Time.deltaTime * num4;
                this.rotationX = MouseOverLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
                this.rotationY = MouseOverLook.ClampAngle(this.rotationY, min, max);
                Quaternion rhs = Quaternion.AngleAxis(this.rotationX, Vector3.up);
                Quaternion rhs2 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
                base.transform.localRotation = this.originalRotation * rhs * rhs2;
                base.transform.position = base.transform.parent.position + base.transform.TransformDirection(this.originalPosition);
            }
            else
            {
                this.rotationY += UnityEngine.Input.GetAxis("Mouse Y") * OptionsManager.MouseSensityX * Time.deltaTime * this.zoomFactor * num4 * (float)num5;
                this.rotationY = MouseOverLook.ClampAngle(this.rotationY, min, max);
                Quaternion rhs3 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
                base.transform.localRotation = this.originalRotation * rhs3;
                base.transform.position = base.transform.parent.position + base.transform.TransformDirection(this.originalPosition);
            }
        }
        if ((bool)base.transform.parent)
        {
            this.Intersect();
        }
    }

    private void Intersect()
    {
        Vector3 direction = base.transform.TransformDirection(this.originalPosition) - new Vector3(0f, 1.5f, 0f);
        Vector3 vector = base.transform.parent.position + new Vector3(0f, 1.5f, 0f);
        Ray ray = new Ray(vector, direction);
        int num = 1540;
        num = ~num;
        RaycastHit raycastHit = default(RaycastHit);
        Physics.Raycast(ray, out raycastHit, direction.magnitude, num);
        if ((bool)raycastHit.transform)
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
}


