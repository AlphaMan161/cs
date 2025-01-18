// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class AlphaTween : MonoBehaviour
{
    public float start;

    public float end;

    public float speed;

    private bool active;

    private float distance;

    private float position;

    private bool finished;

    private bool tweenOut;

    private List<Material> materials;

    public bool Finished
    {
        get
        {
            return this.finished;
        }
    }

    public void Launch(float start, float end, float delay)
    {
        this.Launch(start, end, delay, false);
    }

    public void Launch(float start, float end, float delay, bool tweenOut)
    {
        this.tweenOut = tweenOut;
        this.finished = false;
        this.start = start;
        this.end = end;
        this.distance = Mathf.Abs(end - start);
        this.speed = this.distance / delay;
        this.position = 0f;
        float a = this.start;
        if (this.distance > 0f)
        {
            this.active = true;
        }
        else
        {
            a = this.end;
            this.active = false;
            this.finished = true;
        }
        this.materials = new List<Material>();
        Renderer[] componentsInChildren = ((Component)base.transform).GetComponentsInChildren<Renderer>();
        Renderer[] array = componentsInChildren;
        foreach (Renderer renderer in array)
        {
            this.materials.Add(renderer.material);
            Material material = renderer.material;
            Color color = renderer.material.color;
            float r = color.r;
            Color color2 = renderer.material.color;
            float g = color2.g;
            Color color3 = renderer.material.color;
            material.color = new Color(r, g, color3.b, a);
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
            float a = (this.end * this.position + this.start * (this.distance - this.position)) / this.distance;
            if (this.position >= this.distance)
            {
                a = this.end;
                this.active = false;
                this.finished = true;
            }
            List<Material>.Enumerator enumerator = this.materials.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Material current = enumerator.Current;
                    Material material = current;
                    Color color = current.color;
                    float r = color.r;
                    Color color2 = current.color;
                    float g = color2.g;
                    Color color3 = current.color;
                    material.color = new Color(r, g, color3.b, a);
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
        }
    }

    public void Destroy()
    {
        base.enabled = false;
    }
}


