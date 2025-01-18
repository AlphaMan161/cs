// ILSpyBased#2
using UnityEngine;

public class SceneTurn : Scene
{
    private Vector3 prevRotation;

    private Transform glider;

    private float angleDistance;

    public SceneTurn(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
        : base(sceneCompleteListener, task, name)
    {
    }

    protected override void Init()
    {
        this.glider = LocalPlayerManager.Instance.LocalPlayer.transform;
        this.angleDistance = 0f;
        this.prevRotation = this.glider.localEulerAngles;
    }

    protected override bool Trigger()
    {
        bool result = false;
        Vector3 localEulerAngles = this.glider.localEulerAngles;
        float f = this.AngleDifference(localEulerAngles.y, this.prevRotation.y);
        this.angleDistance += Mathf.Abs(f);
        if (this.angleDistance > 180f)
        {
            result = true;
        }
        this.prevRotation = this.glider.localEulerAngles;
        return result;
    }

    private float AngleDifference(float newAngle, float currentAngle)
    {
        float num = newAngle - currentAngle;
        if (num > 180f)
        {
            num = currentAngle + 360f - newAngle;
        }
        if (num < -180f)
        {
            num = currentAngle - 360f - newAngle;
        }
        return num;
    }
}


