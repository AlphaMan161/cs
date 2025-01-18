// ILSpyBased#2
using System.Collections;
using UnityEngine;

public class AjaxRequest
{
    public enum DataType
    {
        Text = 1,
        Image,
        Binary
    }

    public delegate void AjaxHandler(object result, AjaxRequest request);

    private WWW www;

    private string url = string.Empty;

    public DataType type = DataType.Text;

    private bool useCache = true;

    private object tag;

    public int Attempt;

    private bool isCached;

    public float Progress
    {
        get
        {
            if (this.www == null)
            {
                return 0f;
            }
            return this.www.progress;
        }
    }

    public string Url
    {
        get
        {
            return this.url;
        }
    }

    public bool UseCache
    {
        get
        {
            return this.useCache;
        }
    }

    public object Tag
    {
        get
        {
            return this.tag;
        }
    }

    public event AjaxHandler OnComplete;

    public event AjaxHandler OnError;

    public event AjaxHandler OnFail;

    public AjaxRequest(string url)
    {
        this.url = url;
        this.www = new WWW(url);
    }

    public AjaxRequest(string url, DataType type)
    {
        this.url = url;
        this.www = new WWW(url);
        this.type = type;
    }

    public AjaxRequest(string url, object tag)
    {
        this.url = url;
        this.www = new WWW(url);
        this.tag = tag;
    }

    public AjaxRequest(string url, bool useCache)
    {
        this.url = url;
        this.www = new WWW(url);
        this.useCache = useCache;
    }

    public AjaxRequest(string url, bool useCache, DataType type)
    {
        this.url = url;
        this.www = new WWW(url);
        this.useCache = useCache;
        this.type = type;
    }

    public AjaxRequest(string url, bool useCache, object tag)
    {
        this.url = url;
        this.www = new WWW(url);
        this.useCache = useCache;
        this.tag = tag;
    }

    public AjaxRequest(string url, bool useCache, object tag, DataType type)
    {
        this.url = url;
        this.www = new WWW(url);
        this.useCache = useCache;
        this.tag = tag;
        this.type = type;
    }

    public void AddForm(WWWForm form)
    {
        this.www = new WWW(this.url, form);
    }

    public IEnumerator ILoad()
    {
        yield return (object)this.www;
        this.isCached = true;
        if (this.www.error != null)
        {
            if (this.OnError != null)
            {
                this.OnError(this.www.error, this);
            }
        }
        else if (this.OnComplete != null)
        {
            if (this.type == DataType.Image)
            {
                this.OnComplete(this.www.texture, this);
            }
            else if (this.type == DataType.Text)
            {
                this.OnComplete(this.www.text, this);
            }
            else
            {
                this.OnComplete(this.www.bytes, this);
            }
        }
    }

    public void ProcessFail()
    {
        if (this.OnFail != null)
        {
            this.OnFail("Max attemt", this);
        }
    }

    public void ClearCache()
    {
        if (this.isCached)
        {
            this.www.Dispose();
            this.www = new WWW(this.url);
        }
    }
}


