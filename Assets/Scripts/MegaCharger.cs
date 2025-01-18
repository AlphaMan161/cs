// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class MegaCharger : MonoBehaviour
{
    private const float FOLLOW = 0.7f;

    public Transform target;

    private bool on = true;

    private LineRenderer Source;

    private int maxPoints = 50;

    private int pointCount = 1;

    private float step = 2f;

    private Vector3[] points;

    public Bezier myBezier;

    private float turnOffAt;

    private ParticleEmitter Glow;

    private void Start()
    {
        this.points = new Vector3[4];
        this.Source = ((Component)base.transform).GetComponentInChildren<LineRenderer>();
        this.Glow = ((Component)base.transform).GetComponentInChildren<ParticleEmitter>();
        this.Glow.emit = false;
        this.Source.SetVertexCount(1);
        this.points[0] = new Vector3(0f, 0f, 0f);
        this.Source.SetPosition(0, this.points[0]);
        this.turn(false);
    }

    private Vector3 Bezier3(Vector3 s, Vector3 st, Vector3 et, Vector3 e, float t)
    {
        return (((-s + 3f * (st - et) + e) * t + (3f * (s + et) - 6f * st)) * t + 3f * (st - s)) * t + s;
    }

    private void LateUpdate()
    {
        if (this.on && (Object)this.target != (Object)null)
        {
            if (!this.Glow.emit)
            {
                this.Glow.emit = true;
            }
            this.points[3] = this.Source.transform.InverseTransformPoint(this.target.transform.position);
            float num = Mathf.Sin(Time.time * 5f) * 10f;
            float num2 = Mathf.Sin(Time.time * 3f) * 10f;
            this.points[2] = this.points[3] * 0.5f + new Vector3(0f, num, num2);
            float num3 = Vector3.Distance(this.points[3], this.points[0]);
            this.points[1] = new Vector3((0f - num3) * 0.7f, (0f - num) / 2f, (0f - num2) / 2f);
            this.pointCount = (int)(num3 / this.step);
            if (this.pointCount > this.maxPoints)
            {
                this.pointCount = this.maxPoints;
            }
            if (this.pointCount < 2)
            {
                this.pointCount = 2;
            }
            this.Source.SetVertexCount(this.pointCount + 1);
            float num4 = 0f;
            for (int i = 0; i <= this.pointCount; i++)
            {
                num4 = (float)i / (float)this.pointCount;
                this.Source.SetPosition(i, this.Bezier3(this.points[0], this.points[1], this.points[2], this.points[3], num4));
            }
            this.Glow.transform.position = this.Source.transform.TransformPoint(this.points[3] + new Vector3(20f, 0f, 0f));
        }
    }

    public void fire(Transform target)
    {
        if (!((Object)this.Source == (Object)null) && (bool)target)
        {
            this.turn(true);
            this.target = target;
            base.StartCoroutine(this.WaitAndFireOff(0.5f));
        }
    }

    public void turn(bool on)
    {
        this.on = on;
        if (!on)
        {
            this.target = null;
            this.Source.SetVertexCount(1);
            if (this.Glow.emit)
            {
                this.Glow.emit = false;
            }
        }
    }

    private IEnumerator WaitAndFireOff(float waitTime)
    {
        float currTime2 = Time.time;
        if (currTime2 + waitTime > this.turnOffAt)
        {
            this.turnOffAt = currTime2 + waitTime;
        }
        yield return (object)new WaitForSeconds(waitTime);
        currTime2 = Time.time;
        if ((double)currTime2 + 0.01 >= (double)this.turnOffAt)
        {
            this.turn(false);
        }
    }
}


