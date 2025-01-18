// ILSpyBased#2
using UnityEngine;

public class NetworkTransformReceiver : MonoBehaviour
{
    private Transform thisTransform;

    private NetworkTransformInterpolation interpolator;

    private void Awake()
    {
        this.thisTransform = base.transform;
        this.interpolator = base.GetComponent<NetworkTransformInterpolation>();
        if ((Object)this.interpolator != (Object)null)
        {
            this.interpolator.StartReceiving();
        }
    }

    public void StartReceiving()
    {
        if ((Object)this.interpolator != (Object)null)
        {
            this.interpolator.StartReceiving();
        }
    }

    public void StopReceiving()
    {
        if ((Object)this.interpolator != (Object)null)
        {
            this.interpolator.StopReceiving();
        }
    }

    public long ReceiveTransform(NetworkTransform ntransform)
    {
        if ((Object)this.interpolator != (Object)null)
        {
            return this.interpolator.ReceiveTransform(ntransform);
        }
        MonoBehaviour.print("NO Intrepolation!!!");
        this.thisTransform.position = ntransform.Position;
        this.thisTransform.localEulerAngles = ntransform.Rotation;
        return 0L;
    }

    public void ReceiveAnimationKey(byte key, long timeStamp)
    {
        if ((Object)this.interpolator != (Object)null)
        {
            this.interpolator.ReceiveAnimationKey(key, timeStamp);
        }
        else
        {
            CombatPlayer component = base.GetComponent<CombatPlayer>();
            if (!((Object)component == (Object)null))
            {
                component.SetAnimationKey(key);
            }
        }
    }

    public void ReceiveAnimationState(byte state, long timeStamp)
    {
        if ((Object)this.interpolator != (Object)null)
        {
            this.interpolator.ReceiveAnimationState(state, timeStamp);
        }
        else
        {
            CombatPlayer component = base.GetComponent<CombatPlayer>();
            if (!((Object)component == (Object)null))
            {
                component.SetAnimationState(state);
            }
        }
    }

    public void Reset()
    {
        if ((Object)this.interpolator != (Object)null)
        {
            this.interpolator.Reset();
        }
    }
}


