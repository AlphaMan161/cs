// ILSpyBased#2
using UnityEngine;

public class TransformTween : MonoBehaviour
{
    public Vector3 start;

    public Vector3 end;

    public float speed;

    private bool active;

    private float distance;

    private float position;

    private bool finished;

    private bool tweenOut;

    public bool Finished
    {
        get
        {
            return this.finished;
        }
    }

    public void Launch(Vector3 start, Vector3 end, float delay)
    {
        this.Launch(start, end, delay, false);
    }

    public void Launch(Vector3 start, Vector3 end, float delay, bool tweenOut)
    {
        this.tweenOut = tweenOut;
        this.finished = false;
        this.start = start;
        this.end = end;
        this.distance = (end - start).magnitude;
        this.speed = this.distance / delay;
        this.position = 0f;
        if (this.distance > 0f)
        {
            base.transform.localPosition = this.start;
            this.active = true;
        }
        else
        {
            base.transform.localPosition = this.end;
            this.active = false;
            this.finished = true;
        }
    }

    private void FixedUpdate()
    {
        if (this.active)
        {
            float num = 1f;
            num = ((!this.tweenOut) ? (1.5f * (this.distance - this.position) / this.distance) : 1f);
            if (num < 0.05f)
            {
                num = 0.05f;
            }
            this.position += num * this.speed * Time.deltaTime;
            base.transform.localPosition = (this.end * this.position + this.start * (this.distance - this.position)) / this.distance;
            if (this.position >= this.distance)
            {
                base.transform.localPosition = this.end;
                this.active = false;
                this.finished = true;
            }
        }
    }

    public void Destroy()
    {
        base.enabled = false;
    }
}


