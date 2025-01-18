// ILSpyBased#2
using UnityEngine;

[AddComponentMenu("Camera-Control/TPS Camera")]
public class TPSCamera : MonoBehaviour
{
    public float Distance = 10f;

    private Transform Biped;

    public MouseOverLook mouseOverLook;

    public bool OverLook
    {
        set
        {
            this.mouseOverLook.RotationEnabled = value;
        }
    }

    private void Start()
    {
        this.Biped = base.transform.parent.parent.FindChild("Bip01");
    }

    private void OnEnable()
    {
        if ((Object)this.mouseOverLook != (Object)null)
        {
            this.mouseOverLook.Reset();
        }
    }

    private void FixedUpdate()
    {
        base.transform.parent.position = this.Biped.transform.position;
    }
}


