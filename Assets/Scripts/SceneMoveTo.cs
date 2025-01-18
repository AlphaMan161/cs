// ILSpyBased#2
using UnityEngine;

public class SceneMoveTo : Scene
{
    private Transform glider;

    private Vector3 min;

    private Vector3 max;

    public SceneMoveTo(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
        : base(sceneCompleteListener, task, name)
    {
    }

    protected override void Init()
    {
        this.glider = LocalPlayerManager.Instance.LocalPlayer.transform;
        this.min = (Vector3)base.Objects["min"];
        this.max = (Vector3)base.Objects["max"];
    }

    protected override bool Trigger()
    {
        bool result = false;
        if (this.Less(this.min, this.glider.position) && this.Less(this.glider.position, this.max))
        {
            result = true;
        }
        return result;
    }

    protected bool Less(Vector3 vector, Vector3 thenVector)
    {
        if (vector.x >= thenVector.x)
        {
            return false;
        }
        if (vector.y >= thenVector.y)
        {
            return false;
        }
        if (vector.z >= thenVector.z)
        {
            return false;
        }
        return true;
    }
}


