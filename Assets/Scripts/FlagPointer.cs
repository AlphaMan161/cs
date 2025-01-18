// ILSpyBased#2
using UnityEngine;

public class FlagPointer : MonoBehaviour
{
    public Transform Me;

    public Transform Target;

    private float angle;

    public int TargetID = -3;

    private Material material;

    private Transform pointer;

    private void Start()
    {
    }

    private void LateUpdate()
    {
        if (!((Object)this.Me == (Object)null) && !((Object)this.Target == (Object)null))
        {
            Vector3 vector = this.Target.position - this.Me.position;
            Vector3 vector2 = this.Me.TransformDirection(0f, 0f, 1f);
            Vector3 vector3 = new Vector3(vector.x, 0f, vector.z);
            this.angle = Vector3.Angle(vector2, vector3);
            Vector3 vector4 = Vector3.Cross(vector2, vector3);
            float y = vector4.y;
            if (y > 0f)
            {
                this.angle = 0f - this.angle;
            }
            base.transform.eulerAngles = new Vector3(0f, 0f, this.angle);
            if ((Object)this.pointer != (Object)null)
            {
                float num = (500f - vector.magnitude) / 5000f;
                if (num < 0.05f)
                {
                    num = 0.05f;
                }
                this.pointer.localScale = new Vector3(num, 1f, num);
            }
        }
    }

    public void SetColor(Color color)
    {
        if ((Object)this.material == (Object)null)
        {
            Renderer componentInChildren = base.GetComponentInChildren<Renderer>();
            if (!((Object)componentInChildren == (Object)null))
            {
                this.pointer = componentInChildren.transform;
                this.material = componentInChildren.material;
                goto IL_003d;
            }
            return;
        }
        goto IL_003d;
        IL_003d:
        this.material.SetColor("_TintColor", color);
    }

    public void SetColor(float alpha)
    {
        if ((Object)this.material == (Object)null)
        {
            Renderer componentInChildren = base.GetComponentInChildren<Renderer>();
            if (!((Object)componentInChildren == (Object)null))
            {
                this.pointer = componentInChildren.transform;
                this.material = componentInChildren.material;
                goto IL_003d;
            }
            return;
        }
        goto IL_003d;
        IL_003d:
        Color color = this.material.GetColor("_TintColor");
        color.a = alpha;
        this.material.SetColor("_TintColor", color);
    }
}


