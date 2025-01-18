// ILSpyBased#2
using UnityEngine;

public class SceneJump : Scene
{
    private Transform glider;

    private Vector3 min;

    private Vector3 max;

    public SceneJump(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
        : base(sceneCompleteListener, task, name)
    {
    }

    public void Setup(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    protected override void Init()
    {
        this.glider = LocalPlayerManager.Instance.LocalPlayer.transform;
    }

    protected override bool Trigger()
    {
        bool result = false;
        Vector3 position = this.glider.position;
        if (position.y > -4f)
        {
            result = true;
        }
        return result;
    }
}


