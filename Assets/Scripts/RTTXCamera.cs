// ILSpyBased#2
using System;
using UnityEngine;

[AddComponentMenu("Camera-Control/RTTX Camera")]
public class RTTXCamera : MonoBehaviour
{
    public const long ReadPeriod = 5000000L;

    public const long StartPeriod = 100000000L;

    private long lastRead = DateTime.Now.Ticks;

    private long startTime = DateTime.Now.Ticks;

    private Color aveColor = new Color(0f, 0f, 0f);

    private static RTTXCamera Instance;

    public Rect RECT = new Rect(-50f, -50f, 100f, 100f);

    public Vector2 DESTINATION = new Vector2(0f, 0f);

    public Color AveColor
    {
        get
        {
            return this.aveColor;
        }
    }

    private void Start()
    {
        RTTXCamera.Instance = this;
    }

    private void OnPostRender()
    {
        long ticks = DateTime.Now.Ticks;
        if (this.lastRead + 5000000 <= ticks)
        {
            this.lastRead = ticks;
            Texture2D texture2D = new Texture2D((int)this.RECT.width, (int)this.RECT.height, TextureFormat.RGB24, false);
            texture2D.ReadPixels(new Rect((float)(Screen.width / 2) - this.RECT.left, (float)(Screen.height / 2) - this.RECT.top, this.RECT.width, this.RECT.height), (int)this.DESTINATION.x, (int)this.DESTINATION.y);
            if (Configuration.DebugEnableRTTX)
            {
                texture2D.Apply();
            }
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            float num4 = 0f;
            int x = (int)(this.RECT.width * 0.2f);
            for (int i = 2; i <= (int)this.RECT.height - 2; i += (int)(this.RECT.height * 0.25f))
            {
                Color pixel = texture2D.GetPixel(x, i);
                num += pixel.r;
                num2 += pixel.g;
                num3 += pixel.b;
                num4 += 1f;
            }
            Vector3 a = new Vector3(num / num4, num2 / num4, num3 / num4);
            num4 = 0f;
            num3 = 0f; num = (num2 = (num3 ));
            x = (int)(this.RECT.width * 0.8f);
            for (int j = 2; j <= (int)this.RECT.height - 2; j += (int)(this.RECT.height * 0.25f))
            {
                Color pixel2 = texture2D.GetPixel(x, j);
                num += pixel2.r;
                num2 += pixel2.g;
                num3 += pixel2.b;
                num4 += 1f;
            }
            Vector3 b = new Vector3(num / num4, num2 / num4, num3 / num4);
            Vector3 vector = a - b;
            if (vector.sqrMagnitude > 0.005f)
            {
                if ((UnityEngine.Object)PlayerManager.Instance == (UnityEngine.Object)null)
                {
                    NetworkDev.CheckAim = true;
                    this.DestroyRTTX();
                }
                else
                {
                    PlayerManager.Instance.SendEnterBaseRequest(17);
                    if (Configuration.DebugEnableRTTX)
                    {
                        UnityEngine.Debug.Log("WALL HACK DETECTED!" + DateTime.Now.Second);
                        GameHUDFPS.Instance.SetDebugLine(string.Format("WH DETECTED: {0} {1}", vector.sqrMagnitude, "(0.005f : oo)"), 1);
                    }
                    PlayerManager.Instance.SetCameraTexture(texture2D);
                }
            }
            else
            {
                Vector3 vector2 = (a + b) * 0.5f;
                if (vector2.x / vector2.z < 0.4f || vector2.x / vector2.z > 10f)
                {
                    PlayerManager.Instance.SendEnterBaseRequest(17);
                    if (Configuration.DebugEnableRTTX)
                    {
                        UnityEngine.Debug.Log("WALL HACK DETECTED!" + DateTime.Now.Second);
                        GameHUDFPS.Instance.SetDebugLine(string.Format("WH DETECTED: {0} {1}", vector2.x / vector2.z, "(0.4f : 10f)"), 1);
                    }
                }
                if (Configuration.DebugEnableRTTX)
                {
                    GameHUDFPS.Instance.SetDebugLine(string.Format("WH monitor: {0} {1} {2} {3}", vector.sqrMagnitude, "(0.005f : oo)", vector2.x / vector2.z, "(0.4f : 10f)"), 1);
                    PlayerManager.Instance.SetCameraTexture(texture2D);
                }
                if ((UnityEngine.Object)PlayerManager.Instance == (UnityEngine.Object)null)
                {
                    this.DestroyRTTX();
                }
            }
        }
    }

    private void DestroyRTTX()
    {
        if (base.transform.parent.name.StartsWith("RTTX"))
        {
            UnityEngine.Object.Destroy(base.transform.parent.gameObject);
        }
        else
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    private void FixedUpdate()
    {
        long ticks = DateTime.Now.Ticks;
        if (this.startTime + 100000000 < ticks)
        {
            this.DestroyRTTX();
            RTTXCamera.Instance = null;
        }
    }

    public static void Init()
    {
        if ((UnityEngine.Object)RTTXCamera.Instance == (UnityEngine.Object)null)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(CharacterManager.Instance.GetPrefab("RTTX"));
        }
        else
        {
            RTTXCamera.Instance.Prolong();
        }
    }

    public static void SetPosition(Vector3 pos)
    {
        RTTXCamera.Instance.transform.position = pos;
    }

    public static void TestCameraRTTX(bool rttx)
    {
        Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
        Camera[] array2 = array;
        foreach (Camera camera in array2)
        {
            camera.enabled = false;
        }
        Camera component = ((Component)RTTXCamera.Instance.transform).GetComponent<Camera>();
        component.enabled = true;
    }

    public static void TestCamera(bool rttx)
    {
        Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
        Camera[] array2 = array;
        foreach (Camera camera in array2)
        {
            camera.enabled = false;
        }
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        UnityEngine.Object.Destroy(gameObject.GetComponent<Renderer>());
        UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
        Camera camera2 = gameObject.AddComponent<Camera>();
        if (rttx)
        {
            camera2.transform.position = new Vector3(19.7f, -57.3f, 274f);
        }
        else
        {
            camera2.transform.position = new Vector3(2001.948f, -1999.813f, 1991.838f);
        }
        camera2.enabled = true;
        camera2.clearFlags = CameraClearFlags.Skybox;
        camera2.nearClipPlane = 0.1f;
        camera2.farClipPlane = 300f;
    }

    public void Prolong()
    {
        this.startTime = DateTime.Now.Ticks;
    }
}


