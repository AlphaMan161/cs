// ILSpyBased#2
using UnityEngine;

public class SceneZoom : Scene
{
    private LocalMouseOverLook mouseOverLook;

    private Vector3 min;

    private Vector3 max;

    public SceneZoom(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
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
        Transform transform = LocalPlayerManager.Instance.LocalPlayer.transform;
        this.mouseOverLook = ((Component)transform).GetComponentInChildren<LocalMouseOverLook>();
    }

    protected override bool Trigger()
    {
        bool result = false;
        if (this.mouseOverLook.zoomFactor < 0.5f)
        {
            result = true;
        }
        return result;
    }
}


