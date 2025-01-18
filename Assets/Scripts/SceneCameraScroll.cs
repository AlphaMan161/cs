// ILSpyBased#2
using UnityEngine;

public class SceneCameraScroll : Scene
{
    private Vector3 prevCameraPosition;

    private Transform glider;

    private LocalMouseOverLook mouseLook;

    private float scrollDistance;

    public SceneCameraScroll(SceneCompleteListener sceneCompleteListener, GameObject task, string name)
        : base(sceneCompleteListener, task, name)
    {
    }

    protected override void Init()
    {
        this.glider = LocalPlayerManager.Instance.LocalPlayer.transform;
        this.mouseLook = ((Component)this.glider).GetComponentInChildren<LocalMouseOverLook>();
        this.scrollDistance = 0f;
        this.prevCameraPosition = this.mouseLook.OriginalPosition;
    }

    protected override bool Trigger()
    {
        bool result = false;
        float magnitude = (this.mouseLook.OriginalPosition - this.prevCameraPosition).magnitude;
        this.scrollDistance += magnitude;
        if (this.scrollDistance > 10f)
        {
            result = true;
        }
        this.prevCameraPosition = this.mouseLook.OriginalPosition;
        return result;
    }
}


