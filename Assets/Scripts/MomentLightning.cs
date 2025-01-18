// ILSpyBased#2
using UnityEngine;

public class MomentLightning : MonoBehaviour
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

    private void Start()
    {
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

    private void LateUpdate()
    {
        float dist = this.length;
        if ((Object)this.target != (Object)null)
        {
            base.transform.LookAt(this.target, new Vector3(0f, 0f, 0f));
            base.transform.Rotate(new Vector3(0f, 90f, 0f));
            dist = Vector3.Distance(base.transform.position, this.target.position);
        }
        this.doLightning(dist);
    }

    private void doLightning(float dist)
    {
        base.GetComponent<Renderer>().enabled = true;
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
            if (num6 > 0.5f && this.length != 0f)
            {
                num6 = (this.end - vector.x) / this.length;
            }
            vector.y += this.noise.Noise(num + vector.x / 2f, num, num) * this.scale * num6 * dist / 20f;
            vector.z += this.noise.Noise(num2 + vector.x / 2f, num2, num2) * this.scale * num6 * dist / 20f;
            vector.y += this.noise.Noise((float)num3 + vector.x / 10f, (float)num3, (float)num3) * num5 * num6;
            vector.z += this.noise.Noise((float)num4 + vector.x / 10f, (float)num4, (float)num4) * num5 * num6;
            array[i] = vector;
        }
        this.mesh.vertices = array;
        if (this.recalculateNormals)
        {
            this.mesh.RecalculateNormals();
        }
        this.mesh.RecalculateBounds();
    }
}


