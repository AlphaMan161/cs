using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;

public class AuthManager : MonoBehaviour {

    public GameObject AutoUpdateG;
    public GameObject MainG;
    public InputField FullIF;
    public InputField TimeIF;
    public InputField GetFullIF;
    public static string sessionAuth;
    public static string Type;
    public Toggle SaveFull;
    public Toggle SaveTime;
    public Toggle HideFull;
    public Toggle HideTime;
    public Text Version;
    public Text Platform;
    public string VersionC;
    public string VersionC2;
    public string VersionSec;
    public string LinkSec = "https://docs.google.com/document/d/1FzBCog5JoIAb6hRKMadnBAdhfQC6deAlS1syjcaAiGg/edit";
    public string UpdatePCVersion;
    public string UpdateMACVersion;
    public string VersionCheck;
    public static string AssetBandle;
    public string result;
    public bool Work;

    IEnumerator Start()
    {
        WWW www = new WWW(LinkSec);
        yield return www;
        result = www.text;
        //UpdatePC
        int BVPC = result.LastIndexOf("╔") + 1;
        int EVPC = result.IndexOf("╝", BVPC);
        UpdatePCVersion = "http://" + result.Substring(BVPC, EVPC - BVPC);
        Debug.Log(UpdatePCVersion);
        //UpdateMAC
        int BVMAC = result.LastIndexOf("◄") + 1;
        int EVMAC = result.IndexOf("►", BVMAC);
        UpdateMACVersion = "http://" + result.Substring(BVMAC, EVMAC - BVMAC);
        Debug.Log(UpdateMACVersion);
        //VersionCheak
        int BVVC = result.LastIndexOf("ソ") + 1;
        int EVVC = result.IndexOf("ッ", BVVC);
        // VersionCheck = "http://" + result.Substring(BVVC, EVVC - BVVC);
        VersionCheck = result.Substring(BVVC, EVVC - BVVC);
        Debug.Log(VersionCheck);
        //AssetBandle
        int BAB = result.LastIndexOf("▼") + 1;
        int EAB = result.IndexOf("▽", BAB);
        AssetBandle = "http://" + result.Substring(BAB, EAB - BAB);
        Debug.Log(AssetBandle);

        //  WWW www2 = new WWW(VersionCheck);
        //  yield return www2;
        //  VersionC2 = www2.text;
        VersionC2 = VersionCheck;
        Check();
    }

    void Check()
    {
        float VersionC2I = float.Parse(VersionC2);
        float VersionCI = float.Parse(VersionC);
        float VersionC3I = float.Parse(VersionSec);
        if (VersionC2I > VersionCI)
        {
            Work = false;
            Debug.Log("Ваша версия устарела");
            AutoUpdate();
        }
        else
        {
            Work = true;
            MainG.SetActive(true);
            Debug.Log("У вас установлена последняя версия");
        }
    }
    void AutoUpdate()
    {
        AutoUpdateG.SetActive(true);
    }
    public void DownLoadBtn()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.OpenURL(UpdatePCVersion);
        }
        if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            Application.OpenURL(UpdateMACVersion);
        }
    }
    void Awake() {
        VersionC = "1.03";
        Version.text = VersionC + "v";
        FullIF.text = PlayerPrefs.GetString("FullLink");
        TimeIF.text = PlayerPrefs.GetString("TimeLink");
        if (PlayerPrefs.GetInt("TimeLinkActive") == 1)
        {
            SaveTime.isOn = true;
        }
        else
        {
            SaveTime.isOn = false;
        }
        if (PlayerPrefs.GetInt("FullLinkActive") == 1)
        {
            SaveFull.isOn = true;
        }
        else
        {
            SaveFull.isOn = false;
        }

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Platform.text = "Windows";
        }
        if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            Platform.text = "MAC OS";
        }
    }
    public void AdminBtn(Button btn)
    {
        Application.OpenURL("http://vk.com/id" + btn.name);
    }
    public void VkBtn()
    {
        Application.OpenURL("http://vk.com/clientcc");
    }
    public void YouTubeBtn()
    {
        Application.OpenURL("https://youtu.be/rKhz03BR61Q");
    }

    public void Update() {
        if (HideFull.isOn == true)
        {
            FullIF.textComponent.gameObject.SetActive(false);
        }
        else { 
            FullIF.textComponent.gameObject.SetActive(true);
        }
        if (HideTime.isOn == true)
        {
            TimeIF.textComponent.gameObject.SetActive(false);
        }
        else
        {
            TimeIF.textComponent.gameObject.SetActive(true);
        }
    }

    public void LunchFull() {
        if (FullIF.text != null && Work == true)
        {
            string url = DecodeString.ToDecodeString(FullIF.text);
            HttpWebRequest tQ = (HttpWebRequest)HttpWebRequest.Create(url);
            tQ.Referer = url;
            HttpWebResponse tS = (HttpWebResponse)tQ.GetResponse();
            Debug.Log(url);

            if (url.Contains("vk"))
            {
                Type = "vk";
            }
            else if (url.Contains("od"))
            {
                Type = "od";
            }
            else if (url.Contains("mm"))
            {
                Type = "mm";
            }

            string tC = tS.Headers["Set-Cookie"];
            UnityEngine.Debug.Log(tC);
            // Instance ccid
            int numIdB = tC.LastIndexOf("ccid=") + 5;
            int numIdE = tC.IndexOf(";", numIdB);
            UnityEngine.Debug.Log(tC.Substring(numIdB, numIdE - numIdB));
            string ccId = tC.Substring(numIdB, numIdE - numIdB);

            //Instance cckey
            int numKeyB = tC.LastIndexOf("cckey=") + 6;
            int numKeyE = tC.IndexOf(";", numKeyB);
            UnityEngine.Debug.Log(tC.Substring(numKeyB, numKeyE - numKeyB));
            string ccKey = tC.Substring(numKeyB, numKeyE - numKeyB);

            if (SaveFull.isOn == true)
            {
                PlayerPrefs.SetString("FullLink", url);
                PlayerPrefs.SetInt("FullLinkActive", 1);
            }
            else
            {
                PlayerPrefs.SetString("FullLink", null);
                PlayerPrefs.SetInt("FullLinkActive", 0);
            }

            sessionAuth = string.Format("ccid={0}&cckey={1}&", ccId, ccKey);
            Debug.Log(sessionAuth);
            Application.LoadLevel("FirstLoading");
        }
    }
    public void LunchTime()
    {
        if (FullIF.text != null && Work == true)
        {
            string urlTime = DecodeString.ToDecodeString(TimeIF.text);
            Debug.Log(urlTime);
            if (urlTime.Contains("vk"))
            {
                Type = "vk";
            }
            else if (urlTime.Contains("od"))
            {
                Type = "od";
            }
            else if (urlTime.Contains("mm"))
            {
                Type = "mm";
            }
            // Instance ccid
            int numIdB = urlTime.LastIndexOf("ccid=") + 5;
            int numIdE = urlTime.IndexOf("&", numIdB);
            Debug.Log(urlTime.Substring(numIdB, numIdE - numIdB));
            string ccId = urlTime.Substring(numIdB, numIdE - numIdB);

            //Instance cckey
            int numKeyB = urlTime.LastIndexOf("cckey=") + 6;
            int numKeyE = urlTime.IndexOf("&", numKeyB);
            Debug.Log(urlTime.Substring(numKeyB, numKeyE - numKeyB));
            string ccKey = urlTime.Substring(numKeyB, numKeyE - numKeyB);

            if (SaveTime.isOn == true)
            {
                PlayerPrefs.SetString("TimeLink", urlTime);
                PlayerPrefs.SetInt("TimeLinkActive", 1);
            }
            else
            {
                PlayerPrefs.SetString("TimeLink", null);
                PlayerPrefs.SetInt("TimeLinkActive", 0);
            }

            sessionAuth = string.Format("ccid={0}&cckey={1}&", ccId, ccKey);
            Debug.Log(sessionAuth);

            Application.LoadLevel("FirstLoading");
        }
    }
    public void GetTime()
    {
        if (FullIF.text != null && Work == true)
        {
            string Geturl = DecodeString.ToDecodeString(GetFullIF.text);
            HttpWebRequest tQ = (HttpWebRequest)HttpWebRequest.Create(Geturl);
            tQ.Referer = Geturl;
            HttpWebResponse tS = (HttpWebResponse)tQ.GetResponse();
            Debug.Log(Geturl);
            if (Geturl.Contains("vk")) {
                Type = "vk";
            } else if (Geturl.Contains("od"))
            {
                Type = "od";
            } else if (Geturl.Contains("mm")) {
                Type = "mm";
            }
            string tC = tS.Headers["Set-Cookie"];
            UnityEngine.Debug.Log(tC);
            // Instance ccid
            int numIdB = tC.LastIndexOf("ccid=") + 5;
            int numIdE = tC.IndexOf(";", numIdB);
            UnityEngine.Debug.Log(tC.Substring(numIdB, numIdE - numIdB));
            string ccId = tC.Substring(numIdB, numIdE - numIdB);

            //Instance cckey
            int numKeyB = tC.LastIndexOf("cckey=") + 6;
            int numKeyE = tC.IndexOf(";", numKeyB);
            UnityEngine.Debug.Log(tC.Substring(numKeyB, numKeyE - numKeyB));
            string ccKey = tC.Substring(numKeyB, numKeyE - numKeyB);
            
            GetFullIF.text = string.Format("https://" + Type + "-contra.pentagames.net/ajax.php?ccid={0}&cckey={1}&", ccId, ccKey);
        }
    }
}
public static class DecodeString
{
    public static string ToDecodeString(this string x)
    {
        if (x.Contains("%2B"))
        {
            x = x.Replace("%2B", "+");
        }
        if (x.Contains("%2F"))
        {
            x = x.Replace("%2F", "/");
        }
        if (x.Contains("%2C"))
        {
            x = x.Replace("%2C", ",");
        }
        if (x.Contains("%3D"))
        {
            x = x.Replace("%3D", "=");
        }
        if (x.Contains("%3A")) {
            x = x.Replace("%3A", ":");
        }
        if (x.Contains("%3F"))
        {
            x = x.Replace("%3F", "?");
        }
        if (x.Contains("&cc_key="))
        {
            x = x.Replace("&cc_key=", "");
        }
        if (x.Contains("%26"))
        {
            x = x.Replace("%26", "&");
        }
        if (x.Contains("https%3A%2F%2F")) 
        {
            x = x.Replace("https%3A%2F%2F", "https://");
        }
        if (x.Contains("%252"))
        {
            x = x.Replace("%252", "%2");
        }
        if (x.Contains("https://vk.com/away.php?to=https"))
        {
            x = x.Replace("https://vk.com/away.php?to=https", "http");
        }
        if (x.Contains("https://vk.com/away.php?utf=1&to=https"))
        {
            x = x.Replace("https://vk.com/away.php?utf=1&to=https", "http");
        }
        if (x.Contains("https://vk.com/away.php?utf=1&to=http"))
        {
            x = x.Replace("https://vk.com/away.php?utf=1&to=https", "http");
        }
        if (x.Contains("https://vk.com/away.php?to=http"))
        {
            x = x.Replace("https://vk.com/away.php?to=http", "http");
        }
        else if (x.Contains("https")) {
            if (x.IndexOf("https") == 0) {
                x = x.Replace("https", "http");
            }
        }
        return x;
    }
}
