// ILSpyBased#2
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;

public class Ajax : MonoBehaviour
{
    public static int ERROR_ATTEMPT = 5;

    public static float ERROR_WAIT_SEC = 3f;

    private static Ajax mInstance;

    private Dictionary<string, AjaxRequest> requestsList = new Dictionary<string, AjaxRequest>();

    private object requestListLock = new object();

    public bool complete = true;

    public static Ajax Instance
    {
        get
        {
            if ((UnityEngine.Object)Ajax.mInstance == (UnityEngine.Object)null)
            {
                Ajax.mInstance = (new GameObject("Ajax").AddComponent(typeof(Ajax)) as Ajax);
            }
            return Ajax.mInstance;
        }
    }

    public static bool Complete
    {
        get
        {
            return Ajax.Instance.complete;
        }
    }

    private void Start()
    {
        if ((UnityEngine.Object)Ajax.mInstance != (UnityEngine.Object)null)
        {
            UnityEngine.Object.DontDestroyOnLoad(Ajax.mInstance.gameObject);
        }
    }

    public static void Request(AjaxRequest request)
    {
        if (!request.UseCache)
        {
            request.ClearCache();
        }
        object obj = Ajax.Instance.requestListLock;
        Monitor.Enter(obj);
        try
        {
            if (Ajax.Instance.requestsList.ContainsKey(request.Url))
            {
                UnityEngine.Debug.Log("[Ajax] Request exists url: " + request.Url);
            }
            else
            {
                UnityEngine.Debug.Log("[Ajax] Request url: " + request.Url);
                Ajax.Instance.complete = false;
                Ajax.Instance.requestsList.Add(request.Url, request);
                request.OnError -= new AjaxRequest.AjaxHandler(Ajax.Instance.HandleRequestOnError);
                request.OnError += new AjaxRequest.AjaxHandler(Ajax.Instance.HandleRequestOnError);
                request.OnComplete -= new AjaxRequest.AjaxHandler(Ajax.Instance.HandleRequestOnComplete);
                request.OnComplete += new AjaxRequest.AjaxHandler(Ajax.Instance.HandleRequestOnComplete);
                Ajax.Instance.StartCoroutine(request.ILoad());
            }
        }
        finally
        {
            Monitor.Exit(obj);
        }
    }

    public static void Request(string url, AjaxRequest.AjaxHandler OnComplete)
    {
        AjaxRequest ajaxRequest = new AjaxRequest(url);
        ajaxRequest.OnComplete += OnComplete;
        ajaxRequest.OnError += new AjaxRequest.AjaxHandler(Ajax.Instance.HandleRequestOnError);
        ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(Ajax.Instance.HandleRequestOnComplete);
        Ajax.Request(ajaxRequest);
    }

    public static void Request(string url, AjaxRequest.AjaxHandler OnComplete, AjaxRequest.DataType type)
    {
        UnityEngine.Debug.Log("[Ajax] Request url: " + url);
        if (type != AjaxRequest.DataType.Text)
        {
            AjaxRequest ajaxRequest = new AjaxRequest(url, type);
            ajaxRequest.OnComplete += new AjaxRequest.AjaxHandler(Ajax.Instance.HandleRequestOnComplete);
            ajaxRequest.OnComplete += OnComplete;
            ajaxRequest.OnError += new AjaxRequest.AjaxHandler(Ajax.Instance.HandleRequestOnError);
            Ajax.Request(ajaxRequest);
        }
        else
        {
            Ajax.Request(url, OnComplete);
        }
    }

    private void HandleRequestOnComplete(object result, AjaxRequest request)
    {
        UnityEngine.Debug.Log("[Ajax] Request url: " + request.Url + " complete");
        this.requestsList.Remove(request.Url);
        if (this.requestsList.Count == 0)
        {
            Ajax.Instance.complete = true;
        }
    }

    private void HandleRequestOnError(object result, AjaxRequest request)
    {
        UnityEngine.Debug.LogError("[Ajax] HandleRequestOnError url: " + request.Url + "\n response: " + result);
        WebCall.Analitic("Ajax", "HandleRequestOnError " + request.Url, result);
        if (request.Attempt < Ajax.ERROR_ATTEMPT)
        {
            UnityEngine.Debug.Log("[Ajax] OnError url: " + request.Url + " try again Attempt: " + request.Attempt + " Reason: " + result + " time" + Time.time);
            Ajax.Instance.StartCoroutine(this.AgainRequest(result, request));
        }
        else
        {
            UnityEngine.Debug.LogError("[Ajax] OnError url: " + request.Url + " Attempt: " + request.Attempt + "  Reason: " + result);
            this.requestsList.Remove(request.Url);
            if (this.requestsList.Count == 0)
            {
                Ajax.Instance.complete = true;
            }
            request.ProcessFail();
        }
    }

    private IEnumerator AgainRequest(object reason, AjaxRequest request)
    {
        yield return (object)new WaitForSeconds(Ajax.ERROR_WAIT_SEC);
        request.ClearCache();
        request.Attempt++;
        Ajax.Instance.StartCoroutine(request.ILoad());
    }

    public static void Debugging()
    {
        UnityEngine.Debug.Log("[Ajax] Complete: " + Ajax.Complete);
        Dictionary<string, AjaxRequest>.Enumerator enumerator = Ajax.Instance.requestsList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UnityEngine.Debug.Log("[Ajax] Request in process: " + enumerator.Current.Key);
            }
        }
        finally
        {
            ((IDisposable)enumerator).Dispose();
        }
    }

    public static string DecodeUtf(string inputString)
    {
        string empty = string.Empty;
        int num = 0;
        while (inputString.Contains("\\u"))
        {
            int num2 = inputString.IndexOf("\\u");
            int num3 = num2 + 6;
            if (num2 > -1 && num3 > num2)
            {
                empty = inputString.Substring(num2 + 2, num3 - (num2 + 2));
                num = int.Parse(empty, NumberStyles.AllowHexSpecifier);
                inputString = inputString.Replace("\\u" + empty, Convert.ToChar(num).ToString());
            }
            if (!inputString.Contains("\\u"))
            {
                break;
            }
        }
        return inputString;
    }
}


