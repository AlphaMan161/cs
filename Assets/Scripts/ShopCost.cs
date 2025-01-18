// ILSpyBased#2
using SimpleJSON;
using System;
using UnityEngine;

public class ShopCost
{
    private uint shop_cost_id;

    private bool time1;

    private bool time7;

    private bool time30;

    private bool timeP;

    private uint time1_vcost;

    private uint time1_rcost;

    private uint time1_pvpcost;

    private uint time7_vcost;

    private uint time7_rcost;

    private uint time7_pvpcost;

    private uint time30_vcost;

    private uint time30_rcost;

    private uint time30_pvpcost;

    private uint timeP_vcost;

    private uint timeP_rcost;

    private uint timeP_pvpcost;

    private DurationType selectedDuration = DurationType.PERMANENT;

    public uint ShopCostID
    {
        get
        {
            return this.shop_cost_id;
        }
    }

    public bool isTime1
    {
        get
        {
            return this.time1;
        }
    }

    public bool isTime7
    {
        get
        {
            return this.time7;
        }
    }

    public bool isTime30
    {
        get
        {
            return this.time30;
        }
    }

    public bool isTimeP
    {
        get
        {
            return this.timeP;
        }
    }

    public uint Time1VCost
    {
        get
        {
            return this.time1_vcost;
        }
        set
        {
            if (value != 0)
            {
                this.time1 = true;
            }
            this.time1_vcost = value;
        }
    }

    public uint Time1RCost
    {
        get
        {
            return this.time1_rcost;
        }
        set
        {
            if (value != 0)
            {
                this.time1 = true;
            }
            this.time1_rcost = value;
        }
    }

    public uint Time1PVPCost
    {
        get
        {
            return this.time1_pvpcost;
        }
        set
        {
            if (value != 0)
            {
                this.time1 = true;
            }
            this.time1_pvpcost = value;
        }
    }

    public uint Time7VCost
    {
        get
        {
            return this.time7_vcost;
        }
        set
        {
            if (value != 0)
            {
                this.time7 = true;
            }
            this.time7_vcost = value;
        }
    }

    public uint Time7RCost
    {
        get
        {
            return this.time7_rcost;
        }
        set
        {
            if (value != 0)
            {
                this.time7 = true;
            }
            this.time7_rcost = value;
        }
    }

    public uint Time7PVPCost
    {
        get
        {
            return this.time7_pvpcost;
        }
        set
        {
            if (value != 0)
            {
                this.time7 = true;
            }
            this.time7_pvpcost = value;
        }
    }

    public uint Time30VCost
    {
        get
        {
            return this.time30_vcost;
        }
        set
        {
            if (value != 0)
            {
                this.time30 = true;
            }
            this.time30_vcost = value;
        }
    }

    public uint Time30RCost
    {
        get
        {
            return this.time30_rcost;
        }
        set
        {
            if (value != 0)
            {
                this.time30 = true;
            }
            this.time30_rcost = value;
        }
    }

    public uint Time30PVPCost
    {
        get
        {
            return this.time30_pvpcost;
        }
        set
        {
            if (value != 0)
            {
                this.time30 = true;
            }
            this.time30_pvpcost = value;
        }
    }

    public uint TimePVCost
    {
        get
        {
            return this.timeP_vcost;
        }
        set
        {
            if (value != 0)
            {
                this.timeP = true;
            }
            this.timeP_vcost = value;
        }
    }

    public uint TimePRCost
    {
        get
        {
            return this.timeP_rcost;
        }
        set
        {
            if (value != 0)
            {
                this.timeP = true;
            }
            this.timeP_rcost = value;
        }
    }

    public uint TimePPVPCost
    {
        get
        {
            return this.timeP_pvpcost;
        }
        set
        {
            if (value != 0)
            {
                this.timeP = true;
            }
            this.timeP_pvpcost = value;
        }
    }

    public DurationType SelectedDuration
    {
        get
        {
            return this.selectedDuration;
        }
        set
        {
            this.selectedDuration = value;
        }
    }

    public uint SelectedVCost
    {
        get
        {
            if (this.selectedDuration == DurationType.PERMANENT)
            {
                return this.timeP_vcost;
            }
            if (this.selectedDuration == DurationType.DAY)
            {
                return this.time1_vcost;
            }
            if (this.selectedDuration == DurationType.WEEK)
            {
                return this.time7_vcost;
            }
            if (this.selectedDuration == DurationType.MONTH)
            {
                return this.time30_vcost;
            }
            return 0u;
        }
    }

    public ShopCost(JSONObject obj)
    {
        if (obj.type != JSONObject.Type.OBJECT)
        {
            throw new Exception("[ShopCost] Input object not a valid data");
        }
        this.shop_cost_id = Convert.ToUInt32(obj.GetField("sc_id").str);
        if (obj.GetField("t1v") != null)
        {
            if (obj.GetField("t1v").type == JSONObject.Type.NUMBER)
            {
                this.Time1VCost = Convert.ToUInt32(obj.GetField("t1v").n);
            }
            else
            {
                this.Time1VCost = Convert.ToUInt32(obj.GetField("t1v").str);
            }
        }
        if (obj.GetField("t1r") != null)
        {
            if (obj.GetField("t1r").type == JSONObject.Type.NUMBER)
            {
                this.Time1RCost = Convert.ToUInt32(obj.GetField("t1r").n);
            }
            else
            {
                this.Time1RCost = Convert.ToUInt32(obj.GetField("t1r").str);
            }
        }
        if (obj.GetField("t1p") != null)
        {
            if (obj.GetField("t1p").type == JSONObject.Type.NUMBER)
            {
                this.Time1PVPCost = Convert.ToUInt32(obj.GetField("t1p").n);
            }
            else
            {
                this.Time1PVPCost = Convert.ToUInt32(obj.GetField("t1p").str);
            }
        }
        if (obj.GetField("t7v") != null)
        {
            if (obj.GetField("t7v").type == JSONObject.Type.NUMBER)
            {
                this.Time7VCost = Convert.ToUInt32(obj.GetField("t7v").n);
            }
            else
            {
                this.Time7VCost = Convert.ToUInt32(obj.GetField("t7v").str);
            }
        }
        if (obj.GetField("t7r") != null)
        {
            if (obj.GetField("t7r").type == JSONObject.Type.NUMBER)
            {
                this.Time7RCost = Convert.ToUInt32(obj.GetField("t7r").n);
            }
            else
            {
                this.Time7RCost = Convert.ToUInt32(obj.GetField("t7r").str);
            }
        }
        if (obj.GetField("t7p") != null)
        {
            if (obj.GetField("t7p").type == JSONObject.Type.NUMBER)
            {
                this.Time7PVPCost = Convert.ToUInt32(obj.GetField("t7p").n);
            }
            else
            {
                this.Time7PVPCost = Convert.ToUInt32(obj.GetField("t7p").str);
            }
        }
        if (obj.GetField("t30v") != null)
        {
            if (obj.GetField("t30v").type == JSONObject.Type.NUMBER)
            {
                this.Time30VCost = Convert.ToUInt32(obj.GetField("t30v").n);
            }
            else
            {
                this.Time30VCost = Convert.ToUInt32(obj.GetField("t30v").str);
            }
        }
        if (obj.GetField("t30r") != null)
        {
            if (obj.GetField("t30r").type == JSONObject.Type.NUMBER)
            {
                this.Time30RCost = Convert.ToUInt32(obj.GetField("t30r").n);
            }
            else
            {
                this.Time30RCost = Convert.ToUInt32(obj.GetField("t30r").str);
            }
        }
        if (obj.GetField("t30p") != null)
        {
            if (obj.GetField("t30p").type == JSONObject.Type.NUMBER)
            {
                this.Time30PVPCost = Convert.ToUInt32(obj.GetField("t30p").n);
            }
            else
            {
                this.Time30PVPCost = Convert.ToUInt32(obj.GetField("t30p").str);
            }
        }
        if (obj.GetField("tPv") != null)
        {
            if (obj.GetField("tPv").type == JSONObject.Type.NUMBER)
            {
                this.TimePVCost = Convert.ToUInt32(obj.GetField("tPv").n);
            }
            else
            {
                this.TimePVCost = Convert.ToUInt32(obj.GetField("tPv").str);
            }
        }
        if (obj.GetField("tPr") != null)
        {
            if (obj.GetField("tPr").type == JSONObject.Type.NUMBER)
            {
                this.TimePRCost = Convert.ToUInt32(obj.GetField("tPr").n);
            }
            else
            {
                this.TimePRCost = Convert.ToUInt32(obj.GetField("tPr").str);
            }
        }
        if (obj.GetField("tPp") != null)
        {
            if (obj.GetField("tPp").type == JSONObject.Type.NUMBER)
            {
                this.TimePPVPCost = Convert.ToUInt32(obj.GetField("tPp").n);
            }
            else
            {
                this.TimePPVPCost = Convert.ToUInt32(obj.GetField("tPp").str);
            }
        }
        if (this.isTime1)
        {
            this.selectedDuration = DurationType.DAY;
        }
        else if (this.isTime7)
        {
            this.selectedDuration = DurationType.WEEK;
        }
        else if (this.isTime30)
        {
            this.selectedDuration = DurationType.MONTH;
        }
        else if (this.isTimeP)
        {
            this.selectedDuration = DurationType.PERMANENT;
        }
    }

    public ShopCost(JSONNode obj)
    {
        this.shop_cost_id = Convert.ToUInt32(obj["sc_id"].AsInt);
        if (obj["t1v"] != (object)null)
        {
            this.Time1VCost = Convert.ToUInt32(obj["t1v"].AsInt);
        }
        if (obj["t1r"] != (object)null)
        {
            this.Time1RCost = Convert.ToUInt32(obj["t1r"].AsInt);
        }
        if (obj["t1p"] != (object)null)
        {
            this.Time1PVPCost = Convert.ToUInt32(obj["t1p"].AsInt);
        }
        if (obj["t7v"] != (object)null)
        {
            this.Time7VCost = Convert.ToUInt32(obj["t7v"].AsInt);
        }
        if (obj["t7r"] != (object)null)
        {
            this.Time7RCost = Convert.ToUInt32(obj["t7r"].AsInt);
        }
        if (obj["t7p"] != (object)null)
        {
            this.Time7PVPCost = Convert.ToUInt32(obj["t7p"].AsInt);
        }
        if (obj["t30v"] != (object)null)
        {
            this.Time30VCost = Convert.ToUInt32(obj["t30v"].AsInt);
        }
        if (obj["t30r"] != (object)null)
        {
            this.Time30RCost = Convert.ToUInt32(obj["t30r"].AsInt);
        }
        if (obj["t30p"] != (object)null)
        {
            this.Time30PVPCost = Convert.ToUInt32(obj["t30p"].AsInt);
        }
        if (obj["tPv"] != (object)null)
        {
            this.TimePVCost = Convert.ToUInt32(obj["tPv"].AsInt);
        }
        if (obj["tPr"] != (object)null)
        {
            this.TimePRCost = Convert.ToUInt32(obj["tPr"].AsInt);
        }
        if (obj["tPp"] != (object)null)
        {
            this.TimePPVPCost = Convert.ToUInt32(obj["tPp"].AsInt);
        }
        if (this.isTime1)
        {
            this.selectedDuration = DurationType.DAY;
        }
        else if (this.isTime7)
        {
            this.selectedDuration = DurationType.WEEK;
        }
        else if (this.isTime30)
        {
            this.selectedDuration = DurationType.MONTH;
        }
        else if (this.isTimeP)
        {
            this.selectedDuration = DurationType.PERMANENT;
        }
    }

    public string Time1_ToString()
    {
        return string.Format("{0} {1}", (this.time1_vcost != 0) ? this.time1_vcost : this.time1_rcost, (this.time1_vcost != 0) ? "credit" : "thorium");
    }

    public string Time7_ToString()
    {
        return string.Format("{0} {1}", (this.time7_vcost != 0) ? this.time7_vcost : this.time7_rcost, (this.time7_vcost != 0) ? "credit" : "thorium");
    }

    public string Time30_ToString()
    {
        return string.Format("{0} {1}", (this.time30_vcost != 0) ? this.time30_vcost : this.time30_rcost, (this.time30_vcost != 0) ? "credit" : "thorium");
    }

    public string TimeP_ToString()
    {
        return string.Format("{0} {1}", (this.timeP_vcost != 0) ? this.timeP_vcost : this.timeP_rcost, (this.timeP_vcost != 0) ? "credit" : "thorium");
    }

    public void GUILayotDraw(float space, bool isSmall)
    {
        GUISkin skin = GUI.skin;
        GUI.skin = GUISkinManager.Main;
        uint num = 0u;
        uint num2 = 0u;
        uint num3 = 0u;
        if (this.isTime1)
        {
            if (this.Time1VCost != 0)
            {
                num = this.Time1VCost;
            }
            if (this.Time1RCost != 0)
            {
                num2 = this.Time1RCost;
            }
            if (this.Time1PVPCost != 0)
            {
                num3 = this.Time1PVPCost;
            }
        }
        else if (this.isTime7)
        {
            if (this.Time7VCost != 0)
            {
                num = this.Time7VCost;
            }
            if (this.Time7RCost != 0)
            {
                num2 = this.Time7RCost;
            }
            if (this.Time7PVPCost != 0)
            {
                num3 = this.Time7PVPCost;
            }
        }
        else if (this.isTime30)
        {
            if (this.Time30VCost != 0)
            {
                num = this.Time30VCost;
            }
            if (this.Time30RCost != 0)
            {
                num2 = this.Time30RCost;
            }
            if (this.Time30PVPCost != 0)
            {
                num3 = this.Time30PVPCost;
            }
        }
        else if (this.isTimeP)
        {
            if (this.TimePVCost != 0)
            {
                num = this.TimePVCost;
            }
            if (this.TimePRCost != 0)
            {
                num2 = this.TimePRCost;
            }
            if (this.TimePPVPCost != 0)
            {
                num3 = this.TimePPVPCost;
            }
        }
        if (num != 0)
        {
            GUILayout.Label(num.ToString(), (!isSmall) ? "vcurrency" : "vcurrencySmall");
            GUILayout.Space(space);
        }
        if (num2 != 0)
        {
            GUILayout.Label(num2.ToString(), (!isSmall) ? "rcurrency" : "rcurrencySmall");
        }
        if (num3 != 0)
        {
            GUILayout.Label(num3.ToString(), (!isSmall) ? "pvpcurrency" : "pvpcurrencySmall");
        }
        GUI.skin = skin;
    }

    public override string ToString()
    {
        string empty = string.Empty;
        empty += string.Format("this.isTime1: {0} | this.Time1VCost: {1} | this.Time1RCost: {2} | this.Time1PVPCost: {2}\n", this.isTime1, this.Time1VCost, this.Time1RCost, this.Time1PVPCost);
        empty += string.Format("this.isTime7: {0} | this.Time7VCost: {1} | this.Time7RCost: {2} | this.Time7PVPCost: {2}\n", this.isTime7, this.Time7VCost, this.Time7RCost, this.Time7PVPCost);
        empty += string.Format("this.isTime30: {0} | this.Time30VCost: {1} | this.Time30RCost: {2} | this.Time30PVPCost: {2}\n", this.isTime30, this.Time30VCost, this.Time30RCost, this.Time30PVPCost);
        return empty + string.Format("this.isTimeP: {0} | this.TimePVCost: {1} | this.TimePRCost: {2} | this.TimePPVPCost: {2}\n", this.isTimeP, this.TimePVCost, this.TimePRCost, this.TimePPVPCost);
    }
}


