// ILSpyBased#2
using UnityEngine;

public class DoubleLightning : MonoBehaviour
{
    public Transform target;

    public float scale = 1f;

    public float speed = 1f;

    public Light llight;

    private bool recalculateNormals;

    public bool statical;

    private float zxscale = 0.5f;

    private Vector3[] baseVertices;

    private Perlin noise;

    private float start;

    private float end;

    private float length;

    private Mesh mesh;

    private long tm;

    private void Start()
    {
        this.tm = 0L;
        this.noise = new Perlin();
        this.mesh = base.GetComponent<MeshFilter>().mesh;
        Vector3 min = this.mesh.bounds.min;
        this.start = min.x;
        Vector3 max = this.mesh.bounds.max;
        this.end = max.x;
        this.length = this.end - this.start;
        Vector3 localScale = base.transform.localScale;
        this.zxscale = localScale.y;
    }

    public void turn(bool on)
    {
        if ((bool)this.llight)
        {
            this.llight.enabled = on;
        }
        base.GetComponent<Renderer>().enabled = on;
    }

    public void fire(Transform target)
    {
        this.target = target;
        if ((Object)target == (Object)null && (bool)this.llight)
        {
            this.llight.enabled = false;
        }
    }

    private void doLightning(float dist)
    {
        if ((bool)this.llight)
        {
            this.llight.enabled = true;
        }
        base.GetComponent<Renderer>().enabled = true;
        base.transform.LookAt(this.target, new Vector3(0f, 0f, 0f));
        base.transform.Rotate(new Vector3(0f, 90f, 0f));
        Vector3 localScale = new Vector3(0.01f, this.zxscale, this.zxscale);
        localScale.x = dist / this.length;
        base.transform.localScale = localScale;
        if (this.baseVertices == null)
        {
            this.baseVertices = this.mesh.vertices;
        }
        Vector3[] array = new Vector3[this.baseVertices.Length];
        float num = Time.time * this.speed + 1.21688f;
        float num2 = Time.time * this.speed + 2.5564f;
        int num3 = (int)(Time.time * this.speed / 2f + 1.21688f);
        int num4 = (int)(Time.time * this.speed / 2f + 2.5564f);
        float num5 = this.scale * 5f * dist / 20f;
        float num6 = 0f;
        for (int i = 0; i < array.Length; i++)
        {
            Vector3 vector = this.baseVertices[i];
            num6 = ((this.length == 0f) ? 0f : ((vector.x - this.start) / this.length));
            if ((double)num6 > 0.5 && this.length != 0f)
            {
                num6 = (this.end - vector.x) / this.length;
            }
            vector.y += this.noise.Noise(num + vector.x / 2f, num, num) * this.scale * num6 * dist / 20f;
            vector.z += this.noise.Noise(num2 + vector.x / 2f, num2, num2) * this.scale * num6 * dist / 20f;
            vector.y += this.noise.Noise((float)num3 + vector.x / 10f, (float)num3, (float)num3) * num5 * num6;
            vector.z += this.noise.Noise((float)num4 + vector.x / 10f, (float)num4, (float)num4) * num5 * num6;
            array[i] = vector;
            if (Mathf.Abs(num6 - 0.5f) < 0.01f && (bool)this.llight)
            {
                vector.Scale(new Vector3(1f, 2f, 2f));
                vector = base.transform.localToWorldMatrix.MultiplyVector(vector);
                this.llight.transform.position = vector + base.transform.position;
            }
        }
        this.mesh.vertices = array;
        if (this.recalculateNormals)
        {
            this.mesh.RecalculateNormals();
        }
        this.mesh.RecalculateBounds();
    }

    private void doStaticLightning()
    {
        if ((bool)this.llight)
        {
            this.llight.enabled = true;
        }
        base.GetComponent<Renderer>().enabled = true;
        Quaternion localRotation = default(Quaternion);
        localRotation.eulerAngles = new Vector3(0f, 0f, 0f);
        base.transform.localRotation = localRotation;
        Vector3 localScale = new Vector3(0.01f, 0.2f, 0.2f);
        base.transform.localScale = localScale;
        if (this.baseVertices == null)
        {
            this.baseVertices = this.mesh.vertices;
        }
        Vector3[] array = new Vector3[this.baseVertices.Length];
        float num = 0f;
        Vector3 min = this.mesh.bounds.min;
        float x = min.x;
        Vector3 max = this.mesh.bounds.max;
        float x2 = max.x;
        float num2 = x2 - x;
        for (int i = 0; i < array.Length; i++)
        {
            Vector3 vector = this.baseVertices[i];
            num = ((num2 == 0f) ? 0f : ((vector.x - x) / num2));
            if ((double)num > 0.5 && num2 != 0f)
            {
                num = (x2 - vector.x) / num2;
            }
            array[i] = vector;
            if (Mathf.Abs(num - 0.5f) < 0.01f && (bool)this.llight)
            {
                vector.Scale(new Vector3(1f, 2f, 2f));
                vector = base.transform.localToWorldMatrix.MultiplyVector(vector);
                this.llight.transform.position = vector + base.transform.position;
            }
        }
        this.mesh.vertices = array;
        if (this.recalculateNormals)
        {
            this.mesh.RecalculateNormals();
        }
        this.mesh.RecalculateBounds();
    }

    private void LateUpdate()
    {
        if ((bool)this.target)
        {
            this.tm += 1L;
            if (this.tm > 8)
            {
                this.tm = 0L;
            }
            if (this.tm % 2 == 0L)
            {
                float dist = Vector3.Distance(base.transform.position, this.target.position);
                this.doLightning(dist);
            }
        }
        else if (this.statical)
        {
            this.doStaticLightning();
        }
        else
        {
            base.GetComponent<Renderer>().enabled = false;
        }
    }
}


