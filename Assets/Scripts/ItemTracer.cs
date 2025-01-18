// ILSpyBased#2
using UnityEngine;

public abstract class ItemTracer : MonoBehaviour
{
    protected long launchTime;

    protected WeaponType weaponType;

    public WeaponType WeaponType
    {
        get
        {
            return this.weaponType;
        }
        set
        {
            this.weaponType = value;
        }
    }

    public long TimeStamp
    {
        get
        {
            return this.launchTime;
        }
    }

    private void Start()
    {
    }

    public abstract void Destroy();
}


