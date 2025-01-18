// ILSpyBased#2
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNameManager
{
    public enum NameState
    {
        Valid = 1,
        NotValid,
        Checking
    }

    public delegate void OnCheck(NameState state, string name, string[] alternameName);

    public delegate void OnConfirmComplete();

    public delegate void OnConfirm(bool valid);

    private static ChangeNameManager hInstance;

    public static int CHANGE_NAME_COST = 500;

    private OnCheck onCheckCallback;

    private OnConfirmComplete onConfirmCompleteCallback;

    private NameState currentNameState = NameState.NotValid;

    private string currentCheckingName = string.Empty;

    private ErrorInfo.CODE currentLastError;

    private string userName = string.Empty;

    public static ChangeNameManager Instance
    {
        get
        {
            if (ChangeNameManager.hInstance == null)
            {
                ChangeNameManager.hInstance = new ChangeNameManager();
            }
            return ChangeNameManager.hInstance;
        }
    }

    public OnCheck OnCheckCallback
    {
        get
        {
            return this.onCheckCallback;
        }
        set
        {
            this.onCheckCallback = value;
        }
    }

    public static OnConfirmComplete OnConfirmCompleteCallback
    {
        get
        {
            return ChangeNameManager.Instance.onConfirmCompleteCallback;
        }
        set
        {
            ChangeNameManager.Instance.onConfirmCompleteCallback = value;
        }
    }

    public NameState CurrentNameState
    {
        get
        {
            return this.currentNameState;
        }
    }

    public string CurrentCheckingName
    {
        get
        {
            return this.currentCheckingName;
        }
    }

    public ErrorInfo.CODE CurrentLastError
    {
        get
        {
            return this.currentLastError;
        }
    }

    public string UserName
    {
        get
        {
            return this.userName;
        }
        set
        {
            if (this.CurrentNameState != NameState.Checking)
            {
                this.userName = value;
            }
        }
    }

    public static void CheckName(string name)
    {
        name = name.Trim();
        if (!(name == string.Empty))
        {
            ChangeNameManager.Instance.currentNameState = NameState.Checking;
            AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.CHECK_NAME_URL + "&v=" + name + "&ve=" + WWW.EscapeURL(name), name);
            ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ChangeNameManager.OnCheckNameInner);
            Ajax.Request(ajaxRequest);
        }
    }

    private static void OnCheckNameInner(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
        NameState state = NameState.NotValid;
        List<string> list = new List<string>();
        if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
        {
            state = NameState.Valid;
        }
        if (jSONObject.GetField("names") != null && jSONObject.GetField("names").type == JSONObject.Type.ARRAY)
        {
            for (int i = 0; i < jSONObject.GetField("names").Count; i++)
            {
                if (jSONObject.GetField("names")[i].type != 0)
                {
                    list.Add(jSONObject.GetField("names")[i].str);
                }
            }
        }
        if (jSONObject.GetField("err") != null && jSONObject.GetField("err").type == JSONObject.Type.ARRAY)
        {
            ChangeNameManager.Instance.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("err")[0].n);
        }
        else
        {
            ChangeNameManager.Instance.currentLastError = ErrorInfo.CODE.NONE;
        }
        ChangeNameManager.Instance.currentNameState = state;
        ChangeNameManager.Instance.currentCheckingName = request.Tag.ToString();
        if (ChangeNameManager.Instance.onCheckCallback != null)
        {
            ChangeNameManager.Instance.onCheckCallback(state, request.Tag.ToString(), list.ToArray());
        }
    }

    public static void ChangeName(string name)
    {
        name = name.Trim();
        if (!(name == string.Empty))
        {
            ChangeNameManager.Instance.currentNameState = NameState.Checking;
            AjaxRequest ajaxRequest = new AjaxRequest(WebUrls.CHANGE_NAME_PAYED_URL + "&v=" + name + "&ve=" + WWW.EscapeURL(name), name);
            ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(ChangeNameManager.OnChangeNameInner);
            Ajax.Request(ajaxRequest);
        }
    }

    private static void OnChangeNameInner(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(Ajax.DecodeUtf(result.ToString()));
        NameState nameState = NameState.NotValid;
        List<string> list = new List<string>();
        if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
        {
            nameState = NameState.Valid;
        }
        if (jSONObject.GetField("names") != null && jSONObject.GetField("names").type == JSONObject.Type.ARRAY)
        {
            for (int i = 0; i < jSONObject.GetField("names").Count; i++)
            {
                if (jSONObject.GetField("names")[i].type != 0)
                {
                    list.Add(jSONObject.GetField("names")[i].str);
                }
            }
        }
        if (jSONObject.GetField("err") != null && jSONObject.GetField("err").type == JSONObject.Type.ARRAY)
        {
            ChangeNameManager.Instance.currentLastError = (ErrorInfo.CODE)Convert.ToInt32(jSONObject.GetField("err")[0].n);
        }
        else
        {
            ChangeNameManager.Instance.currentLastError = ErrorInfo.CODE.NONE;
        }
        ChangeNameManager.Instance.currentNameState = nameState;
        ChangeNameManager.Instance.currentCheckingName = request.Tag.ToString();
        if (ChangeNameManager.Instance.onConfirmCompleteCallback != null && nameState == NameState.Valid)
        {
            ChangeNameManager.Instance.onConfirmCompleteCallback();
            LocalUser.Money -= ChangeNameManager.CHANGE_NAME_COST;
            LocalUser.Name = request.Tag.ToString();
            GameLogicServerNetworkController.SendChange(11);
        }
    }
}


