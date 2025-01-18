// ILSpyBased#2
using UnityEngine;

[AddComponentMenu("Camera-Control/FPS Camera")]
public class FPSCamera : MonoBehaviour
{
    public float CroachHeight = 4.7f;

    public float CroachWalkHeight = 5.2f;

    public float NormalHeight = 6.5f;

    private float targetHeight = 6.5f;

    public float Rapidity = 12f;

    private bool croach;

    private bool walk;

    private bool moving;

    private float error = 0.05f;

    public float zoomFactor = 1f;

    private float shakeMinAcc = 0.05f;

    private float shakeThreshold = 0.01f;

    private float shakeMaxAcc = 0.2f;

    private float shakeAccMultiplier = 0.1f;

    public bool isZooming;

    private Camera mainCamera;

    private Camera weaponCamera;

    private SoldierCamera soldierCamera;

    private SoldierController soldierController;

    private Vector3 fpsRotation = new Vector3(0f, 0f, 0f);

    private Vector3 shakeRotation = new Vector3(0f, 0f, 0f);

    private bool shaking;

    public float SniperZoomFactor
    {
        get
        {
            return ShotController.Instance.GetSniperZoomFactor();
        }
    }

    public bool Croach
    {
        get
        {
            return this.croach;
        }
        set
        {
            if (this.croach != value)
            {
                this.croach = value;
                this.refreshState();
            }
        }
    }

    public bool Walk
    {
        get
        {
            return this.walk;
        }
        set
        {
            if (this.walk != value)
            {
                this.walk = value;
                this.refreshState();
            }
        }
    }

    public Vector3 FPSRotation
    {
        get
        {
            return this.fpsRotation;
        }
        set
        {
            this.fpsRotation = value;
            base.transform.localEulerAngles = this.fpsRotation + this.shakeRotation;
        }
    }

    public void ProccessShaking()
    {
        float num = this.shakeRotation.magnitude * this.shakeAccMultiplier * Time.deltaTime * 50f;
        Vector3 b = this.shakeRotation.normalized * num;
        Vector3 vector = this.shakeRotation - b;
        if (vector.magnitude <= num || vector.magnitude < this.shakeThreshold)
        {
            this.shakeRotation = new Vector3(0f, 0f, 0f);
            this.shaking = false;
        }
        else
        {
            this.shakeRotation = vector;
        }
    }

    public void Shake(Vector3 vector)
    {
        this.Shake(vector, false);
    }

    public void Shake(Vector3 vector, bool AdditionalShake)
    {
        if (AdditionalShake)
        {
            this.shakeRotation += vector;
        }
        else
        {
            this.shakeRotation = vector;
        }
        this.shaking = true;
    }

    private void refreshState()
    {
        if (this.croach && this.walk)
        {
            this.targetHeight = this.CroachWalkHeight;
        }
        else if (this.croach)
        {
            this.targetHeight = this.CroachHeight;
        }
        else
        {
            this.targetHeight = this.NormalHeight;
        }
        this.moving = true;
    }

    private void Start()
    {
        Camera[] componentsInChildren = base.GetComponentsInChildren<Camera>();
        Camera[] array = componentsInChildren;
        foreach (Camera camera in array)
        {
            if (camera.gameObject.name == "MainCamera")
            {
                this.mainCamera = camera;
            }
            else if (camera.gameObject.name == "WeaponCamera")
            {
                this.weaponCamera = camera;
                this.weaponCamera.gameObject.SetActive(true);
            }
        }
        this.soldierCamera = ((Component)base.transform.parent).GetComponent<SoldierCamera>();
        this.soldierController = ((Component)base.transform.parent).GetComponent<SoldierController>();
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
        if (!((Object)this.mainCamera == (Object)null) && !((Object)this.weaponCamera == (Object)null))
        {
            if (this.shaking)
            {
                this.ProccessShaking();
            }
            if (ShotController.Instance.Zoom)
            {
                goto IL_0043;
            }
            goto IL_0043;
        }
        return;
        IL_0043:
        if (ShotController.Instance.zoomAllowed())
        {
            if (UnityEngine.Input.GetKeyDown(TRInput.Zoom))
            {
                this.Zoom(!this.isZooming);
                this.isZooming = !this.isZooming;
            }
            if (ShotController.Instance.IsBlocked)
            {
                if (this.isZooming)
                {
                    this.Zoom(false);
                }
            }
            else if (this.isZooming)
            {
                this.Zoom(true);
            }
        }
        else if (this.isZooming)
        {
            this.Zoom(false);
            this.isZooming = false;
        }
        if (this.moving)
        {
            Vector3 localPosition = base.transform.localPosition;
            float y = localPosition.y;
            float num = this.targetHeight - y;
            float num2 = num * this.Rapidity * Time.deltaTime;
            if (Mathf.Abs(num2) < this.error)
            {
                num2 = this.error * Mathf.Sign(num2);
            }
            y += num2;
            if (Mathf.Abs(y - this.targetHeight) <= this.error)
            {
                y = this.targetHeight;
                this.moving = false;
            }
            else
            {
                Transform transform = base.transform;
                Vector3 localPosition2 = base.transform.localPosition;
                float x = localPosition2.x;
                float y2 = y;
                Vector3 localPosition3 = base.transform.localPosition;
                transform.localPosition = new Vector3(x, y2, localPosition3.z);
            }
        }
    }

    private void Zoom(bool on)
    {
        if (on)
        {
            if (this.zoomFactor != this.SniperZoomFactor)
            {
                this.zoomFactor = this.SniperZoomFactor;
                goto IL_005c;
            }
        }
        else if (this.zoomFactor != 1f)
        {
            this.zoomFactor = 1f;
            this.weaponCamera.fov = 60f * this.zoomFactor;
            goto IL_005c;
        }
        return;
        IL_005c:
        ShotController.Instance.Zoom = on;
        if (this.SniperZoomFactor < 0.5f || !on)
        {
            if (this.weaponCamera.gameObject.active == on)
            {
                this.weaponCamera.gameObject.active = !on;
            }
        }
        else
        {
            this.weaponCamera.fov = 60f * this.zoomFactor;
        }
        this.soldierCamera.zoomFactor = this.zoomFactor;
        this.soldierController.zoomFactor = this.zoomFactor;
        this.mainCamera.fov = 55f * this.zoomFactor;
    }
}


