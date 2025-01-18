// ILSpyBased#2
using UnityEngine;

public class BankActionPackage
{
    public delegate void BankActionPackageHandler(object param);

    private BankActionPackageHandler onClick;

    private Texture2D ico;

    private short package_id;

    private uint endTime;

    private string url = string.Empty;

    private bool isLoaded;

    private float nextCheckTime;

    public Texture2D Ico
    {
        get
        {
            this.CheckEndTime();
            return this.ico;
        }
    }

    public short PackageID
    {
        get
        {
            return this.package_id;
        }
    }

    public uint EndTime
    {
        get
        {
            return this.endTime;
        }
    }

    public bool IsLoaded
    {
        get
        {
            return this.isLoaded;
        }
    }

    public event BankActionPackageHandler OnEndtime;

    public BankActionPackage(Texture2D ico, short pack_id, uint endTime, BankActionPackageHandler callback)
    {
        this.ico = ico;
        this.package_id = pack_id;
        this.endTime = endTime;
        this.onClick = callback;
        this.isLoaded = true;
    }

    public BankActionPackage(string url, short pack_id, uint endTime, BankActionPackageHandler callback)
    {
        Ajax.Request(url, new AjaxRequest.AjaxHandler(this.OnLoading), AjaxRequest.DataType.Image);
        this.package_id = pack_id;
        this.endTime = endTime;
        this.onClick = callback;
        this.url = url;
    }

    private void CheckEndTime()
    {
        if (!(Time.time < this.nextCheckTime))
        {
            if ((float)(double)this.endTime <= Time.time)
            {
                if (this.OnEndtime != null)
                {
                    this.OnEndtime(this);
                }
                this.nextCheckTime = 3.40282347E+38f;
            }
            this.nextCheckTime = Time.time + 10f;
        }
    }

    private void OnLoading(object res, AjaxRequest request)
    {
        this.ico = (res as Texture2D);
        this.isLoaded = true;
    }

    public void Click()
    {
        if (this.onClick != null)
        {
            this.onClick(this.package_id);
        }
        WebCall.Analitic("Pack_Click", string.Format("ID:{0}", this.package_id));
    }
}


