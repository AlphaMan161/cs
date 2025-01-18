// ILSpyBased#2
using UnityEngine;

public class Follow2D : MonoBehaviour
{
    public Transform Target;

    public string FindTarget = string.Empty;

    public bool FollowX = true;

    public bool FollowY;

    public bool FollowZ = true;

    public Vector3 Shift = Vector3.zero;

    private void Start()
    {
    }

    private void Update()
    {
        if (this.FindTarget != string.Empty)
        {
            Transform[] componentsInChildren = base.transform.parent.GetComponentsInChildren<Transform>(true);
            foreach (Transform transform in componentsInChildren)
            {
                if (transform.gameObject.name == this.FindTarget)
                {
                    this.Target = transform;
                    this.FindTarget = string.Empty;
                    break;
                }
            }
        }
        if (!((Object)this.Target == (Object)null))
        {
            Vector3 position = base.transform.position;
            if (this.FollowX)
            {
                Vector3 position2 = this.Target.position;
                position.x = position2.x;
            }
            if (this.FollowY)
            {
                Vector3 position3 = this.Target.position;
                position.y = position3.y;
            }
            if (this.FollowZ)
            {
                Vector3 position4 = this.Target.position;
                position.z = position4.z;
            }
            base.transform.position = position + this.Shift;
        }
    }
}


