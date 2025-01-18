// ILSpyBased#2
using UnityEngine;

public class SpriteInfo : MonoBehaviour
{
    public bool Invert;

    private Renderer[] renderers;

    private void Awake()
    {
        this.renderers = base.GetComponentsInChildren<Renderer>();
    }

    private void LateUpdate()
    {
        if ((Object)PlayerManager.Instance != (Object)null)
        {
            if ((Object)PlayerManager.Instance.ActiveCamera != (Object)null)
            {
                if (this.Invert)
                {
                    Vector3 b = base.transform.position - PlayerManager.Instance.ActiveCamera.transform.position;
                    base.transform.LookAt(base.transform.position + b);
                }
                else
                {
                    base.transform.LookAt(PlayerManager.Instance.ActiveCamera.transform);
                }
            }
        }
        else
        {
            base.transform.LookAt(Camera.main.transform);
        }
    }
}


