// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLEvent
{
    public enum GLEventType
    {
        NewLevel = 10,
        DailyBonus
    }

    public delegate void EventHandler(GLEvent obj, string msg);

    private int event_id;

    private GLEventType event_type;

    private int user_id;

    public JSONObject EventData;

    public Hashtable eventItems = new Hashtable();

    public int EventID
    {
        get
        {
            return this.event_id;
        }
    }

    public GLEventType Type
    {
        get
        {
            return this.event_type;
        }
    }

    public int UserID
    {
        get
        {
            return this.user_id;
        }
    }

    public event EventHandler OnConfirm;

    public event EventHandler OnConfirmError;

    public GLEvent(JSONObject obj)
    {
        if (obj.type != JSONObject.Type.OBJECT)
        {
            throw new Exception("CLEvent init \nInput object not a valid data \n Type is " + obj.type.ToString());
        }
        this.event_id = Convert.ToInt32(obj.GetField("i").str);
        this.event_type = (GLEventType)Convert.ToInt32(obj.GetField("et").str);
        this.user_id = Convert.ToInt32(obj.GetField("uid").str);
        try
        {
            this.EventData = new JSONObject(obj.GetField("d").str);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log(ex.ToString());
        }
        GLEventType gLEventType = this.event_type;
        int num2;
        int num;
        if (gLEventType == GLEventType.DailyBonus)
        {
            if (obj.GetField("d") == null)
            {
                return;
            }
            num = 1;
            num2 = 0;
            num = ((obj.GetField("d").GetField("d").type != JSONObject.Type.NUMBER) ? Convert.ToInt32(obj.GetField("d").GetField("d").str) : ((int)obj.GetField("d").GetField("d").n));
            if (obj.GetField("d").GetField("vcur") != null)
            {
                if (obj.GetField("d").GetField("vcur").type != JSONObject.Type.NUMBER)
                {
                    if (obj.GetField("d").GetField("vcur").type == JSONObject.Type.STRING)
                    {
                        num2 = Convert.ToInt32(obj.GetField("d").GetField("vcur").str);
                        goto IL_01f7;
                    }
                    throw new Exception("CLEvent::CLEvent unkown vcurrency type");
                }
                num2 = Convert.ToInt32(obj.GetField("d").GetField("vcur").n);
            }
            goto IL_01f7;
        }
        UnityEngine.Debug.LogError("CLEvent: unkown event type");
        return;
        IL_01f7:
        this.eventItems.Add(1, num2);
        this.eventItems.Add(2, num);
    }

    public void Confirm(object obj)
    {
        AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.EVENT_CONFIRM + "&ei=" + this.event_id + "&et=" + (int)this.event_type, this);
        ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(this.OnConfirmRequest);
        Ajax.Request(ajaxRequest);
    }

    private void OnConfirmRequest(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(result.ToString());
        if (jSONObject.GetField("result") != null && jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
        {
            GLEventType type = this.Type;
            if (type == GLEventType.DailyBonus)
            {
                LocalUser.Money += Convert.ToInt32(this.eventItems[1]);
            }
            if (this.OnConfirm != null)
            {
                this.OnConfirm(this, string.Empty);
            }
        }
        else
        {
            UnityEngine.Debug.LogError("[GLEvent] OnConfirmError request: " + result.ToString());
            if (this.OnConfirmError != null)
            {
                this.OnConfirmError(this, "[GLEvent] OnConfirmError request: " + result.ToString());
            }
        }
    }

    public static List<GLEvent> FromArray(JSONObject json_items)
    {
        List<GLEvent> list = new List<GLEvent>();
        if (json_items != null && json_items.type == JSONObject.Type.ARRAY)
        {
            for (int i = 0; i < json_items.Count; i++)
            {
                if (json_items[i].type != 0)
                {
                    list.Add(new GLEvent(json_items[i]));
                }
            }
        }
        return list;
    }
}


