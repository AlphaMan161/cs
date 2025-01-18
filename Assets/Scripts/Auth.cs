// ILSpyBased#2
using System;

public class Auth
{
    public delegate void AuthEventHandler(object sender);

    private uint user_id;

    private string key;

    private static Auth mInstance;

    public static Auth Instance
    {
        get
        {
            if (Auth.mInstance == null)
            {
                Auth.mInstance = new Auth();
            }
            return Auth.mInstance;
        }
    }

    public static uint UserID
    {
        get
        {
            return Auth.Instance.user_id;
        }
    }

    public static string Key
    {
        get
        {
            return Auth.Instance.key;
        }
    }

    public event AuthEventHandler OnValid;

    public event AuthEventHandler OnError;

    public event AuthEventHandler OnLogin;

    public event AuthEventHandler OnLoginError;

    public static void Check()
    {
        Ajax.Request(WebUrls.AUTH_CHECK_URL, new AjaxRequest.AjaxHandler(Auth.Instance.OnCheck));
    }

    private void OnCheck(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(result.ToString());
        if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
        {
            this.user_id = Convert.ToUInt32(jSONObject.GetField("user_id").str);
            this.key = jSONObject.GetField("key").str;
            if (this.OnValid != null)
            {
                this.OnValid(this);
            }
            return;
        }
        if (jSONObject.GetField("error") == null)
        {
            return;
        }
        if (this.OnError != null)
        {
            this.OnError(jSONObject.GetField("error").str);
            return;
        }
        int num = 0;
        if (jSONObject.GetField("error").type == JSONObject.Type.STRING)
        {
            num = Convert.ToInt32(jSONObject.GetField("error").str);
        }
        else if (jSONObject.GetField("error").type == JSONObject.Type.NUMBER)
        {
            num = Convert.ToInt32(jSONObject.GetField("error").n);
        }
        if (num != 1)
        {
            return;
        }
        throw new AuthException("Invalide auth " + WebUrls.AUTH_CHECK_URL);
    }

    public static void Login(string email, string password)
    {
        Ajax.Request(string.Format("{0}&email={1}&password={2}", WebUrls.LOGIN_URL, email, password), new AjaxRequest.AjaxHandler(Auth.Instance.OnLoginRequest));
    }

    private void OnLoginRequest(object result, AjaxRequest request)
    {
        JSONObject jSONObject = new JSONObject(result.ToString());
        if (jSONObject.GetField("result").type == JSONObject.Type.BOOL && jSONObject.GetField("result").b)
        {
            JSONObject field = jSONObject.GetField("auth");
            this.user_id = Convert.ToUInt32(field.GetField("id").n);
            this.key = field.GetField("key").str;
            Configuration.SessionAuth = string.Format("ccid={0}&cckey={1}&", this.user_id, this.key);
            if (this.OnLogin != null)
            {
                this.OnLogin(this);
            }
            return;
        }
        if (jSONObject.GetField("error") == null)
        {
            return;
        }
        if (this.OnLoginError != null)
        {
            this.OnLoginError(jSONObject.GetField("error").str);
            return;
        }
        int num = 0;
        if (jSONObject.GetField("error").type == JSONObject.Type.STRING)
        {
            num = Convert.ToInt32(jSONObject.GetField("error").str);
        }
        else if (jSONObject.GetField("error").type == JSONObject.Type.NUMBER)
        {
            num = Convert.ToInt32(jSONObject.GetField("error").n);
        }
        if (num != 1)
        {
            return;
        }
        throw new AuthException("Invalide auth " + WebUrls.AUTH_CHECK_URL);
    }
}


