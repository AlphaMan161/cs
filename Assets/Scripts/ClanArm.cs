// ILSpyBased#2
using System;
using UnityEngine;

public class ClanArm
{
    private int arm_id;

    private Texture2D ico;

    private bool loading;

    private string imgUrl = string.Empty;

    private bool isDefault;

    private ShopCost cost;

    public int ArmID
    {
        get
        {
            return this.arm_id;
        }
    }

    public Texture2D Ico
    {
        get
        {
            if ((UnityEngine.Object)this.ico == (UnityEngine.Object)null && !this.loading)
            {
                this.loading = true;
                Ajax.Request(this.imgUrl, new AjaxRequest.AjaxHandler(this.OnLoading), AjaxRequest.DataType.Image);
            }
            return this.ico;
        }
    }

    public bool Default
    {
        get
        {
            return this.isDefault;
        }
    }

    public ShopCost Cost
    {
        get
        {
            return this.cost;
        }
    }

    public ClanArm(JSONObject json)
    {
        this.arm_id = Convert.ToInt32(json["aid"].n);
        this.imgUrl = json["i"].str;
        this.isDefault = Convert.ToBoolean(json["d"].n);
        if (json["sc"] != null)
        {
            this.cost = new ShopCost(json["sc"]);
        }
    }

    public ClanArm(int arm_id, string imgUrl, bool isDefault, ShopCost cost)
    {
        this.arm_id = arm_id;
        this.imgUrl = imgUrl;
        this.isDefault = isDefault;
        this.cost = cost;
    }

    private void OnLoading(object res, AjaxRequest request)
    {
        this.ico = (res as Texture2D);
        this.loading = false;
    }
}


