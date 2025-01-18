// ILSpyBased#2
using UnityEngine;

public class SceneMove : Scene
{
    private Vector3 prevPosition;

    private Transform glider;

    private float moveDistance;

    public SceneMove(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
        : base(sceneCompleteListener, task, name)
    {
    }

    protected override void Init()
    {
        this.glider = LocalPlayerManager.Instance.LocalPlayer.transform;
        this.moveDistance = 0f;
        this.prevPosition = this.glider.localPosition;
    }

    protected override bool Trigger()
    {
        bool result = false;
        float magnitude = (this.glider.localPosition - this.prevPosition).magnitude;
        this.moveDistance += magnitude;
        if (this.moveDistance > 40f)
        {
            result = true;
        }
        this.prevPosition = this.glider.localPosition;
        return result;
    }
}


