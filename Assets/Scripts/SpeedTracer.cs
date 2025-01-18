// ILSpyBased#2
using UnityEngine;

public class SpeedTracer : MonoBehaviour
{
    public float flatSpeed = 15f;

    public Vector3 Speed;

    private bool active = true;

    private void Start()
    {
        this.Speed *= this.flatSpeed;
    }

    private void Update()
    {
        if (this.active && !(this.Speed == Vector3.zero))
        {
            base.transform.localPosition = base.transform.localPosition + this.Speed;
        }
    }
}


