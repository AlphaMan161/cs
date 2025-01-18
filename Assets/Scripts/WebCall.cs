// ILSpyBased#2
using UnityEngine;

public class WebCall : MonoBehaviour
{
    private static WebCall mInstance;

    private static WebCall Instance
    {
        get
        {
            if ((Object)WebCall.mInstance == (Object)null)
            {
                WebCall.mInstance = (new GameObject("WebCall").AddComponent(typeof(WebCall)) as WebCall);
            }
            return WebCall.mInstance;
        }
    }

    public static void Init()
    {
        WebCall instance = WebCall.Instance;
    }

    public static void BuyMoney(object obj)
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
        }
        if (Configuration.EnableExternal)
        {
            Application.ExternalEval(string.Format("OpenPayWindow({0});", LocalUser.Money));
        }
    }

    public static void BankPackage(object obj)
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
        }
        if (Configuration.EnableExternal)
        {
            Application.ExternalCall("Bank.ShowPackage", obj);
        }
    }

    public static void Analitic(string category, string action, params object[] args)
    {
        string script = string.Format("Analitics('{0}', '{1}', '{2}', '{3}', '{4}');", category, action, string.Format("{0} ({1})", LocalUser.Name, LocalUser.UserID), (args.Length <= 0) ? null : args[0], (args.Length <= 1) ? null : args[1]);
        if (Configuration.EnableExternal)
        {
            Application.ExternalEval(script);
        }
    }

    public static void GameAnalitic(string name, int point)
    {
        if (Configuration.EnableGMAnalytic)
        {
            string script = string.Format("GMAnal('{0}', {1});", name, point);
            Application.ExternalEval(script);
        }
    }

    public void UpdateMoney()
    {
        LocalUser.RefreshLite();
    }

    public void EnableOffer()
    {
        UnityEngine.Debug.Log("WebCall.EnableOffer");
        ActionRotater.AddOfferAction();
    }

    public void OnChangeLang(string lang)
    {
        UnityEngine.Debug.LogError("WebCall.OnChangeLang input lang: " + lang);
        LanguageManager.ChangeLang(lang, false);
    }

    public static void ChangeLang(string lang)
    {
        Application.ExternalEval(string.Format("OnChangeLang('{0}');", lang));
    }

    public void SetImageUploadUrl(string uploadDir)
    {
        ScreenshotManager.UploadUrl = uploadDir;
    }

    public void SetImageUploadName(string name)
    {
        ScreenshotManager.FileName = name;
    }

    public void SetWallImageUploadUrl(string uploadDir)
    {
        WallManager.UploadUrl = uploadDir;
    }

    public void ViewFriend(string uid)
    {
        StatisticManager.View(uid, Configuration.SType);
    }

    public static void NeedWallUploadUrl()
    {
        if (Configuration.EnableExternal)
        {
            Application.ExternalEval("Wall.GetUploadUrl();");
        }
    }

    public static void NeedUploadUrl()
    {
        if (Configuration.EnableExternal)
        {
            Application.ExternalEval("ScreenShots.TakeScreenShot();");
        }
    }

    public static void ScreenUploaded(string response)
    {
        if (Configuration.EnableExternal)
        {
            Application.ExternalEval(string.Format("ScreenShots.UploadDone({0});", response));
        }
    }

    public static void WallUploaded(string response)
    {
        if (Configuration.EnableExternal)
        {
            Application.ExternalEval(string.Format("Wall.UploadDone({0});", response));
        }
    }

    public static void CheckOffer()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
        }
        if (Configuration.EnableExternal)
        {
            Application.ExternalEval("Social.Init.Offer();");
        }
    }

    public static void InviteFriend(object obj)
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
        }
        if (Configuration.EnableExternal)
        {
            Application.ExternalEval("Social.Click.Invite();");
        }
    }

    public static void OpenOfferWindow(object obj)
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
        }
        if (Configuration.EnableExternal)
        {
            Application.ExternalEval("Social.Click.OfferWindow();");
        }
    }

    public static void OpenUrl(object url)
    {
        if (Configuration.EnableExternal)
        {
            Application.ExternalEval("UnityCommon.OpenUrl(\"" + url.ToString() + "\");");
        }
    }
}


