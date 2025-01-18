// ILSpyBased#2
using UnityEngine;

public class Action
{
    public delegate void ActionClick(object param);

    private ActionClick onClick;

    private Texture2D ico;

    private string buttonText = string.Empty;

    private string url = string.Empty;

    private object onClickParam;

    private bool isLoaded;

    public Texture2D Ico
    {
        get
        {
            return this.ico;
        }
    }

    public string ButtonText
    {
        get
        {
            return this.buttonText;
        }
    }

    public bool IsLoaded
    {
        get
        {
            return this.isLoaded;
        }
    }

    public Action(Texture2D ico, string buttonText, ActionClick callback, object callbackParam)
    {
        this.ico = ico;
        this.buttonText = buttonText;
        this.onClick = callback;
        this.isLoaded = true;
        this.onClickParam = callbackParam;
    }

    public Action(string url, string buttonText, ActionClick callback, object callbackParam)
    {
        Ajax.Request(url, new AjaxRequest.AjaxHandler(this.OnLoading), AjaxRequest.DataType.Image);
        this.buttonText = buttonText;
        this.onClick = callback;
        this.url = url;
        this.onClickParam = callbackParam;
    }

    public Action(Texture2D ico)
    {
        this.ico = ico;
    }

    public Action(string url)
    {
        Ajax.Request(url, new AjaxRequest.AjaxHandler(this.OnLoading), AjaxRequest.DataType.Image);
        this.url = url;
    }

    private void OnLoading(object res, AjaxRequest request)
    {
        this.ico = (res as Texture2D);
        this.isLoaded = true;
    }

    public void ReDownload()
    {
        if (this.url != string.Empty)
        {
            UnityEngine.Debug.Log("[Action] ReDownload: " + this.url);
            Ajax.Request(this.url, new AjaxRequest.AjaxHandler(this.OnLoading), AjaxRequest.DataType.Image);
        }
    }

    public void Click()
    {
        if (this.onClick != null)
        {
            if (this.onClickParam == null)
            {
                this.onClick(this);
            }
            else
            {
                JSONObject jSONObject = new JSONObject(this.onClickParam.ToString());
                string name = Configuration.SType.ToString().ToLower();
                if (jSONObject.GetField(name) != null)
                {
                    this.onClick(jSONObject.GetField(name).str);
                }
                else
                {
                    this.onClick(this.onClickParam);
                }
            }
        }
        WebCall.Analitic("Action_Click", string.Format("Url:{0}", this.url));
    }
}


