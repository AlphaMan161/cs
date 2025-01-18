// ILSpyBased#2
using UnityEngine;

public class AutomaticDoor : ItemTracer
{
    protected long nextScanTime;

    protected new long launchTime;

    protected long launchDelay = 200L;

    protected bool active;

    public float touchDistance = 20f;

    private float touchDistanceSecure = 20f;

    public float doorSpeed = 2f;

    public string OpenAnimationName;

    public string CloseAnimationName;

    private new Animation animation;

    private Vector3 scannerPos;

    private bool doorOpened;

    private bool doorOpenedSecure;

    public byte Team;

    private long time
    {
        get
        {
            return TimeManager.Instance.NetworkTime;
        }
    }

    private void Start()
    {
        this.animation = ((Component)base.transform.parent).GetComponentInChildren<Animation>();
        this.scannerPos = base.transform.parent.FindChild("Scanner").transform.position;
        this.touchDistanceSecure = this.touchDistance;
        this.Launch();
    }

    public void Launch()
    {
        this.launchTime = this.time + this.launchDelay;
    }

    private int OtherTeam(int team)
    {
        switch (team)
        {
            case 0:
                return 0;
            case 1:
                return 2;
            default:
                return 1;
        }
    }

    private void FixedUpdate()
    {
        if (this.touchDistanceSecure != this.touchDistance || this.doorOpenedSecure != this.doorOpened)
        {
            PlayerManager.Instance.SendEnterBaseRequest(9);
        }
        if (this.time >= this.launchTime || this.launchTime == 0L)
        {
            this.active = true;
        }
        if (!this.active)
        {
            return;
        }
        if (this.nextScanTime >= this.time && this.nextScanTime != 0L)
        {
            return;
        }
        this.nextScanTime = this.time + 500;
        int num = -1;
        num = PlayerManager.Instance.TeamScan(this.scannerPos, this.touchDistance, this.OtherTeam(this.Team), false, true);
        if (this.Team != 1)
        {
            goto IL_00be;
        }
        goto IL_00be;
        IL_00be:
        if (num > 0 && !this.doorOpened)
        {
            this.Open();
            this.nextScanTime = this.time + 1000;
            this.doorOpened = true; this.doorOpenedSecure = (this.doorOpened );
        }
        else if (num == -1 && this.doorOpened)
        {
            this.Close();
            this.nextScanTime = this.time + 1000;
            this.doorOpened = false; this.doorOpenedSecure = (this.doorOpened );
        }
    }

    protected void Open()
    {
        this.animation.Stop();
        this.animation.Play(this.OpenAnimationName);
        this.animation[this.OpenAnimationName].speed = this.doorSpeed;
    }

    protected void Close()
    {
        this.animation.Stop();
        this.animation.Play(this.CloseAnimationName);
        this.animation[this.CloseAnimationName].speed = this.doorSpeed;
    }

    public override void Destroy()
    {
        this.active = false;
        base.enabled = false;
        UnityEngine.Object.DestroyImmediate(base.gameObject);
    }
}


