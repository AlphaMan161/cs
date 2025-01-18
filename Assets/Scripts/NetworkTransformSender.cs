// ILSpyBased#2
using UnityEngine;

public class NetworkTransformSender : MonoBehaviour
{
    public long lastSendTime;

    public static readonly float synchronizationDelay = 5000f;

    public long lastSynchronizationTime;

    private float timeLastSending;

    private bool send;

    private NetworkTransform lastState;

    private bool sendHeight;

    private PlayerRemote playerRemote;

    private Transform thisTransform;

    public bool FlagBearer
    {
        set
        {
            this.sendHeight = value;
        }
    }

    private bool InAir
    {
        get
        {
            if ((Object)this.playerRemote == (Object)null)
            {
                this.playerRemote = ((Component)base.transform).GetComponent<PlayerRemote>();
            }
            return this.playerRemote.InAir;
        }
    }

    private void Start()
    {
        this.thisTransform = base.transform;
        this.lastState = NetworkTransform.FromTransform(this.thisTransform);
    }

    public void StartSendTransform()
    {
        this.send = true;
    }

    private void FixedUpdate()
    {
        if (this.send)
        {
            this.SendTransform();
        }
    }

    private void SendTransform()
    {
        long networkTime = TimeManager.Instance.NetworkTime;
        long num = NetworkDev.TPS;
        if (networkTime - this.lastSendTime > num || this.lastSendTime == 0L)
        {
            this.lastState = PlayerManager.Instance.SendTransform(base.transform, this.sendHeight, this.InAir, this.lastState);
            this.timeLastSending = 0f;
            this.lastSendTime = networkTime;
        }
        else
        {
            this.timeLastSending += Time.deltaTime;
        }
    }
}


